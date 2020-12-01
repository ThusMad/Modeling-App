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
    /// Interaction logic for RoseCreationDialog.xaml
    /// </summary>
    public partial class RoseCreationDialog : Window
    {
        public RoseCreationModel RoseCreationModel;

        private bool _isPickEnabled;
        private readonly IInputElement _source;
        public event Action<RoseCreationModel> ModelUpdate;
        public RoseCreationDialog(IInputElement source)
        {
            _source = source;
            _source.MouseLeftButtonDown += SceneClick;
            RoseCreationModel = new RoseCreationModel();

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

            RoseCreationModel.CenterX = (int)position.X;
            RoseCreationModel.CenterY = (int)position.Y;

            ModelUpdate?.Invoke(RoseCreationModel);

            this.ShowDialog();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void KValSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RoseCreationModel.K = (int)e.NewValue;
            KVal.Text = ((int)e.NewValue).ToString();
            ModelUpdate?.Invoke(RoseCreationModel);
        }

        private void StepValSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RoseCreationModel.Step = (float)e.NewValue;
            StepVal.Text = ((float)e.NewValue).ToString();
            ModelUpdate?.Invoke(RoseCreationModel);
        }

        private void YPointValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var value = (e.OriginalSource as TextBox).Text;
            var point = int.Parse(value);
            RoseCreationModel.CenterY = point;
        }

        private void XPointValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var value = (e.OriginalSource as TextBox).Text;
            var point = int.Parse(value);
            RoseCreationModel.CenterX = point;
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
            RoseCreationModel.Size = size;
        }
    }
}
