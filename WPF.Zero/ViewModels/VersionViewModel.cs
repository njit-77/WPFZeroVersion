using System.ComponentModel.Composition;
using System.Reflection;

namespace WPF.Zero.ViewModels
{
    [Export]
    class VersionViewModel : Caliburn.Micro.Screen
    {

        public VersionViewModel()
        {
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            VersionText += $"{info.ProductName} {info.ProductVersion} ({info.FileDescription})";
            VersionText += "\r\n";

            VersionText += $"{info.LegalCopyright})";
            VersionText += "\r\n";

            VersionText += $"MIT License";
            VersionText += "\r\n";
        }

        private string versionText;

        public string VersionText
        {
            get { return versionText; }
            set
            {
                if (versionText != value)
                {
                    versionText = value;
                    NotifyOfPropertyChange(() => VersionText);
                }
            }
        }

        public void Close()
        {
            TryCloseAsync(true);
        }
    }
}
