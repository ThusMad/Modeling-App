using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Modeler.Renderer.Native;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace Modeler.Renderer
{
    public class SceneControl : SceneSwapChain
    {
        public static DependencyProperty SaveCommandProperty
            = DependencyProperty.Register(
                "Render",
                typeof(ICommand),
                typeof(SceneControl));

        private bool _initialized;

        // Transformation matrix for chart movement
        private readonly RawMatrix3x2 _transformMatrix = new RawMatrix3x2(1f, 0, 0, 1, 0, 0);


        #region Constructors

        public SceneControl()
        {
            Loaded += Line_Loaded;
            Debug.WriteLine("\n [Debug] Renderer initialization done! \n");
        }

        private void Line_Loaded(object sender, RoutedEventArgs e)
        {
            _initialized = true;
            Debug.WriteLine("\n [Debug] Renderer loaded!");
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
