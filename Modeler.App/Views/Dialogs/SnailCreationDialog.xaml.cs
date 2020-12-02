using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Modeler.App.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SnailCreationDialog.xaml
    /// </summary>
    public partial class SnailCreationDialog : Window
    {
        public SnailCreationModel SnailCreationModel;

        private bool _isPickEnabled;
        private readonly IInputElement _source;
        public event Action<SnailCreationModel> ModelUpdate;
        public SnailCreationDialog(IInputElement source)
        {
            _source = source;
            _source.MouseLeftButtonDown += SceneClick;
            SnailCreationModel = new SnailCreationModel();

            InitializeComponent();
            Closed += OnClosed;


        }

        private void OnClosed(object sender, EventArgs e)
        {
            _source.MouseLeftButtonDown -= SceneClick;
        }

        private void SceneClick(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(_source);
            xPointValue.Text = position.X.ToString(CultureInfo.CurrentCulture);
            yPointValue.Text = position.Y.ToString(CultureInfo.CurrentCulture);
            _isPickEnabled = false;

            SnailCreationModel.CenterX = (int)position.X;
            SnailCreationModel.CenterY = (int)position.Y;

            ModelUpdate?.Invoke(SnailCreationModel);

            this.ShowDialog();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void RValSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SnailCreationModel.R = (int)e.NewValue;
            RVal.Text = ((int)e.NewValue).ToString();
            ModelUpdate?.Invoke(SnailCreationModel);
        }

        private void HValSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SnailCreationModel.H = (float)e.NewValue;
            HVal.Text = ((float)e.NewValue).ToString();
            ModelUpdate?.Invoke(SnailCreationModel);
        }

        private void YPointValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var value = (e.OriginalSource as TextBox).Text;
            var point = int.Parse(value);
            SnailCreationModel.CenterY = point;
        }

        private void XPointValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var value = (e.OriginalSource as TextBox).Text;
            var point = int.Parse(value);
            SnailCreationModel.CenterX = point;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _isPickEnabled = true;
            this.Hide();
        }

        private void SizeVal_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var value = (e.OriginalSource as TextBox).Text;
            var size = int.Parse(value);
            SnailCreationModel.Size = size;
        }
    }
}
