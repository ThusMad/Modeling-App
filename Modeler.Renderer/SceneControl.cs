using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Modeler.Core;
using Modeler.Core.Utilities;
using Modeler.Renderer.Native;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace Modeler.Renderer
{
    public class SceneControl : SceneSwapChain
    {
        private bool _initialized;

        // Transformation matrix for chart movement
        private readonly RawMatrix3x2 _transformMatrix = new RawMatrix3x2(1f, 0, 0, 1, 0, 0);


        #region Constructors

        public SceneControl()
        {
            Loaded += Line_Loaded;
            Debug.WriteLine("\n [Debug] Renderer initialization done! \n");
        }

        public List<ShapeBase> Grid
        {
            get => (List<ShapeBase>)GetValue(GridProperty);
            set => SetValue(GridProperty, value);
        }

        public List<ShapeBase> Shapes
        {
            get => (List<ShapeBase>)GetValue(ShapesProperty);
            set => SetValue(ShapesProperty, value);
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

        public void GridChanged(DependencyPropertyChangedEventArgs e)
        {
            Draw(target =>
            {
                target.Clear(new RawColor4(1f, 1f, 1f, 1f));
                foreach (var shapeBase in (List<ShapeBase>)e.NewValue)
                {
                    target.DrawShape(shapeBase, (int)shapeBase.Thickness, new SolidColorBrush(target, shapeBase.Color));
                }

                if (Shapes == null)
                {
                    return;
                }

                foreach (var shapeBase in Shapes)
                {
                    target.DrawShape(shapeBase, (int)shapeBase.Thickness, new SolidColorBrush(target, shapeBase.Color));
                }
            });
        }

        public void ShapesChanged(DependencyPropertyChangedEventArgs e)
        {
            Draw(target =>
            {
                target.Clear(new RawColor4(1f, 1f, 1f, 1f));
                if (Grid != null)
                {
                    foreach (var shapeBase in Grid)
                    {
                        target.DrawShape(shapeBase, (int)shapeBase.Thickness, new SolidColorBrush(target, shapeBase.Color));
                    }
                }
                foreach (var shapeBase in (List<ShapeBase>)e.NewValue)
                {
                    target.DrawShape(shapeBase, (int)shapeBase.Thickness, new SolidColorBrush(target, shapeBase.Color));
                }

            });
        }

        #region Property

        public static readonly DependencyProperty GridProperty = DependencyProperty.Register("Grid",
            typeof(List<ShapeBase>), typeof(SceneControl), new PropertyMetadata(null, GridPropertyChanged));
        public static readonly DependencyProperty ShapesProperty = DependencyProperty.Register("Shapes",
            typeof(List<ShapeBase>), typeof(SceneControl), new PropertyMetadata(null, ShapesPropertyChanged));

        private static void GridPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SceneControl)d).GridChanged(e);
        }

        private static void ShapesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SceneControl)d).ShapesChanged(e);
        }

        #endregion


    }
}
