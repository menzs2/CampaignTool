# CampaignTool – Design-Dokument

## 1. Einleitung
Das vorliegende Design-Dokument beschreibt die gestalterischen und benutzerbezogenen Aspekte der WebApp **CampaignTool**. Während die technische Dokumentation bereits Architektur, Datenmodell und API-Endpunkte abbildet, liegt hier der Fokus auf den Anforderungen an die Benutzeroberfläche, das visuelle Erscheinungsbild sowie die Nutzerführung.

## 2. Designprinzipien

### 2.1 Farbgestaltung
- Hintergrund: Hellgrau/Beige  
- Primärfarbe: Dunkelblau RGB (52,101,164) / #3465a4  
- Kontraste gemäß WCAG

### 2.2 Schriftwahl und Lesbarkeit
- Schriftfamilie: Helvetica, Arial, sans-serif  
- Skalierbare Schriftgrößen je nach Gerät

### 2.3 Formsprache & Layoutstruktur
- Grid-System, leichte Rundungen  
- Flaches Design, konsistente Margins/Paddings

### 2.4 Icons & Interaktion
- Line-Art Icons  
- Tooltip bei Hover  
- Sparsame Animationen

### 2.5 Responsives Verhalten
- Breakpoints für Desktop, Tablet, Mobile  
- Navigation passt sich Layout dynamisch an

### 2.6 Barrierefreiheit
- Tastatursteuerung  
- Screenreader-Unterstützung  
- Fokus-Hervorhebung

## 3. UX-Flows

### 3.1 Login & Kampagnenauswahl
- Login oder Gastzugang  
- Kampagne wählen oder Standard laden

### 3.2 Neue Kampagne erstellen
- SL wird automatisch Admin  
- Eingabe von Titel, Beschreibung, Tags

### 3.3 Entität erstellen & bearbeiten
- SC, NSC, Organisation etc. erfassen  
- Verbindungen definieren

### 3.4 Verbindungen
- Auswahl von Beziehungstyp  
- Verbindungsvorschau & Bearbeitung

### 3.5 Benutzerverwaltung
- Rollen zuweisen  
- Kampagnenzugriff konfigurieren

### 3.6 Gastzugang
- Nur Leserechte auf Beispielkampagne

## 4. Wireframes / Mockups

### 4.1 Login / Startseite
**Abb. 1** – Felder, Button, Hinweistext

### 4.2 Kampagnenübersicht
**Abb. 2** – Liste, Filter, „Neue Kampagne erstellen“

### 4.3 Entitätsansicht
**Abb. 3** – Name, Beschreibung, Beziehungen

### 4.4 Einstellungen
**Abb. 4** – Standardkampagne, Anzeigeoptionen

### 4.5 Adminbereich
**Abb. 5** – Benutzerliste, Rollenverwaltung

## 5. Komponenten-Katalog

- **Listenkomponente**  
- **Detailpanel**  
- **Modale Dialoge**  
- **Filterleisten**  
- **Navigation (Sidebar, Top-Bar)**  
- **Tag-Komponenten**  
- **Verbindungsvorschau**  
- **Notification-Elemente**

## 6. Benutzerrollen & Sichtbarkeit

| UI-Element                         | Spieler:in | SL | Admin | Gast |
|-----------------------------------|------------|----|-------|------|
| Kampagne erstellen                | ❌         | ✅ | ✅    | ❌   |
| GMOnly-Felder sehen               | ❌         | ✅ | ✅    | ❌   |
| Entität bearbeiten                | ✅ (eigene) | ✅ | ✅    | ❌   |
| Adminbereich nutzen               | ❌         | ❌ | ✅    | ❌   |
| Beispielkampagne ansehen          | ❌         | ❌ | ❌    | ✅   |

## 7. Erweiterbarkeit & Themes

### 7.1 Themes
- Dark Theme  
- Rollenspielsystem-spezifische Farbpaletten  
- Auswahl in den Einstellungen

### 7.2 Mehrsprachigkeit
- Ressourcen-Dateien  
- Umschaltung im Benutzerprofil

### 7.3 Modularität
- Komponentenkatalog  
- Erweiterbare Filter, Visualisierungen

### 7.4 Externe Inhalte
- Karten, Icons, Namensgeneratoren  
- PDF/JSON-Exportformate

## 8. Anhang

### 8.1 Ressourcen
- Tailwind, Heroicons, Material Icons  
- Wireframes erstellt mit Figma  
- Eigene Skizzen: Stephan Menzi, Juli 2025

### 8.2 Quellen
- WCAG Guidelines  
- MDN Accessibility Patterns  
- Blazor Komponenten Doku  
- MIT-Lizenz Infos

### 8.3 Versionsstand
- Version: 1.0  
- Erstellt: Juli 2025  
- Letzte Änderung: [TT.MM.JJJJ]
