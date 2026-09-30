// SPDX-License-Identifier: EUPL-1.2
using System.IO;

namespace Xfir.App;

// Preview PDFs contain personal data: each session folder is guarded by a lock file held open while the
// window lives, so folders left behind by a crash can be recognised and removed at the next start.
internal static class PreviewStorage
{
    private const string LockName = "session.lock";

    public static string Root { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "xFirW", "Preview");

    public static FileStream CreateSession(string path)
    {
        Directory.CreateDirectory(path);
        return Lock(path);
    }

    public static void RemoveOrphans()
    {
        if (!Directory.Exists(Root)) return;
        foreach (var folder in Directory.EnumerateDirectories(Root))
        {
            try
            {
                // Fails while another running instance owns the folder.
                using (Lock(folder))
                    foreach (var file in Directory.EnumerateFiles(folder).Where(f => !Path.GetFileName(f).Equals(LockName, StringComparison.OrdinalIgnoreCase)))
                        File.Delete(file);
                Directory.Delete(folder);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }

    private static FileStream Lock(string folder) =>
        new(Path.Combine(folder, LockName), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 1, FileOptions.DeleteOnClose);
}
