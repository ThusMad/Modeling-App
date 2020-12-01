using System;
using Modeler.Core.Enums;

namespace Modeler.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ShapeEditAttribute : Attribute
    {
        private ShapeViewEditor _editor;
        private string _label;

        public ShapeEditAttribute(string label, ShapeViewEditor editor)
        {
            _label = label;
            _editor = editor;
        }

        public string Label
        {
            get => _label;
            set => _label = value;
        }

        public ShapeViewEditor Editor
        {
            get => _editor;
            set => _editor = value;
        }
    }
}