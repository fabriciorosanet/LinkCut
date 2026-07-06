# LinkCut

Encurtador de URL construído com .NET 8 — Blazor Server (frontend) + Web API (backend) + SQLite.

## Stack

| Camada     | Tecnologia                              |
|------------|-----------------------------------------|
| Frontend   | .NET 8 Blazor Server (Interactive SSR)  |
| Backend    | ASP.NET Core 8 Web API                  |
| ORM        | Entity Framework Core 8                 |
| BD         | SQLite                                  |
| UI         | Bootstrap 5                             |
| Tempo real | SignalR (Blazor Server circuit)         |

## Estrutura

```
LinkCut.sln
├── LinkCut.Api/          # API REST
│   ├── Controllers/
│   │   ├── UrlsController.cs         # CRUD de URLs encurtadas
│   │   └── RedirectController.cs     # Redireciona short code → URL original
│   ├── Services/
│   │   └── UrlShorteningService.cs   # Gera códigos aleatórios (6 chars)
│   ├── Data/
│   │   └── AppDbContext.cs           # EF Core + SQLite
│   └── Models/
│       └── ShortenedUrl.cs           # Entidade
│
└── LinkCut.Web/          # Blazor Server
    ├── Components/
    │   ├── Pages/
    │   │   └── Home.razor            # Página única com o encurtador
    │   ├── Layout/
    │   │   ├── MainLayout.razor      # Layout principal
    │   │   └── NavMenu.razor         # Navegação lateral
    │   ├── App.razor                 # Entrypoint HTML
    │   └── Routes.razor              # Roteamento
    └── Services/
        └── ApiClient.cs              # HttpClient tipado p/ API
```

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Como rodar

### 1. API (backend)

```bash
cd LinkCut.Api
dotnet run
```

A API sobe em `http://localhost:5000`.  
Swagger disponível em `http://localhost:5000/swagger`.

### 2. Web (frontend)

Em outro terminal:

```bash
cd LinkCut.Web
dotnet run
```

O frontend sobe em `http://localhost:5263` e abre o navegador automaticamente.

> O Web chama a API via `ApiBaseUrl` configurada em `LinkCut.Web/appsettings.json`.  
> Padrão: `http://localhost:5000`.

## API Endpoints

| Método | Rota                    | Descrição                          |
|--------|------------------------|------------------------------------|
| POST   | `/api/urls`            | Criar URL encurtada                |
| GET    | `/api/urls`            | Listar todas as URLs (ordenadas)   |
| GET    | `/{shortCode}`         | Redirecionar p/ URL original       |

### POST `/api/urls`

```json
{ "originalUrl": "https://exemplo.com/muito/longa" }
```

Resposta:

```json
{
  "id": 1,
  "originalUrl": "https://exemplo.com/muito/longa",
  "shortCode": "aB3xYz",
  "shortUrl": "http://localhost:5000/aB3xYz",
  "createdAt": "2025-01-01T00:00:00Z",
  "clickCount": 0
}
```

## Funcionalidades

- Encurtar URLs com código de 6 caracteres alfanuméricos
- Listar URLs encurtadas (mais recentes primeiro)
- Clique em URL curta redireciona com contagem de cliques
- Copiar URL curta para área de transferência
- Auto-refresh da lista a cada 30s
- Validação de URL (requer http:// ou https://)

## Licença

MIT
