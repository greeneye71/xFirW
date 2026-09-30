// SPDX-License-Identifier: EUPL-1.2
using System.Reflection;

namespace Xfir.App;

internal static class AppInfo
{
    public const string Author = "Studio ing. Giovanni Bergamaschi";
    public const string ContactEmail = "info@studiobergamaschi.net";

    // Bump when the text changes materially: users must accept the new wording again.
    public const int DisclaimerRevision = 2;

    // Mirrors articles 7 and 8 of the EUPL v1.2: the licence text prevails.
    public const string Disclaimer =
        "xFirW è software gratuito e open source, distribuito con la Licenza pubblica dell'Unione europea (EUPL) v1.2. " +
        "È fornito «così com'è», senza garanzie di alcun tipo, incluse quelle di correttezza, completezza " +
        "o idoneità a uno scopo particolare.\n\n" +
        "Salvi i casi di dolo o di danni direttamente arrecati a persone fisiche, l'autore non è responsabile " +
        "di valori letti, visualizzati, esportati o stampati in modo errato o incompleto, né di danni diretti " +
        "o indiretti derivanti dall'uso o dall'impossibilità di usare il software.\n\n" +
        "L'utente è tenuto a verificare i dati sul documento XFIR originale e sui sistemi ufficiali RENTRI. " +
        "La copia PDF prodotta non sostituisce il formulario digitale originale.";

    public static string Version => typeof(AppInfo).Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0]
        ?? typeof(AppInfo).Assembly.GetName().Version?.ToString(3) ?? "—";
}
