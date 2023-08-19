using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Windows.Input;
using WPF.Zero.Shared.Utility;

namespace WPF.Zero.ViewModels
{
    [Export(typeof(IScreen))]
    class MainViewModel : Conductor<IScreen>.Collection.OneActive
    {

        [ImportingConstructor]
        public MainViewModel(IWindowManager _windowManager, IEventAggregator _eventAggregator)
        {
            windowManager = _windowManager;
            eventAggregator = _eventAggregator;
        }


        #region Field/Property

        private readonly IWindowManager windowManager;

        private readonly IEventAggregator eventAggregator;

        [Import]
        private VersionViewModel versionViewModel { get; set; }

        #endregion


        #region Menu Command

        public ICommand ExitCommand =>
            new RelayCommand(() =>
            {
                System.Windows.Application.Current.Shutdown();
            }, () => true);

        public void About()
        {
            windowManager.ShowDialogAsync(versionViewModel);
        }

        #endregion

    }
}
