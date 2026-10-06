# DeskFlow API — Gestão de Chamados e Helpdesk de TI

## Sobre o projeto

A **DeskFlow API** é uma Web API RESTful construída em .NET 10, com Entity Framework Core e SQL Server, para organizar o suporte técnico de TI de uma empresa. Ela centraliza a abertura de chamados, controla o ciclo de vida do atendimento, guarda o histórico de interações da equipe de suporte e permite consultas com filtros.

Projeto Final Avaliativo do Módulo 01 — curso Desenvolvedor(a) Back-End .NET.

## Tecnologias utilizadas

- .NET 10 / ASP.NET Core Web API (com Controllers)
- Entity Framework Core 10 + SQL Server (Migrations e Data Annotations)
- Injeção de dependência nativa do ASP.NET Core
- Git e GitHub
- Extensão REST Client do VS Code (arquivo `DeskFlow.API.http`) para testar os endpoints
- IA para ajuda na correção de código, testes e arquitetura final do arquivo README.md de forma a otimizar tempo - necessário, pois tive um imprevisto durante a semana ao ter que formatar a máquina utilizada para o desenvolvimento do projeto -

## Arquitetura em camadas

O fluxo de uma requisição é:

```
Requisição HTTP → Middleware → Controller → Service → Repository → AppDbContext → SQL Server
```

| Camada | Pasta | Responsabilidade |
|---|---|---|
| Controllers | `Controllers/` | Recebem a requisição, definem rotas e status codes HTTP |
| Services | `Services/` | Regras de negócio e validações (transição de status, regras das interações etc.) |
| Repositories | `Repositories/` | Acesso a dados via EF Core |
| Models | `Models/` | Entidades (`Models/Entities`), exceção de regra de negócio (`Models/Exceptions`) e classes de entrada/saída |
| Data | `Data/` | `AppDbContext` e configuração da conexão com o SQL Server |
| Middlewares | `Middlewares/` | `ExceptionHandlingMiddleware`: tratamento global de erros |

Os Services e Repositories são acessados por interfaces (`IChamadoService`, `ICategoriaRepository` etc.) e registrados no `Program.cs` com `AddScoped`.

### Estrutura do repositório

```
deskflow-api/
├── scripts/
│   └── schema.sql
├── src/
│   └── DeskFlow.API/
│       ├── Controllers/
│       ├── Data/
│       ├── Middlewares/
│       ├── Migrations/
│       ├── Models/
│       │   ├── Entities/
│       │   └── Exceptions/
│       ├── Repositories/
│       ├── Services/
│       ├── DeskFlow.API.http
│       ├── Program.cs
│       └── appsettings.json
├── .gitignore
└── README.md
```

## Banco de dados

```
Categorias 1 ──── N Chamados 1 ──── N Interacoes
```

| Tabela | Colunas principais |
|---|---|
| `Categorias` | `Id`, `Nome` |
| `Chamados` | `Id`, `Titulo`, `Descricao`, `Prioridade`, `Status`, `SolicitanteNome`, `DataAbertura`, `DataFechamento`, `Solucao`, `CategoriaId` (FK) |
| `Interacoes` | `Id`, `ChamadoId` (FK), `Autor`, `Mensagem`, `DataRegistro` |

A estrutura é criada e evoluída por **EF Core Migrations** (pasta `Migrations/`). O script `scripts/schema.sql` é gerado a partir delas.

## Como executar

### Pré-requisitos

- .NET SDK 10
- SQL Server em execução (os exemplos usam a instância `SQLEXPRESS`)
- Ferramenta do EF Core: `dotnet tool install --global dotnet-ef`

### Passo a passo

1. Clone o repositório:
   ```bash
   git clone <url-do-repositorio>
   ```

2. Entre na pasta do projeto:
   ```bash
   cd deskflow-api/src/DeskFlow.API
   ```

3. Confira a connection string em `appsettings.json` e ajuste o `Server` se a sua instância for diferente:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. Crie o banco e as tabelas. Escolha **uma** das opções:

   - **Opção A — Migrations (recomendada):**
     ```bash
     dotnet ef database update
     ```
   - **Opção B — Script SQL:** execute o arquivo `scripts/schema.sql` no SSMS ou na extensão SQL Server do VS Code. Ele cria o banco `DeskFlowDb` e todas as tabelas.

5. Execute a API:
   ```bash
   dotnet run
   ```

6. A API sobe em `http://localhost:5152` (porta definida em `Properties/launchSettings.json`). Para testar, abra o arquivo `DeskFlow.API.http` no VS Code (com a extensão REST Client) e clique em **Send Request** em cada bloco.

> O projeto não usa Swagger. Os endpoints são testados pelo arquivo `DeskFlow.API.http`.

## Ciclo de vida do chamado

```
Aberto  →  EmAndamento  →  Fechado
```

- **Aberto:** chamado registrado pelo solicitante.
- **EmAndamento:** suporte em atendimento.
- **Fechado:** chamado encerrado, com texto de solução e data de fechamento.

## Regras de negócio

- **Abrir chamado:** o `Status` é sempre `Aberto` e a `DataAbertura` é a data/hora atual. Valores enviados pelo cliente para esses campos são ignorados.
- **Iniciar atendimento:** só é permitido para chamados `Aberto`.
- **Encerrar chamado:** só é permitido para chamados `EmAndamento`. A `Solucao` é obrigatória; a `DataFechamento` é gravada automaticamente.
- **Adicionar interação:** só é permitido se o chamado existir e **não** estiver `Fechado`.
- **Remover categoria:** bloqueado se houver chamados vinculados a ela.
- **Listagem de chamados:** os filtros `status`, `prioridade` e `categoriaId` são opcionais e podem ser combinados.

## Endpoints

### Categorias

| Método | Rota | Descrição | Respostas |
|---|---|---|---|
| POST | `/api/categorias` | Cadastra categoria | 201 |
| GET | `/api/categorias` | Lista categorias | 200 |
| GET | `/api/categorias/{id}` | Busca categoria por id | 200, 404 |
| PUT | `/api/categorias/{id}` | Altera o nome | 204, 404 |
| DELETE | `/api/categorias/{id}` | Remove categoria | 204, 400 (inexistente ou com chamados vinculados) |

### Chamados

| Método | Rota | Descrição | Respostas |
|---|---|---|---|
| POST | `/api/chamados` | Abre um chamado | 201 |
| GET | `/api/chamados?status=&prioridade=&categoriaId=` | Lista com filtros | 200 |
| GET | `/api/chamados/{id}` | Detalhes com categoria e interações | 200, 404 |
| POST | `/api/chamados/{id}/iniciar` | Inicia o atendimento | 200, 400, 404 |
| POST | `/api/chamados/{id}/encerrar` | Encerra o chamado | 200, 400, 404 |
| POST | `/api/chamados/{id}/interacoes` | Adiciona interação | 201, 400, 404 |

## Exemplos de utilização

**Abrir chamado** — `POST /api/chamados`

```json
{
  "titulo": "Computador nao liga",
  "descricao": "O computador da sala 3 nao liga desde ontem",
  "prioridade": "Alta",
  "solicitanteNome": "Maria Souza",
  "categoriaId": 1
}
```

Resposta (`201 Created`): o chamado criado, com `"status": "Aberto"` e `dataAbertura` preenchida.

**Encerrar chamado** — `POST /api/chamados/1/encerrar`

```json
{
  "solucao": "Fonte de alimentacao substituida"
}
```

**Adicionar interação** — `POST /api/chamados/1/interacoes`

```json
{
  "autor": "Tecnico Joao",
  "mensagem": "Iniciando diagnostico do equipamento"
}
```

**Filtros combinados** — `GET /api/chamados?status=Aberto&prioridade=Alta&categoriaId=1`

## Tratamento de erros

O `ExceptionHandlingMiddleware` captura as exceções não tratadas e devolve JSON padronizado, sem expor stack trace:

- Violação de regra de negócio (`RegraNegocioException`) → `400 Bad Request`
- Qualquer outro erro → `500 Internal Server Error`

```json
{
  "error": "Somente chamados com status Aberto podem ser iniciados."
}
```

Dados de entrada inválidos (por exemplo, `solucao` vazia) retornam `400` com os detalhes da validação.

## Versionamento

Cada funcionalidade foi desenvolvida em uma branch própria (`feature/...`) e incorporada à `master` por merge. Os commits seguem o padrão `feat:`, `fix:`, `refactor:` e `docs:`.

## Possíveis melhorias

- Validar `Prioridade` (Baixa, Media, Alta) e `Status` com enum; hoje são texto livre.
- Verificar se a categoria existe antes de abrir o chamado; hoje uma `categoriaId` inexistente resulta em erro 500.
- Retornar `404` ao remover categoria inexistente e padronizar os erros das rotas de categoria no mesmo JSON do middleware.
- Autenticação com ASP.NET Core Identity e JWT (RNF05, opcional).
- Paginação nas listagens e testes automatizados.