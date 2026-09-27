# Proposta tecnica

L'obiettivo iniziale è leggere un documento XFIR e produrre una rappresentazione PDF fedele nei contenuti e vicina alla disposizione grafica del formulario di riferimento.

Il contenitore XFIR è basato su ASiC-E/ZIP. Può comprendere dati XML di vidimazione, partenza, trasporto, accettazione ed eventi successivi, metadati, manifest, firme XAdES e un oggetto CBOR utilizzato per il QR di vidimazione.

Il flusso previsto è:

```text
XFIR -> lettura e controlli -> modello del formulario -> PDF
                                                       |
                                             anteprima, export, stampa
```

Le responsabilità verranno separate tra lettore del contenitore, modello del formulario, validazione, generazione PDF, interfaccia Windows e packaging. La struttura definitiva dei progetti sarà stabilita durante il prototipo.

Le scelte proposte sono C#/.NET 10 LTS, WPF, PDFsharp, WebView2 e MSIX. Il prototipo dovrà verificare la resa del PDF e la stampa reale prima di consolidare le dipendenze. Le licenze e la redistribuibilità delle dipendenze verranno verificate prima della loro inclusione.

Il lettore dovrà gestire namespace, tabelle di codifica versionate, relazioni `id`/`idRef`, quantità decimali e fuso italiano con ora legale. Dati mancanti, valori falsi e zero dovranno rimanere distinti. I byte originari non devono essere modificati.

Le pagine verranno costruite con testo e linee vettoriali, QR derivato dal CBOR originale, campi numerati e gestione esplicita delle eccedenze. Il numero di pagine dipenderà dagli eventi presenti; il documento esportato sarà lo stesso mostrato in anteprima e stampato.

La validazione distinguerà leggibilità, struttura, integrità crittografica e attendibilità dei certificati. L'identificazione di una firma non deve essere presentata come verifica completa. Il PDF esportato è una rappresentazione del contenuto e non eredita le firme digitali dell'XFIR.

Il parser dovrà limitare dimensioni e numero degli elementi decompressi, rifiutare percorsi anomali e nomi duplicati, disabilitare DTD ed entità esterne e risolvere gli schemi soltanto da risorse locali controllate. I riferimenti delle firme dovranno essere risolti nel contenitore. Gli allegati non saranno eseguiti automaticamente.

Il lavoro è previsto in tre tappe:

1. Prototipo: apertura di un XFIR, PDF, anteprima e stampa A4 con verifica del QR.
2. Prima versione: interfaccia, controlli, eventi aggiuntivi, allegati e collaudo con dati sintetici o anonimizzati.
3. Distribuzione: pacchetto MSIX, associazione `.xfir`, installazione e aggiornamento su macchina pulita, preparazione della pubblicazione nello Store.

Serviranno casi di prova per documenti in corso, accettazione parziale, respingimento, tratte multiple, trasbordi, soste, destinatari successivi, annotazioni lunghe, allegati e input non validi. La verifica avanzata delle firme richiederà una prova tecnica dedicata.

Riferimenti ufficiali:

- [Guida tecnica RENTRI](https://api.rentri.gov.it/docs?page=guida-tecnica-struttura-fir-digitale)
- [Schemi XSD](https://api.rentri.gov.it/docs?page=schemi-xsd)
- [Controlli di validazione XFIR](https://api.rentri.gov.it/docs?page=controlli-validazione-xfir)
- [WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/)
- [PDFsharp](https://docs.pdfsharp.net/)
- [Distribuzione nello Store](https://learn.microsoft.com/en-us/windows/apps/distribute-through-store/how-to-distribute-your-win32-app-through-microsoft-store)
