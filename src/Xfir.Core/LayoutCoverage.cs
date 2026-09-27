// SPDX-License-Identifier: AGPL-3.0-only
using System.Xml.Linq;

namespace Xfir.Core;

// Only fields actually rendered (or used for a relationship) belong here.
// New schema fields automatically become visible in the supplementary appendix.
internal static class LayoutCoverage
{
    private static readonly HashSet<string> DepartureFields = new(StringComparer.Ordinal)
    {
        "DataEmissione", "NumeroFIR", "Annotazioni",
        "Rifiuto/CodiceEER", "Rifiuto/Descrizione", "Rifiuto/StatoFisico", "Rifiuto/Provenienza",
        "Rifiuto/Quantita", "Rifiuto/VerificatoInPartenza", "Rifiuto/NumeroColli", "Rifiuto/Rinfusa",
        "Rifiuto/CaratteristicheChimicoFisiche", "Rifiuto/ClassiPericolo/ClassePericolo", "Rifiuto/TrasportoADR",
        "Rifiuto/DatiADR/Classe", "Rifiuto/DatiADR/NumeroONU", "Rifiuto/DatiADR/Note"
    };
    private static readonly HashSet<string> TransportFields = new(StringComparer.Ordinal)
    {
        "TrasportoTerrestre/Conducente/Nome", "TrasportoTerrestre/Conducente/Cognome", "TrasportoTerrestre/TargaAutomezzo",
        "TrasportoTerrestre/TargaRimorchio", "TrasportoTerrestre/DataOraInizioTrasporto", "TrasportoTerrestre/Percorso"
    };
    private static readonly HashSet<string> AcceptanceFields = new(StringComparer.Ordinal)
    {
        "TipoAccettazione", "QuantitaAccettata", "DataOraArrivo", "AttesaVerificaAnalitica", "MotivoRespingimento"
    };
    internal static IReadOnlyList<DataField> Unmapped(XmlPart? departure, XmlPart? transport, XmlPart? acceptance)
    {
        var allowed = new HashSet<string>(DepartureFields, StringComparer.Ordinal);
        foreach (var actor in new[] { "Produttore", "Destinatario" })
        {
            foreach (var field in new[] { "Denominazione", "CodiceFiscale", "Indirizzo/Indirizzo", "Indirizzo/Civico", "Indirizzo/CAP", "Indirizzo/Citta/Comune", "Indirizzo/Citta/ComuneEstero", "Autorizzazione/Numero", "Autorizzazione/Tipo" })
                allowed.Add(actor + "/" + field);
        }
        allowed.Add("Produttore/NumeroIscrizioneAlbo");
        allowed.Add("Destinatario/Attivita");
        foreach (var field in new[] { "Denominazione", "CodiceFiscale", "NumeroIscrizioneAlbo", "NumIscrSito", "TipoTrasporto" }) allowed.Add("Trasportatori/Trasportatore/" + field);
        foreach (var field in new[] { "Denominazione", "CodiceFiscale", "NumeroIscrizioneAlbo" }) allowed.Add("Intermediari/Intermediario/" + field);
        return Fields(departure, allowed).Concat(Fields(transport, TransportFields)).Concat(Fields(acceptance, AcceptanceFields)).ToList();
    }
    private static IEnumerable<DataField> Fields(XmlPart? part, HashSet<string> allowed)
    {
        if (part is null) yield break;
        foreach (var leaf in part.Root.Descendants().Where(e => !e.HasElements && !string.IsNullOrWhiteSpace(e.Value)))
        {
            var path = string.Join('/', leaf.AncestorsAndSelf().TakeWhile(e => e != part.Root).Reverse().Select(e => e.Name.LocalName));
            if (leaf.Name.Namespace != part.Root.Name.Namespace || !allowed.Contains(path))
                yield return new DataField(part.Name + " · " + path, leaf.Value.Trim());
        }
    }
}
