using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Modeler.App.Models;
using Modeler.Core;
using Modeler.Core.Enums;
using Modeler.Core.Models;
using Modeler.Core.Shapes;
using Modeler.Core.Utilities;
using Modeler.Renderer;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace Modeler.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
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
            FileCreateCommand = new RelayCommand(CreateFile, CanOpenDialog);
            FileOpenCommand = new RelayCommand(OpenFile, CanOpenDialog);
            RefreshCommand = new RelayCommand(RefreshScene);
            SizeChangedCommand = new RelayCommand<SizeChangedEventArgs>(OnSizeChanged);
            SelectToolCommand = new RelayCommand<string>(SelectTool);
            HorizontalHeldCommand = new RelayCommand(HorizontalHeld);
            VerticalHeldCommand = new RelayCommand(VerticalHeld);
            SceneClickCommand = new RelayCommand<MouseButtonEventArgs>(SceneClick);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMove);
            
            Tabs = new ObservableCollection<TabModel>();
        }

        private void HorizontalHeld()
        {
            _isHorizontalHeldActive = !_isHorizontalHeldActive;
        }

        private void VerticalHeld()
        {
            _isVerticalHeldActive = !_isVerticalHeldActive;
        }

        public RelayCommand FileCreateCommand { get; private set; }
        public RelayCommand FileOpenCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand<string> SelectToolCommand { get; private set; }
        public RelayCommand HorizontalHeldCommand { get; private set; }
        public RelayCommand VerticalHeldCommand { get; private set; }
        public RelayCommand<SizeChangedEventArgs> SizeChangedCommand { get; private set; }
        public RelayCommand<MouseButtonEventArgs> SceneClickCommand { get; private set; }
        public RelayCommand<MouseEventArgs> MouseMoveCommand { get; private set; }
        public Cursor Cursor { get; set; }

        public ObservableCollection<TabModel> Tabs { get; set; }

        private void MouseMove(MouseEventArgs obj)
        {
            if (_tempShape != null)
            {
                var currentPoint = obj.GetPosition(obj.Source as IInputElement);

                if (_selectedTool == ToolType.Line)
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
            }
        }

        private void SceneClick(MouseButtonEventArgs obj)
        {
            if (_selectedTool != ToolType.None)
            {
                var currentPoint = obj.GetPosition(obj.Source as IInputElement);

                if (_selectedTool == ToolType.Line)
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
                        _tempShape = null;
                        _clickedPositions.Clear();
                        Messenger.Default.Send<DrawModel>(_drawModel);
                    }
                    
                }
            }
        }

        private void RefreshScene()
        {
            Cursor = Cursors.AppStarting;
            Messenger.Default.Send<DrawModel>(_drawModel);
        }

        private void SelectTool(string obj)
        {
            _selectedTool = _selectedTool == Utility.ParseEnum<ToolType>(obj) ? ToolType.None : Utility.ParseEnum<ToolType>(obj);
            Cursor = _selectedTool == ToolType.None ? Cursors.Hand : Cursors.Cross;
            
            RaisePropertyChanged(nameof(Cursor));
        }

        private void OnSizeChanged(SizeChangedEventArgs obj)
        {
            _drawModel.Grid = GridGenerator.GenerateGrid((int)obj.NewSize.Width - 50, (int)obj.NewSize.Height - 150, 20, new IntPoint(60, 20));
            _sceneSize = new IntPoint((int)obj.NewSize.Width - 50, (int)obj.NewSize.Height - 150);
            Messenger.Default.Send<DrawModel>(_drawModel);
        }

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
                    _drawModel.Grid = GridGenerator.GenerateGrid(500, 500, 20, new IntPoint(60, 20));
                }
            }
        }

        public bool CanOpenDialog()
        {
            return !_isDialogOpened;
        }
    }
}