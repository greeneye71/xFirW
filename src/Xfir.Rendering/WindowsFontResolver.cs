// SPDX-License-Identifier: EUPL-1.2
using PdfSharp.Fonts;

namespace Xfir.Rendering;

public sealed class WindowsFontResolver : IFontResolver
{
    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic) =>
        new(isBold ? (isItalic ? "arialbi" : "arialbd") : (isItalic ? "ariali" : "arial"));

    public byte[] GetFont(string faceName)
    {
        if (faceName is not ("arial" or "arialbd" or "ariali" or "arialbi")) throw new ArgumentException("Carattere non previsto.");
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), faceName + ".ttf");
        if (!File.Exists(path)) throw new InvalidOperationException("Il carattere di sistema Arial non è disponibile.");
        return File.ReadAllBytes(path);
    }
}
