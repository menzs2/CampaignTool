# CampaignTool – Projektdokumentation

## 1. Einleitung
Dieses Dokument beschreibt die Konzeption, Zielsetzung und Struktur der WebApp CampaignTool. Es führt Schritt für Schritt durch die funktionalen und nicht-funktionalen Anforderungen, die technische Architektur, Benutzeroberfläche, Teststrategie sowie das Projektmanagement. Es bildet die Grundlage für die Entwicklung und Weiterentwicklung des Tools. Ergänzende Inhalte zur UI-Gestaltung und technischen Umsetzung sind in separaten Dokumenten enthalten (siehe Design-Dokument und technischer Anhang). 

### 1.1 Projektüberblick
CampaignTool ist eine WebApp zur Dokumentation und Verwaltung von Pen-&-Paper-Kampagnen. Das zentrale Element bildet die strukturierte Darstellung von NSCs, SCs, Organisationen und deren Beziehungen. Ziel ist es, Spielleiter:innen ein Werkzeug zur Seite zu stellen, um komplexe Kampagnen übersichtlich, kollaborativ und regelneutral zu managen.

### 1.2 Zielgruppe und Anwendungsbereich
Die Zielgruppe umfasst Spielleiter:innen und Spieler:innen von Pen-&-Paper-Rollenspielen wie *Dungeons & Dragons*, *Das Schwarze Auge* oder *Savage Worlds*. Die Anwendung ist dabei bewusst regelneutral angelegt.

### 1.3 Zielsetzung und Vision
Das Tool soll helfen, Kampagnenwelten konsistent darzustellen, Informationen effektiv zu verknüpfen und Spielern den Zugang zu relevanten Inhalten – je nach Berechtigung – zu ermöglichen. CampaignTool dient außerdem als Lernprojekt zur praktischen Anwendung von Full-Stack-Entwicklung mit .NET-Technologien.

---

## 2. Funktionale Anforderungen

### 2.1 Kernfeatures
- Neue Kampagnen erstellen (SL wird automatisch Admin)
- Kampagnen wechseln
- Entitäten (SC, NSC, Organisation, Ereignis, Item) erstellen, anzeigen, verknüpfen
- Verbindungen mit Typ (Freund, Feind etc.) verwalten
- Standardkampagne in Benutzereinstellungen festlegen
- Entitäten nach Typ, Tag, Verbindung oder Name filtern
- Sicht auf Entitäten und ihre Beziehungen 1. Grades

### 2.2 Filter- und Suchfunktionen
- Filtern nach Typ, Name, Tag, Ereignis
- „Gleicher Name“-Warnung bei Duplikaten
- Verbindungen visuell anzeigen und filtern

### 2.3 Benutzerverwaltung & Rollen
- Benutzererstellung durch Admins
- Gast-Login mit Zugriff auf Beispielkampagne
- Rollensystem (SL, Spieler, Admin)
- Sichtbarkeit von Informationen gemäß Rolle

### 2.4 Sichtbarkeitsregeln und Berechtigungen
- Bestimmte Inhalte nur für SL sichtbar (z. B. GMOnly-Felder)
- Zugriff auf Kampagneninhalte nur per Login
- Authentifizierung über Token
- Kampagnenberechtigungen vom Admin verwaltbar

---

## 3. Nicht-funktionale Anforderungen

### 3.1 Designprinzipien
- Farbpalette: Helles Grau/Beige, Hauptton Dunkelblau (#3465a4)
- UI: einfache Grids mit schwach abgerundeten Ecken
- Schriftarten: Helvetica, Arial, sans-serif
- Einheitliche Formsprache für Desktop & Mobile

### 3.2 Performance und Skalierbarkeit
- Geeignet für Desktop, Tablet, Smartphone
- Browserübergreifende Unterstützung (moderne Browser)
- Optimierung durch getrennte Architektur (Frontend, Backend, DB)

### 3.3 Sicherheit und Datenschutz
- Kein Zugriff ohne Login
- Zugriffstoken zur Authentifizierung
- Schutz vor SQL-Injection & XSS
- Datenverschlüsselung, regelmäßiges Backup
- Hosting bevorzugt in CH/EU oder Azure

### 3.4 Mehrsprachigkeit und Erweiterbarkeit
- Vorerst nur Deutsch
- Internationalisierung (i18n) für spätere Sprachoptionen vorgesehen
- Erweiterbarkeit modular über Features wie Orte, Karten, Regelsysteme
## 4. Technische Architektur

### 4.1 Systemübersicht
CampaignTool basiert auf einer Three-Tier-Architektur mit separatem Frontend (Blazor WebAssembly), Backend (ASP.NET Core) und PostgreSQL-Datenbank. Die Applikation wird als Single Page Application (SPA) realisiert und nutzt eine REST-API zur Kommunikation zwischen Front- und Backend. Eine Containerisierung mit Docker ist vorgesehen.

### 4.2 Technologiestack
- **Frontend**: Blazor WebAssembly (alternativ React)
- **Backend**: ASP.NET Core, C#
- **Datenbank**: PostgreSQL + EntityFrameworkCore
- **Containerisierung**: Docker
- **Styling**: Tailwind CSS oder Bootstrap
- **Testframework**: xUnit
- **Deployment**: bevorzugt Azure oder ein EU-Hosting-Provider

### 4.3 Datenbankmodell
Die Datenbank speichert Benutzer, Kampagnen und Entitäten (SC, NSC, Organisationen, Ereignisse, Items). Beziehungen zwischen Entitäten sind als eigene Verbindungstabellen modelliert. Tags ermöglichen zusätzliche Klassifikation.

*Beispiel-Tabellenstruktur:*
- **User**, **UserRole**, **Campaign**, **Character**, **Organisation**, **Event**, **Item**
- **ConnectionBetweenEntities**
- **Tags** (n-n-Beziehung mit Entitäten)
- **History** (z. B. Zustände, Beziehungen über Zeit)

### 4.4 API-Design & Endpunkte
REST-Endpunkte strukturieren sich nach Ressourcentyp:
- `Campaign`, `Character`, `Organisation`, `Event`, `Item`
- Zugriff und Änderung von Verbindungen (`/Connections`)
- Zugriff auf User- und Kampagneneinstellungen (`/Setting`)
- Authentifizierung via Token (Login/Logout Endpunkte)

Die API folgt dem CRUD-Prinzip mit spezifischen Routen z. B.:
```http
GET /character/bycampaign/{id}
POST /organisation/connection

### 4.5 Containerisierung & Hosting
- Das Backend soll per Docker-Container deploybar sein.
- Hosting-Entscheidung erfolgt spätestens bis 01.09.2025.
- Priorität für Azure (Lerneffekt), alternativ CH/EU-Anbieter.
- Anforderungen: HTTPS-Unterstützung, Backup, Skalierbarkeit

## 5. Benutzeroberfläche (UI)
### 5.1 Mockups / Wireframes
Mockups beschreiben die wichtigsten Screens:
- Login/Willkommen
- Kampagnenauswahl
- Kampagnenübersicht
- Entitäts-Ansicht mit direkter Beziehungskomponente
- Einstellungen für Benutzer/Admin
### 5.2 Navigationskonzept
- Desktop: Hauptnavigation links
- Mobile/Tablet: Navigation als Top-Bar
- Navigation enthält u. a. Kampagnenauswahl, SC/NSC, Organisationen, Ereignisse, Items, Einstellungen, Logout
### 5.3 Benutzerflows
- Login → Kampagnenauswahl oder letzte Kampagne
- Neue Kampagne erstellen (SL wird automatisch Admin)
- Entitätsverwaltung mit Filtern und Verbindungsansicht
- Admin-Ansicht zur Verwaltung von Nutzern und Verbindungs-Typen
### 5.4 Responsive Design
- Das Design berücksichtigt Desktop, Tablet und Mobile
- Komponenten skalieren durch CSS-Framework (Tailwind o. ä.)
- Mobile Views mit ausgeklappter Navigation und optimierter Listenansicht

## 6. Testing & Qualitätssicherung
### 6.1 Teststrategie
- Unit Tests für Backend-Logik und API (xUnit)
- Integrationstests zwischen API und DB
- Manuelle Tests für UI-Funktionalität
- Beta-Tests mit anderen Spielleitern im Bekanntenkreis
### 6.2 Testframeworks und Automatisierung
- Verwendung von xUnit für automatisierte Tests
- GitHub Actions für Continuous Integration in Planung
### 6.3 Beta-Tests und Feedback-Schleifen
- Erste Beta mit Beispielkampagne und Gastzugang
- Rückmeldung durch Entwickler:innen und Spielleiter:innen
- Feedback fließt in Refinement-Phasen ein

## 7. Projektmanagement

### 7.1 Entwicklungsansatz
Das Projekt wird agil umgesetzt, unter Einsatz von GitHub Project zur Organisation von Tasks und Issues. Als Solo-Entwickler liegt der Fokus auf iterativer Entwicklung mit regelmäßigen Review-Phasen.

### 7.2 Meilensteine & Zeitplan
Grobe Planung (abhängig von beruflicher Situation & Kursfortschritt):

- **Februar 2025** – Start Designprozess
- **Mitte März 2025** – Projektstart & GitHub-Repo
- **Mitte Mai 2025** – Vorläufiges Designdokument
- **Ende Mai 2025** – Systemarchitektur
- **Anfang Juli 2025** – Code-Setup Backend + Frontend
- **01.09.2025** – Hosting-Entscheidung

### 7.3 Rollen & Zuständigkeiten
- Momentan Ein-Mann-Projekt (Stephan Menzi)
- Kein Team-Management erforderlich, aber zukünftige Öffnung für Beiträge durch Open Source denkbar

### 7.4 Dokumentation und Wissenssicherung
- Zentrale Dokumentation per Markdown-Dateien im Repository
- Nutzung von README, API-Beschreibungen, ggf. Doku-Plattform in Zukunft

---

## 8. Zukünftige Entwicklungen

### 8.1 Feature-Roadmap (Auswahl & Priorität)
- 🔵 **1. Priorität**
  - Verbindungen 2./3. Grades
  - Bilder & Kartenintegration
  - Vor-/Zurück-Navigation zwischen Entitäten
- 🔷 **2. Priorität**
  - Chronologie / Journal
  - Graph-Darstellung von Beziehungen
  - Spielsitzungen, Namensgenerator
- ⚪ **3.–4. Priorität**
  - Benutzerinformationen selbst ändern
  - Mehrsprachigkeit
  - Weitere Themes
  - Regelspezifische Erweiterungen

### 8.2 Open Source Strategie
- Veröffentlichung unter MIT-Lizenz:
  > „Permission is hereby granted, free of charge…“
- Ziel: Erfahrungen mit Open Source sammeln, Möglichkeit zur Community-Erweiterung

### 8.3 Externe Integration
- Potenzielle Erweiterung:
  - Karten-Tool-Anbindung
  - Export-Formate (PDF, JSON)
  - Externe Datenquellen (z. B. Fantasy-Namen, Generierungstools)

---

## 9. Anhang

### 9.1 Glossar
- **SC** – Spielercharakter
- **NSC / NPC** – Nichtspielercharakter
- **GMOnly** – Nur für Spielleiter sichtbare Information
- **SPA** – Single Page Application
- **Entity** – Sammelbegriff für Charakter, Organisation etc.

### 9.2 Verweise / Quellen
- [MDN – SPA](https://developer.mozilla.org/de/docs/Glossary/SPA)
- [.NET Dokumentation](https://learn.microsoft.com/de-de/dotnet/)
- [Tailwind CSS](https://tailwindcss.com/)

### 9.3 Lizenz

```text
MIT License

Copyright (c) 2025 Stephan Menzi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


