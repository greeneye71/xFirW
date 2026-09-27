# xFirW

Visualizzatore di formulari digitali XFIR per Windows, con anteprima, esportazione PDF e stampa.

Il progetto è in fase iniziale di progettazione: non sono ancora disponibili un'applicazione eseguibile o una release.

Le funzioni previste sono:

- Apertura di file `.xfir`, anche tramite trascinamento e associazione in Windows.
- Lettura locale del contenitore ASiC-E e dei dati XML.
- Rappresentazione del formulario con impaginazione vicina al modello RENTRI.
- Anteprima, esportazione PDF e stampa dello stesso documento.
- Visualizzazione di soggetti, tratte, eventi, firme e allegati.
- Controlli di struttura con indicazione esplicita dei limiti di validazione.

Lo stack proposto è C# con .NET 10 LTS e WPF, PDFsharp per la generazione del PDF, WebView2 per l'anteprima e MSIX per la distribuzione Windows. La pubblicazione nel Microsoft Store è un obiettivo del progetto.

L'apertura e la rappresentazione dei documenti sono progettate per funzionare localmente, senza richiedere un account RENTRI. La verifica completa delle firme e dello stato online del formulario è una funzionalità distinta dalla visualizzazione.

La proposta tecnica e le tappe di sviluppo sono descritte in [docs/PROGETTO.md](docs/PROGETTO.md).

Non pubblicare formulari reali, certificati privati, credenziali o documenti contenenti dati personali nelle issue o nelle pull request. I casi di test dovranno utilizzare dati sintetici o adeguatamente anonimizzati.

Progetto indipendente, non affiliato al RENTRI o a Microsoft.

## Licenza e uso commerciale

Copyright (C) 2026 Giovanni Bergamaschi e i contributori di xFirW.

Il codice e la documentazione originali del progetto sono distribuiti sotto **GNU Affero General Public License, versione 3 soltanto** (`AGPL-3.0-only`). Il testo completo è in [LICENSE](LICENSE). Il software è fornito senza garanzie, nei termini della licenza.

L'uso commerciale è consentito senza royalty e senza richiedere un'autorizzazione. Le versioni derivate distribuite devono rispettare il copyleft e gli obblighi di disponibilità del codice sorgente corrispondente. Una versione modificata che supporta l'interazione remota tramite rete deve offrire il sorgente corrispondente agli utenti che vi interagiscono, come previsto dalla sezione 13. La licenza non impone la pubblicazione indiscriminata di tutte le modifiche private né l'invio delle modifiche a questo repository.

Se utilizzi xFirW in un progetto commerciale, ci farebbe piacere ricevere una [segnalazione volontaria](https://github.com/greeneye71/xFirW/issues/new?template=uso-commerciale.yml). È una richiesta di cortesia, non una condizione della licenza: non richiede approvazione e la mancata segnalazione non limita i diritti concessi. I dettagli sono in [COMMERCIAL_USE.md](COMMERCIAL_USE.md).

La licenza del programma non si applica automaticamente ai formulari degli utenti o ai dati e PDF generati da essi. Le dipendenze e gli eventuali materiali di terzi mantengono le rispettive licenze.

Per proporre modifiche, consulta [CONTRIBUTING.md](CONTRIBUTING.md).
