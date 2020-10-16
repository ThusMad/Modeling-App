using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using SceneController.Utility;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

namespace SceneController.Native
{
    public abstract class SceneSwapChain : System.Windows.Controls.Image
    {
        // - field -----------------------------------------------------------------------
        private readonly object _chartLock = new object();

        private Dispatcher _dispatcher;

        private System.Timers.Timer _resizeTimer;

        private bool _allowResize = true;
        private bool _allowRender = false;

        private SharpDX.Direct3D11.Device _device;
        private Texture2D _renderTarget;
        private Dx11ImageSource _d3DSurface;
        private RenderTarget _d2DRenderTarget;
        private SharpDX.Direct2D1.Factory _d2DFactory;

        protected ResourceCache ResCache = new ResourceCache();

        private Texture2DDescription _renderDesc;
        private RenderTargetProperties _renderProperties;


        // - property --------------------------------------------------------------------

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                var isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                return isDesignMode;
            }
        }

        // - public methods --------------------------------------------------------------

        protected SceneSwapChain()
        {
            base.Loaded += Window_Loaded;
            base.Unloaded += Window_Closing;
            //base.SizeChanged += ScatterChart_SizeChanged;

            base.SnapsToDevicePixels = true;
            base.VisualEdgeMode = System.Windows.Media.EdgeMode.Aliased;
            base.Stretch = System.Windows.Media.Stretch.Fill;
           
            InitTimer();
        }

        private void InitTimer()
        {
            _resizeTimer = new System.Timers.Timer();
            _resizeTimer.Interval = 30;
            _resizeTimer.Elapsed += ResizeTimer_Elapsed;
        }

        private void PrepareAndCallRender(Action<RenderTarget> render, RawMatrix3x2 matrix)
        {
            if (_device == null || _d2DRenderTarget == null)
            {
                return;
            }

            _d2DRenderTarget.BeginDraw();
            _d2DRenderTarget.Transform = matrix;
            render(_d2DRenderTarget);
            _d2DRenderTarget.EndDraw();

            _device.ImmediateContext.Flush();
        }

        protected void OnDrawScene(Action<RenderTarget> render, RawMatrix3x2 matrix)
        {
            DispatchRender(render, matrix);
        }

        protected void OnClear(Action<RenderTarget> clear, RawMatrix3x2 matrix)
        {
            DispatchRender(clear, matrix);
        }

        // - event handler ---------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SceneSwapChain.IsInDesignMode)
            {
                return;
            }

            _dispatcher = this.Dispatcher;

            _renderDesc = new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.B8G8R8A8_UNorm,
                Width = 0,
                Height = 0,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            };

            _renderProperties = new RenderTargetProperties(new PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied));

            StartD3D();

            StartRendering();
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            if (SceneSwapChain.IsInDesignMode)
            {
                return;
            }

            _resizeTimer.Stop();
            EndD3D();
        }

        private void DispatchRender(Action<RenderTarget> render, RawMatrix3x2 matrix)
        {
            lock (_chartLock)
            {
                if (_allowRender && _d2DRenderTarget != null)
                {
                    PrepareAndCallRender(render, matrix);
                    _dispatcher?.InvokeAsync(() => _d3DSurface?.InvalidateD3DImage(), DispatcherPriority.Send);
                }
            }
        }

        private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_d3DSurface.IsFrontBufferAvailable)
            {
                StartRendering();
            }
            else
            {
                _allowRender = false;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (!_allowResize)
            {
                return;
            }

            lock (_chartLock)
            {
                CreateAndBindTargets();
            }

            _allowResize = false;

            _resizeTimer.Start();

            base.OnRenderSizeChanged(sizeInfo);
        }

        private void ResizeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _allowResize = true;
        }

        // - private methods -------------------------------------------------------------

        private void StartRendering()
        {
            CreateAndBindTargets();
            _allowRender = true;
        }

        private void StopRendering()
        {
            _allowRender = false;
        }

        private void StartD3D()
        {
            _device = new SharpDX.Direct3D11.Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);

            _d3DSurface = new Dx11ImageSource();

            _d3DSurface.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

            CreateAndBindTargets();

            base.Source = _d3DSurface;
        }

        private void EndD3D()
        {
            _d3DSurface.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
            Source = null;

            Disposer.SafeDispose(ref _d2DRenderTarget);
            Disposer.SafeDispose(ref _d2DFactory);
            Disposer.SafeDispose(ref _d3DSurface);
            Disposer.SafeDispose(ref _renderTarget);
            Disposer.SafeDispose(ref _device);
        }

        private void CreateAndBindTargets()
        {
            if (_d3DSurface == null)
            {
                return;
            }

            _d3DSurface.SetRenderTarget(null);

            Disposer.SafeDispose(ref _d2DRenderTarget);
            Disposer.SafeDispose(ref _d2DFactory);
            Disposer.SafeDispose(ref _renderTarget);

            var width = Math.Max((int)Math.Floor(ActualWidth), 100);
            var height = Math.Max((int)Math.Floor(ActualHeight), 100);

            _renderDesc.Width = width;
            _renderDesc.Height = height;

            _renderTarget = new Texture2D(_device, _renderDesc);

            var surface = _renderTarget.QueryInterface<Surface>();

            _d2DFactory = new SharpDX.Direct2D1.Factory();

            _d2DRenderTarget = new RenderTarget(_d2DFactory, surface, _renderProperties)
            {
                AntialiasMode = AntialiasMode.PerPrimitive
            };

            ResCache.RenderTarget = _d2DRenderTarget;

            _d3DSurface.SetRenderTarget(_renderTarget);

            _device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height);
        }

    }
}
