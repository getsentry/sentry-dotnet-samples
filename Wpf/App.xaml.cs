using System.Configuration;
using System.Windows;
using Sentry;
using Application = System.Windows.Application;

namespace Wpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Replace this DSN with your own to see the event at Sentry
            SentrySdk.Init(ConfigurationManager.AppSettings["SentryDsn"]);

            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SentrySdk.Close();
        }
    }
}
