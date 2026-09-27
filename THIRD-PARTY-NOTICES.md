# Componenti di terzi

Il codice originale di xFirW è AGPL-3.0-only. Le dipendenze mantengono le loro licenze. I pacchetti sono identificati dai file di progetto e dai `packages.lock.json`.

| Componente | Versione iniziale | Licenza / termini |
| --- | --- | --- |
| PDFsharp | 6.2.4 | MIT, empira Software GmbH |
| QRCoder | 1.6.0 | MIT, Raffael Herrmann |
| Microsoft.Web.WebView2 | 1.0.3537.50 | Termini Microsoft del pacchetto NuGet e del runtime |
| .NET / WPF | 10 | Licenze e avvisi della distribuzione Microsoft .NET |

La cartella `third-party` contiene le licenze e gli avvisi dei componenti distribuiti, incluse le dipendenze transitive Microsoft.Extensions.DependencyInjection.Abstractions e Microsoft.Extensions.Logging.Abstractions. Questa cartella viene copiata accanto all'eseguibile e deve essere mantenuta nelle distribuzioni. Il runtime WebView2 Evergreen è un prerequisito separato e non viene incluso in questa prima pubblicazione del prototipo.

Il generatore PDF legge i caratteri Arial installati con Windows. Non distribuisce file di font nel repository. I formulari e i dati degli utenti non sono parte del codice del progetto.

I test utilizzano xUnit, xunit.runner.visualstudio e Microsoft.NET.Test.Sdk; non sono componenti dell'applicazione distribuita.

Prima di pubblicare un pacchetto destinato allo Store vanno verificati i termini delle versioni effettive e inclusi tutti gli avvisi richiesti.
