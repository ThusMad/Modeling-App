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
    /// Interaction logic for ShiftDialog.xaml
    /// </summary>
    public partial class ShiftDialog : Window
    {
        public event Action<int> XChangedDelegate;
        public event Action<int> YChangedDelegate;

        public ShiftDialog(int xShift = 0, int yShift = 0)
        {
            InitializeComponent();

            YValSlider.Value = yShift;
            XValSlider.Value = xShift;
        }

        private void XChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            xValText.Text = ((int) e.NewValue).ToString();
            XSHift = (int)e.NewValue;
            XChangedDelegate?.Invoke((int)e.NewValue);
        }

        private void YChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            yValText.Text = ((int)e.NewValue).ToString();
            YSHift = (int) e.NewValue; 
            YChangedDelegate?.Invoke((int)e.NewValue);
        }

        public int XSHift { get; set; }
        public int YSHift { get; set; }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
