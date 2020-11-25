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
    /// Interaction logic for ArcSettingsDialog.xaml
    /// </summary>
    public partial class ArcSettingsDialog : Window
    {
		public ArcSettingsDialog(int defaultValue = 180)
        {
            InitializeComponent();
            angleBox.Text = defaultValue.ToString();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            angleBox.SelectAll();
            angleBox.Focus();
        }

        public int Angle
        {
            get { return int.Parse(angleBox.Text); }
        }
    }
}
