## 1. Fundamentação Teórica Direta ao Ponto

### O que é um ORM (Object-Relational Mapping)?
Em sistemas legados, para salvar um objeto C# no banco de dados, o programador precisava abrir uma conexão manual via ADO.NET, escrever comandos SQL textuais (`INSERT INTO...`), iterar ponteiros (`SqlDataReader`) e converter tipos manualmente.
Um ORM como o **Entity Framework Core (EF Core)** mapeia automaticamente classes C# (Models) em tabelas relacionais e propriedades em colunas. As consultas são escritas em LINQ nativo, tornando o código mais seguro, tipado e livre de SQL Injection.

### Abordagem Code-First
No Senac e nos projetos de referência oficiais, adotamos o padrão **Code-First**:
1. O desenvolvedor escreve as classes C# (Models).
2. O EF Core analisa essas classes e gera os scripts de banco automaticamente através de **Migrations**.
3. O banco de dados evolui a partir do código versionado no repositório Git.

### O Papel do `DbContext`
O `DbContext` representa a sessão de comunicação com o banco de dados. Ele:
- Expõe conjuntos de entidades via `DbSet<TEntidade>` (onde cada `DbSet` corresponde a uma tabela).
- Rastreia alterações feitas nos objetos.
- Coordena as operações de leitura, gravação e transações atômicas no banco.

---

## 2. Demonstração Prática Passo a Passo (Projeto ControleEstoque)

### Passo 1: Instalação dos Pacotes NuGet via Terminal
No terminal, dentro da pasta `ControleEstoque.API/`, execute:

```bash
# Provedor do SQL Server (ou Microsoft.EntityFrameworkCore.Sqlite para execução local leve)
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# Pacotes para ferramentas de design e migrações
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Passo 2: Configuração da String de Conexão no `appsettings.json`
Abra o arquivo `appsettings.json` e adicione a seção `ConnectionStrings`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ControleEstoqueDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Nota Didática: Caso a máquina do laboratório utilize SQLite para zero dependência local, a string de conexão é simplificada para:
> `"DefaultConnection": "Data Source=controle_estoque.db"`
> (e o pacote instalado seria `Microsoft.EntityFrameworkCore.Sqlite`).

### Passo 3: Criação da Pasta `Data` e da Classe `ControleEstoqueDbContext.cs`
Crie a pasta `Data/` e dentro dela o arquivo `ControleEstoqueDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using ControleEstoque.API.Models;

namespace ControleEstoque.API.Data
{
    public class ControleEstoqueDbContext : DbContext
    {
        // Construtor que recebe as opções configuradas (provedor, connection string)
        public ControleEstoqueDbContext(DbContextOptions<ControleEstoqueDbContext> options)
            : base(options)
        {
        }

        // Tabela de Formas de Pagamento mapeada no banco
        public DbSet<FormaPagamento> FormasPagamento { get; set; }
    }
}
```

### Passo 3: Registro do DbContext no `Program.cs`
Abra o arquivo `Program.cs` e registre o serviço de banco de dados antes da linha `var app = builder.Build();`:

```csharp
using Microsoft.EntityFrameworkCore;
using ControleEstoque.API.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Obter a string de conexão do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registrar o DbContext no container de Injeção de Dependência
builder.Services.AddDbContext<ControleEstoqueDbContext>(options =>
    options.UseSqlServer(connectionString));

// Se estiver usando SQLite:
// builder.Services.AddDbContext<ControleEstoqueDbContext>(options =>
//     options.UseSqlite(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Passo 5: Teste de Compilação
Execute no terminal:
```bash
dotnet build
```
Certifique-se de que a mensagem `Build succeeded. 0 Error(s)` seja exibida.
