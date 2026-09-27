// SPDX-License-Identifier: AGPL-3.0-only
using System.Windows;

namespace Xfir.App;

public partial class App : Application
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        var window = new MainWindow(e.Args.FirstOrDefault());
        MainWindow = window;
        window.Show();
    }
}
