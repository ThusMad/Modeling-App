using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Modeler.App.Models;
using Modeler.Core;
using Modeler.Core.Utilities;
using Modeler.Renderer;
using SharpDX.Direct2D1;

namespace Modeler.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isDialogOpened = false;

        public MainViewModel()
        {
            FileCreateCommand = new RelayCommand(CreateFile, CanOpenDialog);
            FileOpenCommand = new RelayCommand(OpenFile, CanOpenDialog);
            RefreshCommand = new RelayCommand(RefreshScene);
            SizeChangedCommand = new RelayCommand<SizeChangedEventArgs>(OnSizeChanged);
            Tabs = new ObservableCollection<TabModel>();
            Grid = new List<ShapeBase>();
            Shapes = new List<ShapeBase>();
        }

        public List<ShapeBase> Grid { get; private set; }
        public List<ShapeBase> Shapes { get; private set; }
        public RelayCommand FileCreateCommand { get; private set; }
        public RelayCommand FileOpenCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand<SizeChangedEventArgs> SizeChangedCommand { get; private set; }
        public Cursor Cursor { get; set; }

        public ObservableCollection<TabModel> Tabs { get; set; }

        private void RefreshScene()
        {
            Cursor = Cursors.AppStarting;
            RaisePropertyChanged(nameof(Grid));
        }

        private void OnSizeChanged(SizeChangedEventArgs obj)
        {
            Grid = GridGenerator.GenerateGrid((int)obj.NewSize.Width - 50, (int)obj.NewSize.Height - 150, 20, new IntPoint(60, 20));
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
                    Grid = GridGenerator.GenerateGrid(500, 500, 20, new IntPoint(60, 20));
                }
            }
        }

        public bool CanOpenDialog()
        {
            return !_isDialogOpened;
        }
    }
}