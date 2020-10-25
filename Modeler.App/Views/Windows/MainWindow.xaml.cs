using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Modeler.Core;
using Modeler.Core.Shapes;
using Modeler.Core.Utilities;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Triangle = Modeler.Core.Shapes.Triangle;

namespace Modeler.App.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ShapeBase> Shapes;
        private List<ShapeBase> Grid;

        private IntPoint _zero = new IntPoint(50, 50);
        public MainWindow()
        {
            InitializeComponent();
            Grid = new List<ShapeBase>();
            Shapes = new List<ShapeBase>();

            //SceneControl.SizeChanged += SizeRedraw;
        }

        //private void SizeRedraw(object sender, SizeChangedEventArgs e)
        //{
        //    CreateShapes();
        //    GenerateGrid();

        //    SceneControl.Draw(target =>
        //    {
        //        DrawGrid(target);
        //        Draw(target);
        //    });
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    CreateShapes();
        //    GenerateGrid();

        //    SceneControl.Draw(target =>
        //    {
        //        DrawGrid(target);
        //        Draw(target);
        //    });

        //}

        //private void Draw(RenderTarget target)
        //{
        //    foreach (var shapeBase in Shapes)
        //    {
        //       target.DrawShape(shapeBase, 2);
        //    }
        //}

        //private void DrawGrid(RenderTarget target)
        //{
        //    target.Clear(new RawColor4(1f, 1f, 1f, 1f));

        //    foreach (var shapeBase in Grid)
        //    {
        //        if (shapeBase is Triangle triangle)
        //        {
        //            target.DrawShape(triangle, 2, new SolidColorBrush(target, new RawColor4(0.2f, 0.2f, 0.2f, 1)));
        //        }
        //        else if (shapeBase is Text text)
        //        {
        //            using (var factoryDWrite = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared))
        //            {
        //                var textFormat = new TextFormat(factoryDWrite, "Tahoma", SharpDX.DirectWrite.FontWeight.DemiBold, SharpDX.DirectWrite.FontStyle.Normal, SharpDX.DirectWrite.FontStretch.Normal, 20);
        //                target.DrawText(text.Message, textFormat, new RawRectangleF(text.Data[0].X, text.Data[0].Y, text.Data[0].X + 20, text.Data[0].Y + 20), new SolidColorBrush(target, new RawColor4(0.2f, 0.2f, 0.2f, 1)));
        //            }

        //        }
        //        else
        //        {
        //            target.DrawShape(shapeBase, 1, new SolidColorBrush(target, new RawColor4(0.15f, 0.15f, 0.15f, 1)));
        //        }

        //    }
        //}

        //private void GenerateGrid()
        //{
        //    Grid.Clear();

        //    var height = SceneControl.ActualHeight;
        //    var width = SceneControl.ActualWidth;

        //    var gridSize = new IntPoint((int)(width * 0.85), (int)(height * 0.85));

        //    // draw Y axis
        //    Grid.Add(new Line(_zero.X + 1, _zero.Y, _zero.X + 1, gridSize.Y));
        //    Grid.Add(new Line(_zero.X, _zero.Y + 1, _zero.X, gridSize.Y + 1));
        //    Grid.Add(new Triangle(_zero.X, gridSize.Y + 22, _zero.X - 10, gridSize.Y, _zero.X + 10, gridSize.Y));
        //    Grid.Add(new Text(_zero.X + 22, gridSize.Y + 5, "Y"));

        //    // draw X axis
        //    Grid.Add(new Line(_zero.X, _zero.Y, gridSize.X, _zero.Y));
        //    Grid.Add(new Triangle(gridSize.X + 20, _zero.Y, gridSize.X, _zero.Y + 10, gridSize.X, _zero.Y - 10));
        //    Grid.Add(new Text(gridSize.X - 15, _zero.Y + 10, "X"));

        //    for (var i = _zero.X; i <= gridSize.X; i+= 20)
        //    {
        //        Grid.Add(new Line(i, _zero.Y, i , gridSize.Y));
        //    }

        //    for (var i = _zero.Y; i <= gridSize.Y; i += 20)
        //    {
        //        Grid.Add(new Line(_zero.X, i, gridSize.X, i));
        //    }

        //}

        //private void CreateShapes()
        //{
        //    Shapes.Clear();

        //    var height = SceneControl.ActualHeight;
        //    var width = SceneControl.ActualWidth;

        //    var gridSize = new IntPoint((int)(width * 0.85), (int)(height * 0.85));

        //    var centerCircle = new Circle((gridSize.X / 2), (gridSize.Y / 2), 50);
        //    var topCircle = new Circle(centerCircle.CenterX, centerCircle.CenterY - centerCircle.Radius * 2 - 15, centerCircle.Radius);
        //    var bottomCircle = new Circle(centerCircle.CenterX, centerCircle.CenterY + centerCircle.Radius * 2 + 15, centerCircle.Radius);

        //    dR1.Text = centerCircle.Radius.ToString();
        //    dR2.Text = topCircle.Radius.ToString();
        //    dR3.Text = bottomCircle.Radius.ToString();

        //    var rightTriangle = new Triangle(
        //        centerCircle.CenterX + centerCircle.Radius, centerCircle.CenterY,
        //        centerCircle.CenterX + (int)(centerCircle.Radius * 2.5), centerCircle.CenterY - centerCircle.Radius,
        //        centerCircle.CenterX + (int)(centerCircle.Radius * 2.5), centerCircle.CenterY + centerCircle.Radius);

        //    var leftTriangle = new Triangle(
        //        centerCircle.CenterX - centerCircle.Radius, centerCircle.CenterY,
        //        centerCircle.CenterX - (int)(centerCircle.Radius * 2.5), centerCircle.CenterY + centerCircle.Radius,
        //        centerCircle.CenterX - (int)(centerCircle.Radius * 2.5), centerCircle.CenterY - centerCircle.Radius);

        //    var rightArc = new Arc(
        //        centerCircle.CenterX + (int)(centerCircle.Radius * 2.5), centerCircle.CenterY - centerCircle.Radius,
        //        centerCircle.CenterX + (int)(centerCircle.Radius * 2.5), centerCircle.CenterY + centerCircle.Radius,
        //        180, 40);

        //    var leftArc = new Arc(
        //        centerCircle.CenterX - (int)(centerCircle.Radius * 2.5), centerCircle.CenterY + centerCircle.Radius,
        //        centerCircle.CenterX - (int)(centerCircle.Radius * 2.5), centerCircle.CenterY - centerCircle.Radius,
        //        180, 40);

        //    var topArc = new Arc(
        //        centerCircle.CenterX + centerCircle.Radius * 2, topCircle.CenterY - topCircle.Radius,
        //        centerCircle.CenterX - centerCircle.Radius * 2, topCircle.CenterY - topCircle.Radius,
        //        90, 40);

        //    var bottomArc = new Arc(
        //        centerCircle.CenterX - centerCircle.Radius * 2, bottomCircle.CenterY + bottomCircle.Radius,
        //        centerCircle.CenterX + centerCircle.Radius * 2, bottomCircle.CenterY + bottomCircle.Radius,
        //        90, 40);

        //    var topLeftLine = new Line(centerCircle.CenterX + centerCircle.Radius * 4, centerCircle.CenterY,
        //        centerCircle.CenterX + centerCircle.Radius * 2, topCircle.CenterY - topCircle.Radius);

        //    var bottomLeftLine = new Line(centerCircle.CenterX + centerCircle.Radius * 4, centerCircle.CenterY,
        //        centerCircle.CenterX + centerCircle.Radius * 2, bottomCircle.CenterY + bottomCircle.Radius);

        //    var topRightLine = new Line(centerCircle.CenterX - centerCircle.Radius * 4, centerCircle.CenterY,
        //        centerCircle.CenterX - centerCircle.Radius * 2, topCircle.CenterY - topCircle.Radius);

        //    var bottomRightLine = new Line(centerCircle.CenterX - centerCircle.Radius * 4, centerCircle.CenterY,
        //        centerCircle.CenterX - centerCircle.Radius * 2, bottomCircle.CenterY + bottomCircle.Radius);

        //    topArc.Rotate(180);
        //    bottomArc.Rotate(180);

        //    Shapes.Add(centerCircle);
        //    Shapes.Add(topCircle); 
        //    Shapes.Add(bottomCircle); 
        //    Shapes.Add(rightTriangle);
        //    Shapes.Add(leftTriangle);
        //    Shapes.Add(rightArc);
        //    Shapes.Add(leftArc);
        //    Shapes.Add(topArc);
        //    Shapes.Add(bottomArc);
        //    Shapes.Add(topLeftLine);
        //    Shapes.Add(bottomLeftLine);
        //    Shapes.Add(topRightLine);
        //    Shapes.Add(bottomRightLine);
        //}

        //private void ApplyProjective(object sender, RoutedEventArgs e)
        //{
        //    var ro = new RawVector3(float.Parse(prR0X.Text), float.Parse(prR0Y.Text), float.Parse(prR0W.Text));
        //    var rx = new RawVector3(float.Parse(prRXX.Text), float.Parse(prRXY.Text), float.Parse(prRXW.Text));
        //    var ry = new RawVector3(float.Parse(prRYX.Text), float.Parse(prRYY.Text), float.Parse(prRYW.Text));

        //    foreach (var shapeBase in Shapes)
        //    {
        //        shapeBase.ProjectiveTransformation(ro, rx, ry);
        //    }
        //    foreach (var shapeBase in Grid)
        //    {
        //        shapeBase.ProjectiveTransformation(ro, rx, ry);
        //    }

        //    SceneControl.Draw(target =>
        //    {
        //        DrawGrid(target);
        //        Draw(target);
        //    });
        //}

        //private void ApplyAffine(object sender, RoutedEventArgs e)
        //{
        //    var ro = new RawVector2(float.Parse(afR0X.Text) / 20, float.Parse(afR0Y.Text) / 20);
        //    var rx = new RawVector2(float.Parse(afRXX.Text) / 20,  float.Parse(afRXY.Text) / 20);
        //    var ry = new RawVector2(float.Parse(afRYX.Text) / 20, float.Parse(afRYY.Text) / 20);

        //    foreach (var shapeBase in Shapes)
        //    {
        //        shapeBase.AffineTransformation(ro, rx, ry);
        //    }
        //    foreach (var shapeBase in Grid)
        //    {
        //        shapeBase.AffineTransformation(ro, rx, ry);
        //    }

        //    SceneControl.Draw(target =>
        //    {
        //        DrawGrid(target);
        //        Draw(target);
        //    });
        //}

        //private void ApplySizeChanged(object sender, RoutedEventArgs e)
        //{
        //    var counter = 1;
        //    for (int i = 0; i < Shapes.Count; i++)
        //    {
        //        if (!(Shapes[i] is Circle circle)) continue;

        //        switch (counter)
        //        {
        //            case 1:
        //                circle.Radius = int.Parse(dR1.Text);
        //                break;
        //            case 2:
        //                circle.Radius = int.Parse(dR2.Text);
        //                break;
        //            case 3:
        //                circle.Radius = int.Parse(dR3.Text);
        //                break;
        //        }

        //        counter++;
        //    }

        //    SceneControl.Draw(target =>
        //    {
        //        DrawGrid(target);
        //        Draw(target);
        //    });
        //}

        //private void ApplyOffsetChanged(object sender, RoutedEventArgs e)
        //{
        //    foreach (var shapeBase in Shapes)
        //    {
        //        shapeBase.Transform(int.Parse(dX.Text), int.Parse(dY.Text));
        //    }

        //    SceneControl.Draw(target =>
        //    {
        //        DrawGrid(target);
        //        Draw(target);
        //    });
        //}

        //private void ApplyRotationChanged(object sender, RoutedEventArgs e)
        //{

        //    foreach (var shapeBase in Shapes)
        //    {
        //        shapeBase.Rotate(int.Parse(Angle.Text), _zero.X + int.Parse(rotX.Text), _zero.Y + int.Parse(rotY.Text));
        //    }

        //    Shapes.Add(new Circle(_zero.X + int.Parse(rotX.Text), _zero.Y + int.Parse(rotY.Text), 20));

        //    SceneControl.Draw(target =>
        //    {
        //        DrawGrid(target);
        //        Draw(target);
        //    });
        //}
    }
}
