// SPDX-License-Identifier: AGPL-3.0-only
using System.Text.Json;
using Xfir.Core;
using Xfir.Rendering;

if (args.Length != 2 || args[0] is "--help" or "-h")
{
    Console.WriteLine("Uso: Xfir.Cli <documento.xfir> <copia.pdf | --inspect>\nNon sovrascrive file esistenti.");
    return args.Length == 1 ? 0 : 2;
}
try
{
    var document = new XfirReader().Read(args[0]);
    if (args[1] == "--inspect")
        Console.WriteLine(JsonSerializer.Serialize(new { document.Number, document.State, document.Producer, document.Recipient,
            document.WasteCode, document.Quantity, document.Unit, document.SourceSha256, document.Warnings,
            Signatures = document.Signatures.Count, Parts = document.Parts.Select(p => p.Name) }, new JsonSerializerOptions { WriteIndented = true }));
    else
    {
        var pdf = new FormPdfRenderer().Render(document);
        using var output = new FileStream(args[1], FileMode.CreateNew, FileAccess.Write);
        output.Write(pdf);
        Console.WriteLine("PDF creato: " + Path.GetFullPath(args[1]));
        foreach (var warning in document.Warnings) Console.Error.WriteLine("Avviso: " + warning);
    }
    return 0;
}
catch (Exception e) when (e is XfirReadException or IOException or UnauthorizedAccessException or InvalidOperationException)
{
    Console.Error.WriteLine(e.Message);
    return 1;
}
