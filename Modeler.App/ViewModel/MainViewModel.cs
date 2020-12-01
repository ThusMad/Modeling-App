using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Modeler.App.Models;
using Modeler.App.Views.Dialogs;
using Modeler.Core;
using Modeler.Core.Enums;
using Modeler.Core.Models;
using Modeler.Core.Shapes;
using Modeler.Core.Utilities;
using Modeler.Renderer;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
using SharpDX.Mathematics.Interop;
using Triangle = Modeler.Core.Shapes.Triangle;

namespace Modeler.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IInputElement _source;
        private ShapeBase _tempTangent;
        private ShapeBase _tempNormal;

        private int _xShift = 0;
        private int _yShift = 0;

        private int _rotAngle = 0;
        private IntPoint _rotPoint = new IntPoint(0, 0);

        private ShapeBase _shapeOver;

        private bool _isModelChanged = false;
        private bool _isDialogOpened = false;
        private ToolType _selectedTool = ToolType.None;

        private bool _isHorizontalHeldActive = false;
        private bool _isVerticalHeldActive = false;

        private IntPoint _sceneSize;

        private ShapeBase _tempShape;
        private List<Point> _clickedPositions;
        private DrawModel _drawModel;
        public MainViewModel()
        {
            _clickedPositions = new List<Point>();
            _drawModel = new DrawModel(new List<ShapeBase>(), new List<ShapeBase>());
            _sceneSize = new IntPoint(0, 0);

            DrawDebugInfoCommand = new RelayCommand(DrawDebugInfo);
            FileCreateCommand = new RelayCommand(CreateFile, CanOpenDialog);
            FileOpenCommand = new RelayCommand(OpenFile, CanOpenDialog);
            RefreshCommand = new RelayCommand(RefreshScene);
            SizeChangedCommand = new RelayCommand<SizeChangedEventArgs>(OnSizeChanged);
            SelectToolCommand = new RelayCommand<string>(SelectTool);
            HorizontalHeldCommand = new RelayCommand(HorizontalHeld);
            VerticalHeldCommand = new RelayCommand(VerticalHeld);
            SceneClickCommand = new RelayCommand<MouseButtonEventArgs>(SceneClick);
            SceneClickCtrlCommand = new RelayCommand<MouseButtonEventArgs>(ControlClickCommand);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMove);
            EscapeCommand = new RelayCommand(EscapeAction);
            ShiftCommand = new RelayCommand(ShiftShapes);
            RotateCommand = new RelayCommand(RotateShapes);
            AffineCommand = new RelayCommand(AffineTransformShape);
            HomographyCommand = new RelayCommand(HomographyTransformShaoe);
            RoseToolCommand = new RelayCommand(RoseCreationAction);
            Tabs = new ObservableCollection<TabModel>();
        }

      

        private void RoseCreationAction()
        {
            var roseDialog = new RoseCreationDialog(_source);
            var tempRose = new Rose(0, 0, 0, 0, 0,0, new RawColor4(1f, 0f, 0f, 1f));
            _drawModel.Shapes.Add(tempRose);

            roseDialog.ModelUpdate += model =>
            {
                _drawModel.Shapes.Remove(tempRose);
                tempRose = new Rose(model, new RawColor4(1f, 0f, 0f, 1f));
                _drawModel.Shapes.Add(tempRose);

                Messenger.Default.Send<DrawModel>(_drawModel);
            };

            if (roseDialog.ShowDialog() == true)
            {
                _drawModel.Shapes.Remove(tempRose);
                _drawModel.Shapes.Add(new Rose(roseDialog.RoseCreationModel, new RawColor4(0f, 0f, 0f, 1f)));
                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        private void HomographyTransformShaoe()
        {
            var inpDialog = new HomographyTransformationDialog();

            inpDialog.HomographyTransformationDelegate += model =>
            {
                _drawModel.IsHomographyActive = true;
                _drawModel.HomographyTransformation = model;
                Messenger.Default.Send<DrawModel>(_drawModel);
            };

            if (inpDialog.ShowDialog() == true)
            {
                _drawModel.HomographyTransformation = inpDialog.Transformation;
                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        private void AffineTransformShape()
        {
            var inpDialog = new AffineTransformationDialog();

            inpDialog.AffineTransformationDelegate += model =>
            {
                _drawModel.IsAffineActive = true;
                _drawModel.AffineTransformation = model;
                Messenger.Default.Send<DrawModel>(_drawModel);
            };

            if (inpDialog.ShowDialog() == true)
            {
                _drawModel.AffineTransformation = inpDialog.Transformation;
                Messenger.Default.Send<DrawModel>(_drawModel);
            }

        }

        private void RotateShapes()
        {
            var inpDialog = new RotateDialog(_source);

            inpDialog.RotationDelegate += i =>
            {
                _drawModel.RotationAngle = i;
                Messenger.Default.Send<DrawModel>(_drawModel);
            };

            inpDialog.PointDelegate += (x, y) =>
            {
                _drawModel.RotationPoint = new IntPoint(x, y);
                Messenger.Default.Send<DrawModel>(_drawModel);
            };

            if (inpDialog.ShowDialog() == true)
            {
                _drawModel.RotationAngle = inpDialog.Angle;
                _drawModel.RotationPoint = new IntPoint(inpDialog.X, inpDialog.Y);

                _rotAngle = inpDialog.Angle;
                _rotPoint = new IntPoint(inpDialog.X, inpDialog.Y);

                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        private void ShiftShapes()
        {
            var inpDialog = new ShiftDialog(_xShift, _yShift);

            inpDialog.XChangedDelegate += i =>
            {
                _drawModel.XShift = i;
                Messenger.Default.Send<DrawModel>(_drawModel);
            };

            inpDialog.YChangedDelegate += i =>
            {
                _drawModel.YShift = i;
                Messenger.Default.Send<DrawModel>(_drawModel);
            };

            if (inpDialog.ShowDialog() == true)
            {
                _drawModel.XShift = inpDialog.XSHift;
                _drawModel.YShift = inpDialog.YSHift;

                _xShift = inpDialog.XSHift;
                _yShift = inpDialog.YSHift;

                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        private void OnSizeChanged(SizeChangedEventArgs obj)
        {
            _drawModel.Grid = GridGenerator.GenerateGrid((int)obj.NewSize.Width - 50, (int)obj.NewSize.Height - 150, 20, new IntPoint(60, 20));
            _sceneSize = new IntPoint((int)obj.NewSize.Width - 50, (int)obj.NewSize.Height - 150);
            Messenger.Default.Send<DrawModel>(_drawModel);
        }

        public RelayCommand RoseToolCommand { get; set; }
        public RelayCommand DrawDebugInfoCommand { get; private set; }
        public RelayCommand ShiftCommand { get; private set; }
        public RelayCommand FileCreateCommand { get; private set; }
        public RelayCommand FileOpenCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand EscapeCommand { get; private set; }
        public RelayCommand AffineCommand { get; private set; }
        public RelayCommand<string> SelectToolCommand { get; private set; }
        public RelayCommand HorizontalHeldCommand { get; private set; }
        public RelayCommand VerticalHeldCommand { get; private set; }
        public RelayCommand<SizeChangedEventArgs> SizeChangedCommand { get; private set; }
        public RelayCommand<MouseButtonEventArgs> SceneClickCommand { get; private set; }
        public RelayCommand<MouseButtonEventArgs> SceneClickCtrlCommand { get; private set; }
        public RelayCommand<MouseEventArgs> MouseMoveCommand { get; private set; }
        public RelayCommand RotateCommand { get; private set; }
        public RelayCommand HomographyCommand { get; private set; }
        public Cursor Cursor { get; set; }

        public ObservableCollection<TabModel> Tabs { get; set; }

        private void DrawDebugInfo()
        {
            Messenger.Default.Send<DebugDrawingModel>(new DebugDrawingModel((taget =>
            {
                foreach (var drawModelShape in _drawModel.Shapes)
                {
                    taget.DrawRectangle(new RawRectangleF(drawModelShape.OuterBox.Left, drawModelShape.OuterBox.Top, drawModelShape.OuterBox.Right, drawModelShape.OuterBox.Bottom),
                        new SolidColorBrush(taget, new RawColor4(0f, 0f, 0.6f, 1f)), 2, new StrokeStyle(taget.Factory, new StrokeStyleProperties()
                        {
                            DashStyle = DashStyle.Dash
                        }));
                }
                
            })));
        }

        private void MouseMove(MouseEventArgs obj)
        {
            if (_source == null)
            {
                _source = obj.Source as IInputElement;
            }
            var currentPoint = obj.GetPosition(obj.Source as IInputElement);

            if (_tempShape == null)
            {
                CollisionBehaviour(currentPoint);
                return;
            }

            if (_selectedTool == ToolType.Line)
            {
                LineDrawBehaviour(currentPoint);
            }

            if (_selectedTool == ToolType.Rectangle)
            {
                RectangleDrawBehaviour(currentPoint);
            }

            if (_selectedTool == ToolType.Triangle)
            {
                TriangleDrawBehaviour(currentPoint);
            }

            if (_selectedTool == ToolType.Circle)
            {
                CircleDrawBehaviour(currentPoint);
            }

            if (_selectedTool == ToolType.Arc)
            {
                ArcDrawBehaviour(currentPoint);
            }
        }

        #region ShortcutEvents

        private void EscapeAction()
        {
            if (_tempShape != null)
            {
                _drawModel.Shapes.Remove(_tempShape);
                _tempShape = null;
                _selectedTool = ToolType.None;
                Cursor = Cursors.Cross;
                _clickedPositions.Clear();
                Messenger.Default.Send(_drawModel);
            }
        }

        private void HorizontalHeld()
        {
            _isHorizontalHeldActive = !_isHorizontalHeldActive;
        }

        private void VerticalHeld()
        {
            _isVerticalHeldActive = !_isVerticalHeldActive;
        }

        private void RefreshScene()
        {
            Cursor = Cursors.AppStarting;
            Messenger.Default.Send<DrawModel>(_drawModel);
        }

        #endregion

        #region MouseMoveBehaviours

        private void CollisionBehaviour(Point currentPoint)
        {
            var shape = DetectMouseCollision(_drawModel.Shapes, new IntPoint((int)currentPoint.X, (int)currentPoint.Y));
            


            if (shape != null)
            {
                if (shape.Item1 == null)
                {
                    return;
                }


                if (_tempTangent != null)
                {
                    _drawModel.Shapes.Remove(_tempTangent);
                    _tempTangent = null;
                }

                if (_tempNormal != null)
                {
                    _drawModel.Shapes.Remove(_tempNormal);
                    _tempNormal = null;
                }

                _shapeOver = shape.Item1;

                shape.Item1.Color = new RawColor4(1f, 0f, 0f, 1f);
                shape.Item1.IsMouseOver = true;
                Cursor = Cursors.Hand;
                RaisePropertyChanged(nameof(Cursor));

                var tangent = shape.Item1.BuildTangen(shape.Item2, 300);
                var normal = shape.Item1.BuildNormal(shape.Item2, 300);

                _tempTangent = new Line(0, 0, 0, 0, new RawColor4(0, 0f, 0.3f, 1f))
                {
                    Data = tangent.ToList()
                };

                _tempNormal = new Line(0, 0, 0, 0, new RawColor4(0, 0f, 0.3f, 1f))
                {
                    Data = normal.ToList()
                };

                _drawModel.Shapes.Add(_tempTangent);
                _drawModel.Shapes.Add(_tempNormal);

                Messenger.Default.Send<DrawModel>(_drawModel);
            }
            else
            {
                Cursor = _selectedTool == ToolType.None ? Cursors.Cross : Cursors.Pen;
                _shapeOver = null;
                RaisePropertyChanged(nameof(Cursor));
            }

            if (_isModelChanged)
            {
                Messenger.Default.Send<DrawModel>(_drawModel);
                _isModelChanged = false;
            }
        }

        private void LineDrawBehaviour(Point currentPoint)
        {
            if (!_isHorizontalHeldActive)
            {
                if (!_isVerticalHeldActive)
                {
                    _tempShape.Data[1] = new RawVector2((int)currentPoint.X, (int)currentPoint.Y);
                }
                else
                {
                    _tempShape.Data[1] = new RawVector2((int)currentPoint.X, _tempShape.Data[0].Y);
                }
            }
            else
            {
                _tempShape.Data[1] = new RawVector2(_tempShape.Data[0].X, (int)currentPoint.Y);
            }

            Messenger.Default.Send<DrawModel>(_drawModel);
        }

        private void RectangleDrawBehaviour(Point currentPoint)
        {
            (_tempShape as Rectangle).Width = (int)currentPoint.X - (int)_clickedPositions[0].X;
            (_tempShape as Rectangle).Height = (int)currentPoint.Y - (int)_clickedPositions[0].Y;

            Messenger.Default.Send<DrawModel>(_drawModel);
        }

        private void CircleDrawBehaviour(Point currentPoint)
        {
            (_tempShape as Circle).Radius = (int)Utility.GetDistance((float)_clickedPositions[0].X, (float)_clickedPositions[0].Y, (float)currentPoint.X,
                (float)currentPoint.Y);

            Messenger.Default.Send<DrawModel>(_drawModel);
        }

        private void TriangleDrawBehaviour(Point currentPoint)
        {
            if (_clickedPositions.Count == 1)
            {
                _tempShape.Data[1] = new RawVector2((int)currentPoint.X, (int)currentPoint.Y);
            }

            if (_clickedPositions.Count == 2)
            {
                _tempShape.Data[2] = new RawVector2((int)currentPoint.X, (int)currentPoint.Y);
            }

            Messenger.Default.Send<DrawModel>(_drawModel);
        }

        private void ArcDrawBehaviour(Point currentPoint)
        {
            _drawModel.Shapes.Remove(_tempShape);
            _tempShape = new Arc((int) _clickedPositions[0].X, (int) _clickedPositions[0].Y, (int) currentPoint.X,
                (int) currentPoint.Y, 180, 90, new RawColor4(1f, 0f, 0f, 0.4f))
            {
                Color = new RawColor4(1f, 0f, 0f, 0.4f)
            };
            _drawModel.Shapes.Add(_tempShape);

            Messenger.Default.Send<DrawModel>(_drawModel);
        }

        #endregion


        #region MouseClickBehaviours

        private void LineClickBehaviour(Point currentPoint)
        {
            if (_clickedPositions.Count == 0)
            {
                _clickedPositions.Add(currentPoint);

                _tempShape = new Line((int)currentPoint.X, (int)currentPoint.Y, (int)currentPoint.X, (int)currentPoint.Y, new RawColor4(1f, 0f, 0f, 0.4f));
                _drawModel.Shapes.Add(_tempShape);
                Messenger.Default.Send<DrawModel>(_drawModel);
                return;
            }

            if (_clickedPositions.Count == 1)
            {
                if (!_isHorizontalHeldActive)
                {
                    if (!_isVerticalHeldActive)
                    {
                        _tempShape.Data[1] = new RawVector2((int)currentPoint.X, (int)currentPoint.Y);
                    }
                    else
                    {
                        _tempShape.Data[1] = new RawVector2((int)currentPoint.X, _tempShape.Data[0].Y);
                    }
                }
                else
                {
                    _tempShape.Data[1] = new RawVector2(_tempShape.Data[0].X, (int)currentPoint.Y);
                }
                _tempShape.Color = new RawColor4(0f, 0f, 0f, 1f);
                _tempShape.CalculateOuterBox();
                _tempShape = null;
                _clickedPositions.Clear();
                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        private void TriangleClickBehaviour(Point currentPoint)
        {
            if (_clickedPositions.Count == 0)
            {
                _clickedPositions.Add(currentPoint);

                _tempShape = new Triangle((int)currentPoint.X, (int)currentPoint.Y, (int)currentPoint.X, (int)currentPoint.Y, (int)currentPoint.X, (int)currentPoint.Y, new RawColor4(1f, 0f, 0f, 0.4f));
                _drawModel.Shapes.Add(_tempShape);
                Messenger.Default.Send<DrawModel>(_drawModel);
                return;
            }

            if (_clickedPositions.Count == 1)
            {
                if (!_isHorizontalHeldActive)
                {
                    if (!_isVerticalHeldActive)
                    {
                        _tempShape.Data[1] = new RawVector2((int)currentPoint.X, (int)currentPoint.Y);
                    }
                    else
                    {
                        _tempShape.Data[1] = new RawVector2((int)currentPoint.X, _tempShape.Data[0].Y);
                    }
                }
                else
                {
                    _tempShape.Data[1] = new RawVector2(_tempShape.Data[0].X, (int)currentPoint.Y);
                }

                _clickedPositions.Add(currentPoint);

                return;   
            }

            if (_clickedPositions.Count == 2)
            {
                if (!_isHorizontalHeldActive)
                {
                    if (!_isVerticalHeldActive)
                    {
                        _tempShape.Data[2] = new RawVector2((int)currentPoint.X, (int)currentPoint.Y);
                    }
                    else
                    {
                        _tempShape.Data[2] = new RawVector2((int)currentPoint.X, _tempShape.Data[0].Y);
                    }
                }
                else
                {
                    _tempShape.Data[2] = new RawVector2(_tempShape.Data[0].X, (int)currentPoint.Y);
                }
                _tempShape.Color = new RawColor4(0f, 0f, 0f, 1f);
                _tempShape.CalculateOuterBox();
                _tempShape = null;
                _clickedPositions.Clear();
                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        private void RectangleClickBehaviour(Point currentPoint)
        {
            if (_clickedPositions.Count == 0)
            {
                _clickedPositions.Add(currentPoint);

                _tempShape = new Rectangle((int)currentPoint.X, (int)currentPoint.Y, 0, 0, new RawColor4(1f, 0f, 0f, 0.4f));
                _drawModel.Shapes.Add(_tempShape);
                Messenger.Default.Send<DrawModel>(_drawModel);
                return;
            }

            if (_clickedPositions.Count == 1)
            {
                (_tempShape as Rectangle).Width = (int)currentPoint.X - (int)_clickedPositions[0].X;
                (_tempShape as Rectangle).Height = (int)currentPoint.Y - (int)_clickedPositions[0].Y;
                _tempShape.Color = new RawColor4(0f, 0f, 0f, 1f);

                _tempShape = null;
                _clickedPositions.Clear();
                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        private void CircleClickBehaviour(Point currentPoint)
        {
            if (_clickedPositions.Count == 0)
            {
                _clickedPositions.Add(currentPoint);

                _tempShape = new Circle((int)currentPoint.X, (int)currentPoint.Y, 0, new RawColor4(1f, 0f, 0f, 0.4f));
                _drawModel.Shapes.Add(_tempShape);
                Messenger.Default.Send<DrawModel>(_drawModel);
                return;
            }

            if (_clickedPositions.Count == 1)
            {
                var tempShape = _tempShape as Circle;

                tempShape.Radius = (int)Utility.GetDistance((float)_clickedPositions[0].X, (float)_clickedPositions[0].Y, (float)currentPoint.X,
                    (float)currentPoint.Y);

                _tempShape.Color = new RawColor4(0f, 0f, 0f, 1f);

                _tempShape = null;
                _clickedPositions.Clear();
                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        private void ArcClickBehaviour(Point currentPoint)
        {
            if (_clickedPositions.Count == 0)
            {
                _clickedPositions.Add(currentPoint);

                _tempShape = new Arc((int)currentPoint.X, (int)currentPoint.Y, (int)currentPoint.X, (int)currentPoint.Y, 180, 90, new RawColor4(1f, 0f, 0f, 0.4f));
                _drawModel.Shapes.Add(_tempShape);
                Messenger.Default.Send<DrawModel>(_drawModel);
                return;
            }

            if (_clickedPositions.Count == 1) 
            {
                _drawModel.Shapes.Remove(_tempShape);
                _drawModel.Shapes.Add(new Arc((int)_clickedPositions[0].X, (int)_clickedPositions[0].Y, (int)currentPoint.X, (int)currentPoint.Y, 180, 180, new RawColor4(1f, 0f, 0f, 0.4f))
                {
                    Color = new RawColor4(0f, 0f, 0f, 1f)
                });

                _tempShape = null;
                _clickedPositions.Clear();
                Messenger.Default.Send<DrawModel>(_drawModel);
            }
        }

        #endregion

        private Tuple<ShapeBase, RawVector2>? DetectMouseCollision(List<ShapeBase> shapes, IntPoint position)
        {
            foreach (var shapeBase in shapes)
            {
                if (shapeBase.OuterBox.Left < position.X && shapeBase.OuterBox.Right > position.X &&
                    shapeBase.OuterBox.Top < position.Y && shapeBase.OuterBox.Bottom > position.Y)
                {
                    if (shapeBase is Rose)
                    {
                        foreach (var point in shapeBase.Data)
                        {
                            if (Math.Abs(point.X - position.X) == 0 && Math.Abs(point.Y - position.Y) == 0)
                            {
                                _isModelChanged = true;
                                return new Tuple<ShapeBase, RawVector2>(shapeBase, point);
                            }
                        }
                    }

                    foreach (var point in shapeBase.SplitLine())
                    {
                        if (Math.Abs(point.X - position.X) <= 2 && Math.Abs(point.Y - position.Y) <= 2)
                        {
                            _isModelChanged = true;
                            return new Tuple<ShapeBase, RawVector2>(shapeBase, point);
                        }
                    }

                    if (shapeBase.IsMouseOver)
                    {
                        shapeBase.Color = new RawColor4(0f, 0f, 0f, 1f);
                        shapeBase.IsMouseOver = false;
                        _isModelChanged = true;
                    }
                }
                else
                {
                    if (shapeBase.IsMouseOver)
                    {
                        shapeBase.Color = new RawColor4(0f, 0f, 0f, 1f);
                        shapeBase.IsMouseOver = false;
                        _isModelChanged = true;
                    }
                }
            }

            return null;
        }

        private void ControlClickCommand(MouseButtonEventArgs obj)
        {
            if (_shapeOver != null)
            {
                if (obj.RightButton == MouseButtonState.Pressed)
                {
                    if (_shapeOver is Arc arc)
                    {
                        var inpDialog = new ArcSettingsDialog(arc.Angle);

                        if (inpDialog.ShowDialog() == true)
                            arc.Angle = inpDialog.Angle;
                    }
                }
            }
        }

        private void SceneClick(MouseButtonEventArgs obj)
        {
            if (_selectedTool != ToolType.None)
            {
                var currentPoint = obj.GetPosition(obj.Source as IInputElement);

                if (_selectedTool == ToolType.Line)
                {
                    LineClickBehaviour(currentPoint);
                }

                if (_selectedTool == ToolType.Rectangle)
                {
                    RectangleClickBehaviour(currentPoint);
                }

                if (_selectedTool == ToolType.Circle)
                {
                    CircleClickBehaviour(currentPoint);
                }

                if (_selectedTool == ToolType.Triangle)
                {
                    TriangleClickBehaviour(currentPoint);
                }

                if (_selectedTool == ToolType.Arc)
                {
                    ArcClickBehaviour(currentPoint);
                }
            }
        }

        private void SelectTool(string obj)
        {
            _selectedTool = _selectedTool == Utility.ParseEnum<ToolType>(obj) ? ToolType.None : Utility.ParseEnum<ToolType>(obj);
            Cursor = _selectedTool == ToolType.None ? Cursors.Cross : Cursors.Pen;
            
            RaisePropertyChanged(nameof(Cursor));
        }

        #region FileEvents

        public void CreateFile()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "lab files (*.lab)|*.lab",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.WriteLine("Hello World!");
                    Tabs.Add(new TabModel(Path.GetFileNameWithoutExtension(saveFileDialog.FileName)));
                }
            }
        }

        public void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "lab files (*.lab)|*.lab",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                using (var sw = new StreamWriter(openFileDialog.FileName))
                {
                    Tabs.Add(new TabModel(Path.GetFileNameWithoutExtension(openFileDialog.FileName)));
                    _drawModel.Grid = GridGenerator.GenerateGrid(500, 500, 20, new IntPoint(0, 0));
                }
            }
        }

        public bool CanOpenDialog()
        {
            return !_isDialogOpened;
        }

        #endregion
    }
}