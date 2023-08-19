using System;
using System.Threading.Tasks;
using System.Windows;
using WPF.Zero.Tools;

namespace WPF.Zero
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private delegate int CallBack(ref long a);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern Int32 SetUnhandledExceptionFilter(CallBack cb);

        private CallBack callBack;

        private System.Threading.Mutex mutex;

        public const string GUID = "6c5270f9-a098-4da0-a631-04ec12caf74c";

        public App()
        {
#if LOG_NEWLIFE
            NewLife.Log.XTrace.Log.Enable = true;
#else
            NewLife.Log.XTrace.Log.Enable = false;
#endif
            Startup += (obj, e) =>
            {
                mutex = new System.Threading.Mutex(true, $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name} - {GUID}", out bool ret);
                if (!ret)
                {
                    System.Windows.MessageBox.Show($"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name} has already started up.",
                        "OnStartup",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);

                    Environment.Exit(0);
                }
                else
                {
                    /// 设置UI线程发生异常时处理函数
                    System.Windows.Application.Current.DispatcherUnhandledException += (sender, exception) =>
                    {
                        LogHelper.Error("[UI线程]异常：{0}.", exception.Exception);

                        System.Windows.MessageBox.Show(exception.Exception.ToString(),
                            "Error",
                            System.Windows.MessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Error);

                        exception.Handled = true;
                    };

                    /// 设置非UI线程发生异常时处理函数
                    AppDomain.CurrentDomain.UnhandledException += (sender, exception) =>
                    {
                        LogHelper.Fatal("[非UI线程]异常：{0}.", exception);

                        System.Windows.MessageBox.Show("软件出现不可恢复错误，即将关闭。",
                            "Error",
                            System.Windows.MessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Error);

                        Environment.Exit(0);
                    };

                    /// 设置托管代码异步线程发生异常时处理函数
                    TaskScheduler.UnobservedTaskException += (sender, exception) =>
                    {
                        LogHelper.Fatal($"Fatal - [Task]异常 GetMessage = {Utility.GetMessage(exception.Exception)}.");
                    };

                    /// 设置非托管代码发生异常时处理函数
                    callBack = new CallBack(ExceptionFilter);
                    SetUnhandledExceptionFilter(callBack);
                }
            };
        }

        private int ExceptionFilter(ref long a)
        {
            LogHelper.Fatal("[非托管代码]异常：{0}.", Environment.StackTrace);

            return 1;
        }

    }
}
