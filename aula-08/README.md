# Aula 08: CRUD Síncrono Completo com EF Core


## 1. Fundamentação Teórica Direta ao Ponto

### Como o EF Core Atualiza Registros no Banco?
Quando buscamos uma entidade pelo método `_context.Entidades.Find(id)`, o EF Core carrega o objeto do banco para a memória e passa a monitorá-lo (estado `Unchanged`).
Quando alteramos uma de suas propriedades (ex: `item.Nome = "Novo Nome"`), o rastreador de mudanças (Change Tracker) detecta a modificação e altera o estado da entidade para `Modified`.
Ao invocar `_context.SaveChanges()`, o EF Core compara os valores originais com os novos e gera um comando SQL otimizado:
```sql
UPDATE [Tabela] SET [Nome] = @p0 WHERE [Id] = @p1;
```

### Como o EF Core Exclui Registros no Banco?
Para excluir uma linha física do banco relacional:
1. Localizamos a entidade pelo ID (`Find(id)`).
2. Se não existir, retornamos `404 NotFound` imediatamente (evitando tentar deletar o que não existe).
3. Se existir, marcamos a entidade para remoção via `_context.Entidades.Remove(item)` (estado `Deleted`).
4. Chamamos `_context.SaveChanges()`, momento em que o EF Core gera:
```sql
DELETE FROM [Tabela] WHERE [Id] = @p0;
```

---

## 2. Demonstração Prática Passo a Passo (Projeto ControleEstoque)

Abra o arquivo `Controllers/FormasPagamentoController.cs` e implemente os métodos de PUT e DELETE com o Entity Framework Core:

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
        private readonly ControleEstoqueDbContext _context;

        public FormasPagamentoController(ControleEstoqueDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ObterTodas()
        {
            var lista = _context.FormasPagamento.ToList();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var item = _context.FormasPagamento.Find(id);
            if (item == null)
            {
                return NotFound(new { erro = $"Forma de pagamento com ID {id} nao encontrada." });
            }

            return Ok(item);
        }

        [HttpPost]
        public IActionResult Criar([FromBody] FormaPagamento novaForma)
        {
            if (novaForma == null || string.IsNullOrWhiteSpace(novaForma.Nome))
            {
                return BadRequest(new { erro = "O nome da forma de pagamento e obrigatorio." });
            }

            _context.FormasPagamento.Add(novaForma);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = novaForma.Id }, novaForma);
        }

        // 4. Atualizar registro existente no banco (PUT)
        // Rota: PUT /api/formaspagamento/1
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] FormaPagamento formaAtualizada)
        {
            if (formaAtualizada == null)
            {
                return BadRequest(new { erro = "Dados nao informados." });
            }

            if (id != formaAtualizada.Id)
            {
                return BadRequest(new { erro = "O ID da rota difere do ID do objeto fornecido." });
            }

            // Busca o registro atual diretamente no banco
            var itemNoBanco = _context.FormasPagamento.Find(id);
            if (itemNoBanco == null)
            {
                return NotFound(new { erro = $"Forma de pagamento com ID {id} nao encontrada para atualizacao." });
            }

            if (string.IsNullOrWhiteSpace(formaAtualizada.Nome))
            {
                return BadRequest(new { erro = "O nome nao pode ficar em branco." });
            }

            // Atualiza as propriedades rastreadas
            itemNoBanco.Nome = formaAtualizada.Nome;
            itemNoBanco.Ativo = formaAtualizada.Ativo;

            // Envia o comando UPDATE para o banco de dados
            _context.SaveChanges();

            return NoContent();
        }

        // 5. Excluir registro existente no banco (DELETE)
        // Rota: DELETE /api/formaspagamento/1
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var itemNoBanco = _context.FormasPagamento.Find(id);
            if (itemNoBanco == null)
            {
                return NotFound(new { erro = $"Forma de pagamento com ID {id} nao encontrada para exclusao." });
            }

            _context.FormasPagamento.Remove(itemNoBanco);

            // Envia o comando DELETE para o banco de dados
            _context.SaveChanges();

            return NoContent();
        }
    }
}
```

### Demonstração dos Testes no Swagger:
1. Executar `PUT /api/formaspagamento/1` alterando "Dinheiro" para "Dinheiro em Especie".
   - Demonstrar retorno HTTP 204 NoContent.
   - Executar `SELECT * FROM FormasPagamento;` no SSMS/SQLite e mostrar o texto alterado.
2. Executar `DELETE /api/formaspagamento/2` (Pix).
   - Demonstrar retorno HTTP 204 NoContent.
   - Executar `SELECT * FROM FormasPagamento;` e comprovar a exclusão da linha.
3. Tentar executar `DELETE /api/formaspagamento/99` e demonstrar a resposta semântica `404 NotFound`.
