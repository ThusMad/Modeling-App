using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using Modeler.Core;
using Modeler.Core.Models;
using Modeler.Core.Utilities;
using Modeler.Renderer.Native;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace Modeler.Renderer
{
    public class SceneControl : SceneSwapChain
    {
        private bool _initialized;
        private object _sync = new object();
        // Transformation matrix for chart movement
        private readonly RawMatrix3x2 _transformMatrix = new RawMatrix3x2(1f, 0, 0, 1, 0, 0);


        #region Constructors

        public SceneControl()
        {
            Loaded += Line_Loaded;
            Messenger.Default.Register<DrawModel>
            (
                this,
                (model) => DrawModel(model)
            );
            Debug.WriteLine("\n [Debug] Renderer initialization done! \n");
        }

        private void Line_Loaded(object sender, RoutedEventArgs e)
        {
            _initialized = true;

            Debug.WriteLine("\n [Debug] Renderer loaded!");
        }

        #endregion

        public void DrawModel(DrawModel model)
        {
            if (_initialized)
            {
                lock (_sync)
                {
                    OnDrawScene((target) =>
                    {
                        target.Clear(new RawColor4(1f, 1f, 1f, 1f));

                        foreach (var shape in model.Grid)
                        {
                            target.DrawShape(shape, (int)shape.Thickness, new SolidColorBrush(target, shape.Color));
                        }
                        foreach (var shape in model.Shapes)
                        {
                            target.DrawShape(shape, (int)shape.Thickness, new SolidColorBrush(target, shape.Color));
                        }
                    }, _transformMatrix);
                }
            }
        }


    }
}
