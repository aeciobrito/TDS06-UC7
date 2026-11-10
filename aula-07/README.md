# Aula 07: Primeiro CRUD com Acesso ao Banco de Dados no Controller

- Ambiente: Windows 11, .NET SDK 8.0+, Visual Studio 2022 ou VS Code com C# Dev Kit, SGBD com migrações aplicadas.
- Indicadores: IND 3 (Conexão com banco de dados) e IND 4 (Código de acesso a dados).
- Projeto Central de Demonstração (Professor): ControleEstoque.
- Projeto de Atividade dos Alunos: Hospedagem.

## 2. Fundamentação Teórica Direta ao Ponto

### Injeção de Dependência do `DbContext` no Controller
Como o `DbContext` foi registrado no `Program.cs` via `builder.Services.AddDbContext<...>()`, o ASP.NET Core gerencia automaticamente o seu ciclo de vida.
Basta declarar o `DbContext` como parâmetro no construtor do Controller. O framework injetará a instância configurada a cada requisição HTTP recebida e a descartará adequadamente ao término da requisição (ciclo de vida `Scoped`).

```csharp
[ApiController]
[Route("api/[controller]")]
public class MeuController : ControllerBase
{
    private readonly MeuDbContext _context;

    // O ASP.NET Core injeta a instância do DbContext aqui
    public MeuController(MeuDbContext context)
    {
        _context = context;
    }
}
```

### O Papel do `SaveChanges()`
Diferente de uma lista em memória onde `_lista.Add(item)` é imediato, no Entity Framework Core o método `_context.Tabela.Add(item)` apenas sinaliza ao rastreador de mudanças (Change Tracker) que aquele objeto deve ser inserido.
A gravação física no banco de dados e a geração do comando SQL `INSERT INTO...` só ocorrem quando o método `_context.SaveChanges()` é executado.
Após o `SaveChanges()`, o banco gera a chave primária automática e o EF Core atualiza a propriedade `Id` do objeto C# imediatamente.

---

## 3. Demonstração Prática Passo a Passo (Projeto ControleEstoque)

Abra o arquivo `Controllers/FormasPagamentoController.cs` e refatore-o, substituindo a lista estática pelo `ControleEstoqueDbContext`:

```csharp
using Microsoft.AspNetCore.Mvc;
using ControleEstoque.API.Data;
using ControleEstoque.API.Models;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormasPagamentoController : ControllerBase
    {
        // 1. Campo privado somente leitura para armazenar a referência do DbContext
        private readonly ControleEstoqueDbContext _context;

        // 2. Injeção de dependência via construtor
        public FormasPagamentoController(ControleEstoqueDbContext context)
        {
            _context = context;
        }

        // 3. Obter todas as formas de pagamento do banco (GET)
        // Rota: GET /api/formaspagamento
        [HttpGet]
        public IActionResult ObterTodas()
        {
            // O EF Core executa: SELECT [Id], [Nome], [Ativo] FROM [FormasPagamento]
            var lista = _context.FormasPagamento.ToList();
            return Ok(lista);
        }

        // 4. Obter forma de pagamento por ID do banco (GET)
        // Rota: GET /api/formaspagamento/1
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // O método Find busca pela chave primária
            var item = _context.FormasPagamento.Find(id);
            if (item == null)
            {
                return NotFound(new { erro = $"Forma de pagamento com ID {id} nao encontrada no banco." });
            }

            return Ok(item);
        }

        // 5. Cadastrar nova forma de pagamento no banco (POST)
        // Rota: POST /api/formaspagamento
        [HttpPost]
        public IActionResult Criar([FromBody] FormaPagamento novaForma)
        {
            if (novaForma == null || string.IsNullOrWhiteSpace(novaForma.Nome))
            {
                return BadRequest(new { erro = "O nome da forma de pagamento e obrigatorio." });
            }

            // Marca a entidade para inserção
            _context.FormasPagamento.Add(novaForma);

            // Persiste fisicamente no banco de dados (executa INSERT INTO)
            _context.SaveChanges();

            // O novaForma.Id já estará preenchido com o IDENTITY gerado pelo banco
            return CreatedAtAction(nameof(ObterPorId), new { id = novaForma.Id }, novaForma);
        }
    }
}
```

### Demonstração dos Testes no Swagger e no SGBD:
1. Execute `dotnet run`.
2. Acesse `/swagger` e execute `GET /api/formaspagamento`. Observe que agora o retorno é uma lista vazia `[]` (pois o banco acabou de ser criado e não há registros).
3. Execute `POST /api/formaspagamento` cadastrando:
   ```json
   { "nome": "Dinheiro", "ativo": true }
   ```
4. Observe que o retorno traz `id: 1` gerado pelo SQL Server/SQLite.
5. Cadastre outro item:
   ```json
   { "nome": "Pix", "ativo": true }
   ```
6. Abra o SSMS (ou SQLite Viewer) e execute:
   ```sql
   SELECT * FROM FormasPagamento;
   ```
   Mostre aos alunos que as duas linhas estão gravadas fisicamente no disco.
7. Encerre o terminal da API (`Ctrl+C`) e execute `dotnet run` novamente.
8. Execute `GET /api/formaspagamento` e prove que, diferente da lista em memória, **os dados não foram perdidos**.
