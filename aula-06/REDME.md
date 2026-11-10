# Aula 06: Gerenciamento de Migrações (Migrations) com Entity Framework Core

- Ambiente: Windows 11, .NET SDK 8.0+, Visual Studio 2022 ou VS Code com C# Dev Kit, SGBD (SQL Server LocalDB ou SQLite).
- Indicadores: IND 1 (Configuração de ambiente) e IND 3 (Conexão e migração de banco de dados).
- Projeto de Atividade dos Alunos: Hospedagem.

## 2. Fundamentação Teórica Direta ao Ponto

### Por que usar Migrações?
Em projetos corporativos, o banco de dados nunca é criado ou alterado manualmente via comandos soltos no SGBD em produção.
As **Migrations** do Entity Framework Core atuam como um sistema de controle de versão (semelhante ao Git) para a estrutura do banco de dados:
1. Sempre que uma Model é criada ou alterada, o EF Core compara o modelo C# com a última foto (Snapshot) do banco.
2. É gerado um arquivo C# na pasta `Migrations/` contendo dois métodos essenciais:
   - `Up()`: Instruções para aplicar as novas alterações (ex: `CreateTable`, `AddColumn`).
   - `Down()`: Instruções para desfazer (rollback) a alteração caso necessário (ex: `DropTable`, `DropColumn`).
3. Ao rodar `dotnet ef database update`, o EF Core consulta a tabela interna `__EFMigrationsHistory` no banco. As migrações ainda não registradas são executadas dentro de uma transação.

---

## 3. Demonstração Prática Passo a Passo (Projeto ControleEstoque)

### Passo 1: Instalação da Ferramenta Global `dotnet-ef`
Caso a ferramenta de linha de comando do EF Core ainda não esteja instalada na máquina, execute no terminal:

```bash
dotnet tool install --global dotnet-ef
```
Se já estiver instalada, atualize:
```bash
dotnet tool update --global dotnet-ef
```

### Passo 2: Criação da Primeira Migração
Dentro da pasta `ControleEstoque.API/`, execute:

```bash
dotnet ef migrations add CriarTabelaFormasPagamento
```

Observe o resultado:
- Uma nova pasta `Migrations/` foi criada no projeto.
- Um arquivo com timestamp (ex: `20261026_CriarTabelaFormasPagamento.cs`) foi gerado.
- Abra o arquivo gerado e mostre para os alunos os métodos `Up` e `Down`:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "FormasPagamento",
        columns: table => new
        {
            Id = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
            Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Ativo = table.Column<bool>(type: "bit", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_FormasPagamento", x => x.Id);
        });
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropTable(
        name: "FormasPagamento");
}
```

### Passo 3: Aplicação da Migração no Banco de Dados
Execute o comando:

```bash
dotnet ef database update
```

O EF Core conectará ao banco de dados configurado na Connection String, criará o banco caso ainda não exista, criará a tabela de histórico `__EFMigrationsHistory` e executará o DDL da tabela `FormasPagamento`.

### Passo 4: Inspeção no SGBD
1. Abra o SQL Server Management Studio (SSMS) conectando em `(localdb)\MSSQLLocalDB` (ou a extensão SQLite no VS Code caso use SQLite).
2. Expanda o banco `ControleEstoqueDB` -> `Tabelas`.
3. Demonstre aos alunos as tabelas `dbo.__EFMigrationsHistory` e `dbo.FormasPagamento`.
4. Mostre que a coluna `Id` foi configurada automaticamente como chave primária e autoincremento (`IDENTITY`).
