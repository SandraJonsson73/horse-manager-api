# Horse Manager API

ASP.NET Core Web API för registrering och hantering av hästar.

## Teknik

- **Framework**: ASP.NET Core Web API (.NET 10)
- **Databas**: SQLite via Entity Framework Core
- **Arkitektur**: Controller-baserad REST API

### Val av teknik

- **SQLite**: Filbaserad databas som kräver ingen separat server. Data sparas automatiskt i `horses.db` och kvarstår mellan omstarter. Perfekt för lokal utveckling och testning.
- **EF Core**: ORM som hanterar databasen automatiskt via `DbContext`. `EnsureCreated()` vid startup säkerställer att databasen finns utan att användaren behöver köra separata migrations.
- **CORS konfigurerad**: Frontend på `http://localhost:5173` kan anropa API:et under utveckling.

## Installation och start

### Krav
- .NET 10 SDK eller senare
- PowerShell/Terminal

### Steg

1. **Klona repot** (om du ännu inte gjort det):
   ```powershell
   https://github.com/SandraJonsson73/horse-manager-api.git
   cd horse-manager-api
    ```

2. **Installera dependencies:**
    ```powershell 
    dotnet restore
    ```

3. **Starta API:et:**
    ```powershell
    dotnet run
    ```

4. **Verifiera att det fungerar:**
API lyssnar på `http://localhost:5280` (eller annan port om 5280 är upptagen)
Gå till `http://localhost:5280/api/horses` i webbläsaren
Du bör få en tom JSON-array `[]`

5. **Databasen skapas automatiskt** som `horses.db` i projektets rot

## API-endpoints
**Hästar**
  - `GET /api/horses` — Hämta alla hästar
  - `GET /api/horses/{id}` — Hämta en specifik häst
  - `POST /api/horses` — Skapa en ny häst
    - Body (JSON):` { "name": "...", "breed": "...", "birthYear": 2020, "owner": "..." }`
  - `PUT /api/horses/{id}` — Uppdatera en häst
    - Body (JSON):` { "name": "...", "breed": "...", "birthYear": 2020, "owner": "..." }`
  - `DELETE /api/horses/{id}` — Ta bort en häst

**Filuppladdning**
  - `POST /api/horses/{id}/image` — Ladda upp bild för en häst
    - Accepterar: JPG, PNG, GIF (max 5 MB)
    - Bilden sparas i `wwwroot/uploads/` och returnerar `imagePath`

## Struktur
`horse-manager-api/
├── Models/
│   └── Horse.cs           # Häst-datamodell
├── Data/
│   └── HorseDbContext.cs  # EF Core DbContext
├── Controllers/
│   └── HorsesController.cs # REST endpoints
├── wwwroot/
│   └── uploads/           # Uppladdade bilder (skapas automatiskt)
├── Program.cs             # Applikationskonfiguration
├── appsettings.json       # Databaskonfiguration
└── README.md`

## Felhantering
Alla endpoints returnerar lämpliga HTTP-statuskoder:

  - `200` OK — Framgångsrikt GET/PUT
  - `201` Created — Framgångsrikt POST
  - `204` No Content — Framgångsrikt DELETE
  - `400` Bad Request — Ogiltig inmatning
  - `404` Not Found — Resurs hittades inte

Frontend kan använda dessa för att visa relevanta felmeddelanden.

## Utveckling
För att stoppa API:et: `Ctrl+C` i terminalen

Databasen (`horses.db`) sparas lokalt och kan tas bort för att starta med en tom databas.