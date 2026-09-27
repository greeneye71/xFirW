// SPDX-License-Identifier: AGPL-3.0-only
using System.Text;

namespace Xfir.Core;

public static class Base45
{
    private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
    public static string Encode(ReadOnlySpan<byte> bytes)
    {
        var output = new StringBuilder((bytes.Length * 3 + 1) / 2);
        for (var i = 0; i < bytes.Length; i += 2)
        {
            var pair = i + 1 < bytes.Length;
            var value = pair ? bytes[i] * 256 + bytes[i + 1] : bytes[i];
            output.Append(Alphabet[value % 45]);
            output.Append(Alphabet[value / 45 % 45]);
            if (pair) output.Append(Alphabet[value / 2025]);
        }
        return output.ToString();
    }
}
