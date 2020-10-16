#define EnableMeasure

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using SceneController.Native;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace SceneController
{
    public class SceneControl : SceneSwapChain
    {
        public static DependencyProperty SaveCommandProperty
            = DependencyProperty.Register(
                "Render",
                typeof(ICommand),
                typeof(SceneControl));

        // Basic params for points normalization

        private bool _allowAddapt;
        private bool _initialized;

        // Transformations and scales
        private int _width = 1;
        private int _height = 1;

        private float _currentMoveTransformX = 0f;
        private float _savedMoveTransformX = 0;

        private double _offsetY = 0;

        private double _transformX = 0;
        private double _transformY = 0;

        private double _scaleX = 1;
        private double _scaleY = 10;

        private double _target;
        private double _error;

        // Transformation matrix for chart movement
        private RawMatrix3x2 _transformMatrix = new RawMatrix3x2(1f, 0, 0, 1, 0, 0);


        #region Constructors

        public SceneControl()
        {
            Loaded += Line_Loaded;
            SizeChanged += Line_SizeChanged;

            // Basic settings
            SnapsToDevicePixels = true;
            Cursor = Cursors.Pen;

            Debug.WriteLine("\n SceneControl initialization done! \n");
        }

        private void Line_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _width = (int)Math.Floor(e.NewSize.Width);
            _height = (int)Math.Floor(e.NewSize.Height);
        }

        private void Line_Loaded(object sender, RoutedEventArgs e)
        {
            List<RawVector2> normalizedPoints = new List<RawVector2>();
            List<RawVector2> newNormalizedPoints = new List<RawVector2>();

            //ChartColors.ColorChanged += UpdateColor;

            ////Initializing brushes for current target 
            //resCache.Add("LineBrush", t => new SolidColorBrush(t, ChartColors.GetColor("ChartLineColor")));

            //resCache.Add("NormalizedPoints", t => normalizedPoints);

            //resCache.Add("RenderData", t => _cacheData);

            _initialized = true;
            Debug.WriteLine("\n --Chart loaded!");
        }

        #endregion

        public void Draw(Action<RenderTarget> action)
        {
            if (_initialized)
            {
                OnDrawScene(action, _transformMatrix);
            }
        }

    }
}
