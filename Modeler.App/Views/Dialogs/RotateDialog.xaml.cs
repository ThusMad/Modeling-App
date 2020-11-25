using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Modeler.App.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for RotateDialog.xaml
    /// </summary>
    public partial class RotateDialog : Window
    {
        private readonly IInputElement _source;
        private bool _isPickEnabled;
        public event Action<int> RotationDelegate;
        public event Action<int, int> PointDelegate;

        public RotateDialog(IInputElement source)
        {
            InitializeComponent();
            Closed += OnClosed;
            _source = source;
            source.MouseLeftButtonDown += SceneClick;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _source.MouseLeftButtonDown -= SceneClick;
        }

        private void SceneClick(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(_source);
            xPointValue.Text = position.X.ToString();
            yPointValue.Text = position.Y.ToString();
            rotValText.Text = 0.ToString();
            XValSlider.Value = 0;
            _isPickEnabled = false;
            this.ShowDialog();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void RotateChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rotValText.Text = ((int)e.NewValue).ToString();
            Angle = (int)e.NewValue;
            RotationDelegate?.Invoke((int)e.NewValue);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _isPickEnabled = true;
            this.Hide();
        }

        private void XPointValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            X = int.Parse(xPointValue.Text);
            PointDelegate?.Invoke(X, Y);
        }

        private void YPointValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Y = int.Parse(yPointValue.Text);
            PointDelegate?.Invoke(X, Y);
        }

        public int Angle { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
