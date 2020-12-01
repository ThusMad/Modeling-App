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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Modeler.Core.Attributes;

namespace Modeler.App.Views.Controls
{
    /// <summary>
    /// Interaction logic for ObjectControl.xaml
    /// </summary>
    public partial class ObjectControl : UserControl
    {
        private object _shape;

        public ObjectControl(object shape)
        {
            _shape = shape;
            InitializeComponent();

            var elements = InitFields(shape);

            foreach (var uiElement in elements)
            {
                Props.Children.Add(uiElement);
            }

            Expander.Header = shape.GetType().Name;
        }

        public ICollection<UIElement> InitFields(object shape)
        {
            var list = new List<UIElement>();
            var type = shape.GetType();

            var fields = type.GetFields();
            foreach (var fieldInfo in fields)
            {
                foreach (var editAttr in (ShapeEditAttribute[]) fieldInfo.GetCustomAttributes(typeof(ShapeEditAttribute), true))
                {
                    if (editAttr != null)
                    {
                        list.Add(BuildInput(editAttr));
                    }
                }
            }

            return list;
        }

        private UIElement BuildInput(ShapeEditAttribute attribute)
        {
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            stackPanel.Children.Add(new TextBlock()
            {
                Text = attribute.Label
            });

            stackPanel.Children.Add(new TextBox());

            return null;
        }
    }
}
