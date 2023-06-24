using System;
using System.Threading.Tasks;
using System.Windows;
using NewLife.Log;

namespace WPFZeroVersion
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


        public App()
        {
            Startup += (obj, e) =>
            {
                mutex = new System.Threading.Mutex(true, $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name} - 6c5270f9-a098-4da0-a631-04ec12caf74c", out bool ret);
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
                        XTrace.Log.Error("[UI线程]异常：{0}.", exception.Exception);

                        System.Windows.Forms.MessageBox.Show(exception.Exception.ToString(),
                            "Error",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);

                        exception.Handled = true;
                    };

                    /// 设置非UI线程发生异常时处理函数
                    AppDomain.CurrentDomain.UnhandledException += (sender, exception) =>
                    {
                        XTrace.Log.Fatal("[非UI线程]异常：{0}.", exception);

                        System.Windows.Forms.MessageBox.Show("软件出现不可恢复错误，即将关闭。",
                            "Error",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);

                        Environment.Exit(0);
                    };

                    /// 设置托管代码异步线程发生异常时处理函数
                    TaskScheduler.UnobservedTaskException += (sender, exception) =>
                    {
                        /// NewLife.Log写法
                        foreach (Exception innerException in exception.Exception.Flatten().InnerExceptions)
                        {
                            XTrace.Log.Error("[Task]异常：{0}.", exception);
                        }
                        exception.SetObserved();
                    };

                    /// 设置非托管代码发生异常时处理函数
                    callBack = new CallBack(ExceptionFilter);
                    SetUnhandledExceptionFilter(callBack);

                    XTrace.WriteVersion();
                }
            };
        }

        private int ExceptionFilter(ref long a)
        {
            XTrace.Log.Fatal("[非托管代码]异常：{0}.", Environment.StackTrace);

            return 1;
        }

    }
}
