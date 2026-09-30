// SPDX-License-Identifier: EUPL-1.2
using System.Windows;

namespace Xfir.App;

public partial class App : Application
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        if (!DisclaimerStore.IsAccepted())
        {
            // Keep the process alive while the notice is the only window.
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            if (new DisclaimerWindow().ShowDialog() != true)
            {
                Shutdown(1);
                return;
            }
            DisclaimerStore.MarkAccepted();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        var window = new MainWindow(e.Args.FirstOrDefault());
        MainWindow = window;
        window.Show();
    }
}
