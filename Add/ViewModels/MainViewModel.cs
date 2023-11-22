using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Reflection;
using WPF.Zero.IView;

namespace Add.ViewModels
{
    [Export(typeof(IView))]
    public class MainViewModel : Caliburn.Micro.Screen, IView
    {

        [ImportingConstructor]
        public MainViewModel(IWindowManager _windowManager,
            IEventAggregator _eventAggregator)
        {
            windowManager = _windowManager;
            eventAggregator = _eventAggregator;
        }


        #region Field/Property

        private readonly IWindowManager windowManager;

        private readonly IEventAggregator eventAggregator;


        #region IView

        private static System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        public int Get_Property()
        {
            return 0;
        }

        public string Get_Name()
        {
            return "Add";
        }

        public string Get_Description()
        {
            return "Add";
        }

        public string Get_Version()
        {
            return $"{Get_Description()} {info.ProductVersion} ({info.FileDescription})";
        }

        public void Init()
        {

        }

        public void UnInit()
        {

        }

        #endregion


        #region Bind Property

        private int test = 1;

        public int Test
        {
            get => test;
            set
            {
                test = value;
                NotifyOfPropertyChange(() => Test);
            }
        }


        private string version;

        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                NotifyOfPropertyChange(() => Version);
            }
        }


        #endregion


        #endregion


        #region Method

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            eventAggregator.SubscribeOnUIThread(this);

            Version = Get_Version();
        }

        #endregion


        #region Command

        public void Add()
        {
            Test++;
        }

        #endregion


    }
}
