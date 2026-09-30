// SPDX-License-Identifier: EUPL-1.2
using System.Windows;

namespace Xfir.App;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        VersionLabel.Text = $"Versione {AppInfo.Version} · Prototipo";
        AuthorLabel.Text = AppInfo.Author;
        EmailLabel.Text = AppInfo.ContactEmail;
        DisclaimerLabel.Text = AppInfo.Disclaimer;
    }

    private void OnClose(object sender, RoutedEventArgs e) => Close();
}
