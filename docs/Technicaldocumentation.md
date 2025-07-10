# CampaignTool – Technischer Anhang

## 1. Systemarchitektur

Die Anwendung folgt einer modernen Drei-Schichten-Architektur:

- **Frontend**: Blazor WebAssembly (alternativ: React möglich)
- **Backend**: ASP.NET Core Web API mit C#
- **Datenbank**: PostgreSQL mit Entity Framework Core

Kommunikation erfolgt per REST, Authentifizierung über Token. Die Anwendung ist als SPA aufgebaut und nutzt Containerisierung (Docker, optional) für flexible Deployment-Möglichkeiten.

**Architekturdiagramm (optional):**
→ [Hier Diagramm einfügen, z. B. als Bild: „Abb. A – Systemübersicht“]

## 2. Datenbankmodell

Die zentrale Datenhaltung erfolgt in einer relationalen PostgreSQL-Datenbank.

### 2.1 Hauptentitäten

| Tabelle         | Beschreibung                                    |
|----------------|--------------------------------------------------|
| `Campaign`      | Kampagneninfos: Titel, Beschreibung, Owner      |
| `Entity`        | SC, NSC, Organisation, Ereignis, Item           |
| `User`          | Registrierte Benutzer mit Rollen                |
| `Connection`    | Beziehung zwischen zwei Entitäten               |
| `Tag`           | Kategorisierung von Entitäten                   |
| `Session`       | Optional: Spielsitzungen einer Kampagne         |
| `HistoryLog`    | Chronologie wichtiger Änderungen                |

### 2.2 Beziehungen

- `Entity` ↔ `Tag`: n:n über `EntityTag`
- `Entity` ↔ `Entity`: n:n über `Connection`
- `User` ↔ `Campaign`: n:n über `CampaignAccess`
- `Entity` ↔ `Campaign`: 1:n

**ER-Diagramm (optional):**
→ [„Abb. B – ER-Modell“ einfügen]

## 3. API-Endpunkte

Die REST-API ist nach Ressourcentypen strukturiert und ermöglicht CRUD-Operationen.

### 3.1 Charaktere

- `GET /character/bycampaign/{campaignId}`
- `POST /character/create`
- `PUT /character/update/{characterId}`
- `DELETE /character/delete/{characterId}`

### 3.2 Verbindungen

- `POST /connection/create`
- `GET /connection/byentity/{entityId}`
- `DELETE /connection/remove/{connectionId}`

### 3.3 Kampagnen

- `GET /campaign/list`
- `POST /campaign/create`
- `PUT /campaign/update/{id}`

### 3.4 Benutzer

- `POST /user/login`
- `GET /user/bycampaign/{id}`
- `PUT /user/update/{id}`
- `POST /user/assignrole`

### 3.5 Zusatzfunktionen

- `GET /tag/list`
- `POST /tag/create`
- `GET /session/bycampaign/{id}`

**Authentifizierung:** Alle Endpunkte (bis auf Login und Beispielzugang) nutzen Token-basierte Absicherung (JWT), Berechtigungen werden über Rollen geprüft.

---

✨ Dieses Dokument kannst du flexibel erweitern – etwa mit Beispiel-Payloads, Response-Formaten oder Integrationshinweisen (Swagger etc.). Wenn du möchtest, helfe ich dir gern, ein passendes **Architekturdiagramm oder ER-Modell als Bild zu gestalten**, oder bereite daraus eine lesbare API-Referenz im GitHub-Wiki.

Wie möchtest du weitermachen – Diagramme visualisieren, Endpunktdetails ausformulieren oder ins Repo integrieren? 😊
