# FinanceTrackerCM

Aplicacao de controle financeiro pessoal com API RESTful em .NET 9 e frontend React/Vite. O projeto usa Clean Architecture, CQRS com MediatR, autenticacao JWT, refresh token em cookie HttpOnly e auditoria automatica com o pacote NuGet `AuditLogCM`.

## Stack

- .NET 9 / ASP.NET Core Web API
- Entity Framework Core 9 + SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- MediatR
- AuditLogCM / AuditLogCM.EFCore
- QuestPDF e ClosedXML para relatorios
- React 18 + Vite + Chart.js
- Docker Compose com SQL Server

## Estrutura

```text
FinanceTrackerCM.API             Controllers, autenticacao, DI e startup
FinanceTrackerCM.Application     Use cases, DTOs, interfaces e usuarios
FinanceTrackerCM.Domain          Entidades, enums e validadores
FinanceTrackerCM.Infrastructure  DbContext, migrations e servicos externos
FinanceTrackerCM.Client          Frontend React/Vite
```

## Funcionalidades

- Cadastro, login, refresh token e logout
- CRUD de contas, categorias e transacoes
- Isolamento multi-tenant por usuario autenticado
- Dashboard mensal com receitas, despesas, saldo e grafico
- Relatorios mensais em PDF e Excel
- Atualizacao automatica do saldo da conta ao criar, editar ou excluir transacoes
- Auditoria de operacoes de escrita via AuditLogCM

## Configuracao local

1. Copie o exemplo de ambiente:

```bash
copy FinanceTrackerCM.API\.env.example FinanceTrackerCM.API\.env
```

2. Preencha `JWT_KEY` e `Jwt__Key` no `.env` com uma chave longa.

3. Ajuste a connection string em `FinanceTrackerCM.API/appsettings.json`, se necessario.

4. Aplique as migrations:

```bash
dotnet ef database update --project FinanceTrackerCM.Infrastructure --startup-project FinanceTrackerCM.API --context AppDbContext
```

5. Execute a API:

```bash
dotnet run --project FinanceTrackerCM.API
```

6. Execute o frontend:

```bash
cd FinanceTrackerCM.Client
npm install
npm run dev
```

Por padrao, o frontend roda em `http://localhost:5174` e a API em `http://localhost:5062`.

## Docker

O `docker-compose.yml` sobe SQL Server e a API. A API recebe a connection string, configuracoes JWT e `Database__ApplyMigrations=true`, entao aplica as migrations no startup do container.

```bash
docker compose up --build
```

A API fica exposta em:

```text
http://localhost:5000
```

## Endpoints principais

### Auth

| Metodo | Rota | Descricao |
| --- | --- | --- |
| POST | `/api/auth/register` | Cadastra usuario |
| POST | `/api/auth/login` | Autentica e cria refresh token |
| POST | `/api/auth/refresh` | Renova access token |
| POST | `/api/auth/logout` | Revoga refresh token e limpa cookie |

### Contas

| Metodo | Rota | Descricao |
| --- | --- | --- |
| POST | `/api/contas` | Cria conta |
| GET | `/api/contas` | Lista contas |
| GET | `/api/contas/{id}` | Busca conta |
| PUT | `/api/contas/{id}` | Atualiza conta |
| DELETE | `/api/contas/{id}` | Exclui conta |

### Categorias

| Metodo | Rota | Descricao |
| --- | --- | --- |
| POST | `/api/categorias` | Cria categoria |
| GET | `/api/categorias` | Lista categorias |
| PUT | `/api/categorias/{id}` | Atualiza categoria |
| DELETE | `/api/categorias/{id}` | Exclui categoria |

### Transacoes

| Metodo | Rota | Descricao |
| --- | --- | --- |
| POST | `/api/transacoes` | Cria transacao e atualiza saldo |
| GET | `/api/transacoes` | Lista transacoes |
| GET | `/api/transacoes/{id}` | Busca transacao |
| PUT | `/api/transacoes/{id}` | Atualiza transacao e recalcula saldo |
| DELETE | `/api/transacoes/{id}` | Exclui transacao e reverte saldo |

### Dashboard e relatorios

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/api/summary?month=6&year=2026` | Resumo mensal do tenant autenticado |
| GET | `/api/reports/monthly/pdf?month=6&year=2026` | Relatorio PDF do tenant autenticado |
| GET | `/api/reports/monthly/excel?month=6&year=2026` | Relatorio Excel do tenant autenticado |

Os endpoints de relatorio nao recebem `tenantId`; o tenant vem do JWT do usuario autenticado.

## Roadmap

- [x] Clean Architecture em 4 camadas
- [x] CQRS com MediatR
- [x] CRUD de contas, categorias e transacoes
- [x] Autenticacao JWT com refresh token
- [x] Isolamento multi-tenant
- [x] Dashboard React
- [x] Relatorios PDF e Excel
- [x] Docker Compose
- [ ] Testes automatizados
- [ ] GitHub Actions CI/CD completo

## Autor

Claudio Matheus - [@ClaudioMatheusDev](https://github.com/ClaudioMatheusDev)

Parte de uma trilogia de projetos pessoais:

- AuditLogCM - pacote NuGet de auditoria
- FinanceTrackerCM - controle financeiro
- JobSchedulerCM - sistema de agendamento de jobs

## Licenca

MIT
