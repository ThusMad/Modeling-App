using System.Security.Cryptography.X509Certificates;
using GalaSoft.MvvmLight;

namespace Modeler.App.Models
{
    public class TabModel : ObservableObject
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public TabModel(string name)
        {
            _name = name;
        }
    }
}