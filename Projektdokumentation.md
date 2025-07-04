# CampaignTool – Projektdokumentation

## 1. Einleitung

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

