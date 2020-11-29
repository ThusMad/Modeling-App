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
using Modeler.Core.Models;
using SharpDX.Mathematics.Interop;

namespace Modeler.App.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for HomographyTransformationDialog.xaml
    /// </summary>
    public partial class HomographyTransformationDialog : Window
    {
        public HomographyTransformationModel Transformation { get; set; }

        public event Action<HomographyTransformationModel> HomographyTransformationDelegate;

        private RawVector3 R0 = new RawVector3(0, 0, 0);
        private RawVector3 RX = new RawVector3(0, 0, 0);
        private RawVector3 RY = new RawVector3(0, 0, 0);

        public HomographyTransformationDialog()
        {
            Transformation = new HomographyTransformationModel();

            InitializeComponent();
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = true;
        }

        #region RY

        private void RYYSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            YY.Text = ((float)e.NewValue).ToString();
            RY.Y = (float)e.NewValue;
            Transformation.RY = RY;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        private void RYXSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            YX.Text = ((float)e.NewValue).ToString();
            RY.X = (float)e.NewValue;
            Transformation.RY = RY;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        private void RYWSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            YW.Text = ((float)e.NewValue).ToString();
            RY.Z = (float)e.NewValue;
            Transformation.RY = RY;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        #endregion



        #region RX

        private void RXXSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            XX.Text = ((float)e.NewValue).ToString();
            RX.X = (float)e.NewValue;
            Transformation.RX = RX;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        private void RXYSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            XY.Text = ((float)e.NewValue).ToString();
            RX.Y = (float)e.NewValue;
            Transformation.RX = RX;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        private void RXWSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            XW.Text = ((float)e.NewValue).ToString();
            RX.Z = (float)e.NewValue;
            Transformation.RX = RX;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        #endregion



        #region R0

        private void R0YSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R0Y.Text = ((float)e.NewValue).ToString();
            R0.Y = (float)e.NewValue;
            Transformation.R0 = R0;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        private void R0XSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R0X.Text = ((float)e.NewValue).ToString();
            R0.X = (float)e.NewValue;
            Transformation.R0 = R0;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        private void R0WSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R0W.Text = ((float)e.NewValue).ToString();
            R0.Z = (float)e.NewValue;
            Transformation.R0 = R0;

            HomographyTransformationDelegate?.Invoke(Transformation);
        }

        #endregion

    }
}
