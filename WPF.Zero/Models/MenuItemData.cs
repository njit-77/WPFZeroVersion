using System.Windows.Input;
using WPF.Zero.Shared.MVVM;

namespace WPF.Zero.Models
{
    public class MenuItemData : PropertyChangeBase
    {

        private bool _Checked;

        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                PropertyChangedBaseEx.OnPropertyChanged(this, p => p.Checked);
            }
        }

        public string Header { get; set; }

        public ICommand Command { get; set; }

        public string CommandParameter { get; set; }

    }
}
