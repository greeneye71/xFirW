// SPDX-License-Identifier: EUPL-1.2
using Microsoft.Win32;

namespace Xfir.App;

/// <summary>Per-user record of the accepted disclaimer revision (HKCU, no admin rights needed).</summary>
internal static class DisclaimerStore
{
    private const string KeyPath = @"Software\xFirW";
    private const string ValueName = "DisclaimerAcceptedRevision";

    public static bool IsAccepted()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(KeyPath);
            return key?.GetValue(ValueName) is int revision && revision >= AppInfo.DisclaimerRevision;
        }
        catch (Exception) { return false; }
    }

    public static void MarkAccepted()
    {
        try
        {
            using var key = Registry.CurrentUser.CreateSubKey(KeyPath);
            key.SetValue(ValueName, AppInfo.DisclaimerRevision, RegistryValueKind.DWord);
        }
        catch (Exception) { /* Not persisted: the notice will simply be shown again next time. */ }
    }
}
