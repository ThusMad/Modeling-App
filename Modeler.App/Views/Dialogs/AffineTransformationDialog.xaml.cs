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
using Modeler.Core;
using Modeler.Core.Models;
using SharpDX.Mathematics.Interop;

namespace Modeler.App.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AffineTransformationDialog.xaml
    /// </summary>
    public partial class AffineTransformationDialog : Window
    {
        public AffineTransformationModel Transformation { get; set; }

        public event Action<AffineTransformationModel> AffineTransformationDelegate;

        private RawVector2 R0 = new RawVector2(0, 0);
        private RawVector2 RX = new RawVector2(0, 0);
        private RawVector2 RY = new RawVector2(0, 0);

        public AffineTransformationDialog()
        {
            Transformation = new AffineTransformationModel();
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void RYYSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {   
            YY.Text = ((float)e.NewValue).ToString();
            RY.Y = (float)e.NewValue;
            Transformation.RY = RY;

            AffineTransformationDelegate?.Invoke(Transformation);
        }

        private void RYXSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            YX.Text = ((float)e.NewValue).ToString();
            RY.X = (float)e.NewValue;
            Transformation.RY = RY;

            AffineTransformationDelegate?.Invoke(Transformation);
        }

        private void RXXSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            XX.Text = ((float)e.NewValue).ToString();
            RX.X = (float)e.NewValue;
            Transformation.RX = RX;

            AffineTransformationDelegate?.Invoke(Transformation);
        }

        private void RXYSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            XY.Text = ((float)e.NewValue).ToString();
            RX.Y = (float)e.NewValue;
            Transformation.RX = RX;

            AffineTransformationDelegate?.Invoke(Transformation);
        }

        private void R0YSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R0Y.Text = ((float) e.NewValue).ToString();
            R0.Y = (float)e.NewValue;
            Transformation.R0 = R0;

            AffineTransformationDelegate?.Invoke(Transformation);
        }

        private void R0XSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R0X.Text = ((float)e.NewValue).ToString();
            RX.X = (float)e.NewValue;
            Transformation.RX = RX;

            AffineTransformationDelegate?.Invoke(Transformation);
        }
    }
}
