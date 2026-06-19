# FinanceTrackerCM

API RESTful de controle financeiro pessoal construída com **Clean Architecture**, **CQRS** e integração com o pacote NuGet [AuditLogCM](https://www.nuget.org/packages/AuditLogCM) — desenvolvido pelo mesmo autor para auditoria automática de operações no banco de dados.

---

## Sobre o projeto

O FinanceTrackerCM permite que usuários gerenciem suas finanças pessoais com controle de contas, categorias e transações. Toda operação de escrita é automaticamente auditada pelo pacote `AuditLogCM`, registrando quem alterou, o quê, quando e como estava antes.

---

## Tecnologias

- .NET 9 / C#
- ASP.NET Core Web API
- Entity Framework Core 9 + SQL Server
- MediatR (CQRS)
- JWT Bearer Authentication
- AuditLogCM (pacote NuGet próprio)
- Docker (em breve)
- GitHub Actions CI/CD

---

## Arquitetura

O projeto segue os princípios de **Clean Architecture**, com separação clara de responsabilidades entre as camadas:

```
FinanceTrackerCM.Domain          → Entidades, Enums e regras de negócio
FinanceTrackerCM.Application     → Use Cases (Commands/Queries), Interfaces, DTOs
FinanceTrackerCM.Infrastructure  → DbContext, Repositórios, Serviços externos
FinanceTrackerCM.API             → Controllers, Middleware, Program.cs
```

O fluxo de dependência respeita a regra da Clean Architecture:

```
API → Application → Domain
Infrastructure → Application
Infrastructure → Domain
```

---

## Funcionalidades

- Gerenciamento de contas financeiras (Nubank, Carteira, Poupança...)
- Categorização de transações (Alimentação, Transporte, Salário...)
- Registro de transações com tipo (Receita/Despesa) e status
- Auditoria automática de todas as operações via AuditLogCM
- Autenticação e autorização com JWT (em desenvolvimento)
- Padrão CQRS com MediatR para separação de leitura e escrita

---

## Estrutura de pastas

```
FinanceTrackerCM/
├── FinanceTrackerCM.Domain/
│   ├── Entities/
│   │   ├── Conta.cs
│   │   ├── Categoria.cs
│   │   └── Transacao.cs
│   └── Enums/
│       ├── TipoTransacao.cs
│       ├── StatusConta.cs
│       └── StatusTransacao.cs
│
├── FinanceTrackerCM.Application/
│   ├── Interfaces/
│   │   └── IAppDbContext.cs
│   ├── DTOs/
│   └── UseCases/
│       ├── Contas/
│       ├── Categorias/
│       ├── Transacoes/
│       └── Users/
│
├── FinanceTrackerCM.Infrastructure/
│   ├── Context/
│   │   └── AppDbContext.cs
│   └── Services/
│       └── CurrentUserResolver.cs
│
└── FinanceTrackerCM.API/
    ├── Controllers/
    │   ├── ContasController.cs
    │   ├── CategoriasController.cs
    │   └── TransacoesController.cs
    ├── Middleware/
    └── Program.cs
```

---

## Como executar

### Pré-requisitos

- .NET 9 SDK
- SQL Server (local ou Docker)
- Visual Studio 2022+ ou VS Code

### Configuração

1. Clone o repositório:
```bash
git clone https://github.com/ClaudioMatheusDev/FinanceTrackerCM.git
cd FinanceTrackerCM
```

2. Configure a connection string no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FinanceTrackerCM;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

3. Aplique as migrations:
```bash
dotnet ef database update --project FinanceTrackerCM.Infrastructure --startup-project FinanceTrackerCM.API --context AppDbContext
```

4. Execute a API:
```bash
dotnet run --project FinanceTrackerCM.API
```

5. Acesse a documentação:
```
http://localhost:{porta}/openapi/v1.json
```

---

## Endpoints

### Contas
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/contas` | Criar nova conta |
| GET | `/api/contas` | Listar todas as contas |
| GET | `/api/contas/{id}` | Obter conta por ID |
| PUT | `/api/contas/{id}` | Atualizar conta |
| DELETE | `/api/contas/{id}` | Excluir conta |

### Categorias
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/categorias` | Criar nova categoria |
| GET | `/api/categorias` | Listar todas as categorias |
| GET | `/api/categorias/{id}` | Obter categoria por ID |
| PUT | `/api/categorias/{id}` | Atualizar categoria |
| DELETE | `/api/categorias/{id}` | Excluir categoria |

### Transações
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/transacoes` | Criar nova transação |
| GET | `/api/transacoes` | Listar todas as transações |
| GET | `/api/transacoes/{id}` | Obter transação por ID |
| PUT | `/api/transacoes/{id}` | Atualizar transação |
| DELETE | `/api/transacoes/{id}` | Excluir transação |

---

## Integração com AuditLogCM

O projeto consome o pacote NuGet [AuditLogCM.EFCore](https://www.nuget.org/packages/AuditLogCM.EFCore) para auditoria automática. Toda operação de `Create`, `Update` ou `Delete` no banco gera um registro na tabela `AuditEntries` com:

- Nome da entidade alterada
- Tipo da operação
- Valores anteriores e novos (JSON)
- ID e nome do usuário responsável
- Timestamp da operação

---

## Roadmap

- [x] Domain (Entidades e Enums)
- [x] Clean Architecture em 4 camadas
- [x] CQRS com MediatR
- [x] CRUD de Contas, Categorias e Transações
- [x] Integração com AuditLogCM
- [ ] Autenticação JWT
- [ ] Relatórios em PDF com QuestPDF
- [ ] Exportação para Excel
- [ ] Docker + docker-compose
- [ ] GitHub Actions CI/CD completo

---

## Autor

**Claudio Matheus** — [@ClaudioMatheusDev](https://github.com/ClaudioMatheusDev)

Parte de uma trilogia de projetos pessoais:
- [AuditLogCM](https://github.com/ClaudioMatheusDev/AuditLogCM) — Pacote NuGet de auditoria ✅
- **FinanceTrackerCM** — API de controle financeiro (em desenvolvimento)
- JobSchedulerCM — Sistema de agendamento de jobs (em breve)

---

## Licença

MIT
