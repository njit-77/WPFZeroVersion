using Caliburn.Micro;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Timers;
using System.Windows.Documents;
using System.Windows.Input;
using WPF.Zero.Models;
using WPF.Zero.Shared.MVVM;

namespace WPF.Zero.ViewModels
{
    [Export(typeof(IScreen))]
    class MainViewModel : Conductor<IScreen>.Collection.OneActive
    {

        public static MainViewModel Instance { get; set; }


        [ImportingConstructor]
        public MainViewModel(IWindowManager _windowManager, IEventAggregator _eventAggregator)
        {
            windowManager = _windowManager;
            eventAggregator = _eventAggregator;

            Instance = this;
        }


        #region Field/Property

        private readonly IWindowManager windowManager;

        private readonly IEventAggregator eventAggregator;

        public WPF.Zero.SharedProject.Log.Logger logger { get; private set; }

        #region Screen

        [ImportMany(typeof(WPF.Zero.IView.IView))]
        public List<WPF.Zero.IView.IView> Plugins { get; set; }

        #endregion


        #region Bind Property

        private int cur_view_index = -1;

        public BindableCollection<MenuItemData> MenuItems { get; set; } = new BindableCollection<MenuItemData>();

        private System.Windows.Documents.Paragraph logParagraph = new System.Windows.Documents.Paragraph();

        public System.Windows.Documents.Paragraph LogParagraph
        {
            get => logParagraph;
            set
            {
                logParagraph = value;
                NotifyOfPropertyChange(() => LogParagraph);
            }
        }

        #endregion


        #endregion


        #region Method

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            eventAggregator.SubscribeOnUIThread(this);

            Init();
        }

        private void Init()
        {
            LogInit();
            AddMenuItems();
        }

        private void UnInit()
        {

        }

        private System.Timers.Timer send_timer;

        private void LogInit()
        {
            if (logger == null)
            {
                logger = new WPF.Zero.SharedProject.Log.Logger();

                logger.Log += (level, msg) =>
                {
                    System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        var run = new Run(msg + Environment.NewLine);
                        if ((int)level <= (int)LogLevel.Info)
                        {
                            run.Background = System.Windows.Media.Brushes.Green;
                        }
                        else if ((int)level == (int)LogLevel.Warn)
                        {
                            run.Background = System.Windows.Media.Brushes.OrangeRed;
                        }
                        else
                        {
                            run.Background = System.Windows.Media.Brushes.Red;
                        }
                        if (LogParagraph.Inlines.Count >= 300)
                        {
                            LogParagraph.Inlines.Remove(LogParagraph.Inlines.FirstInline);
                        }
                        LogParagraph.Inlines.Add(run);
                    });
                };
            }

            if (send_timer == null)
            {
                send_timer = new System.Timers.Timer()
                {
                    Interval = 1000,
                    AutoReset = true,
                };
                send_timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    logger.Info(NewLife.Security.Rand.NextString(7));
                    logger.Warn(NewLife.Security.Rand.NextString(7));
                    logger.Error(NewLife.Security.Rand.NextString(7));
                };
                send_timer.Start();
            }
        }

        private void AddMenuItems()
        {
            MenuItems.Clear();

            foreach (var item in Plugins)
            {
                MenuItems.Add(new MenuItemData()
                {
                    Checked = false,

                    Header = item.Get_Name(),

                    CommandParameter = item.Get_Name(),

                    Command = new RelayCommand<string>((string header) =>
                    {
                        MenuCommand(header);
                    }),
                });
            }

            if (MenuItems.Count > 0)
            {
                MenuCommand(MenuItems[0].CommandParameter);
            }
        }

        private void MenuCommand(string header)
        {
            int check_view_index = -1;
            for (int i = 0; i < MenuItems.Count; i++)
            {
                if (MenuItems[i].CommandParameter == header)
                {
                    check_view_index = i;
                    break;
                }
            }

            if (cur_view_index != check_view_index)
            {
                if (cur_view_index != -1)
                {
                    MenuItems[cur_view_index].Checked = false;
                    Plugins[cur_view_index].UnInit();
                }
                if (check_view_index != -1)
                {
                    MenuItems[check_view_index].Checked = true;
                    Plugins[check_view_index].Init();

                    UpdateActiveItem(header);
                }

                cur_view_index = check_view_index;
            }
        }

        private void UpdateActiveItem(string header)
        {
            WPF.Zero.IView.IView view;
            if (string.IsNullOrEmpty(header))
            {
                view = Plugins[0];
            }
            else
            {
                view = Plugins.Find(t => t.Get_Description() == header);
            }

            ActiveItem = view as IScreen;
        }

        #endregion



        #region Menu Command

        public ICommand ExitCommand =>
            new RelayCommand(() =>
            {
                System.Windows.Application.Current.Shutdown();
            }, () => true);

        #endregion

    }
}
