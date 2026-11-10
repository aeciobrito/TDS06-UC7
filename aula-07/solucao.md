# Gabarito de Correção Guiada - Aula 07


## 1. Código Completo do Controlador Conectado ao Banco (`Controllers/ItensController.cs`)

```csharp
using Microsoft.AspNetCore.Mvc;
using Hospedagem.API.Data;
using Hospedagem.API.Models;

namespace Hospedagem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItensController : ControllerBase
    {
        // 1. Injeção de dependência do DbContext
        private readonly HospedagemDbContext _context;

        public ItensController(HospedagemDbContext context)
        {
            _context = context;
        }

        // 2. Consulta de todos os registros persistidos no banco
        // Rota: GET /api/itens
        [HttpGet]
        public IActionResult ObterTodos()
        {
            // Traduzido para: SELECT Id, Nome, Preco, Descricao FROM Itens
            var itens = _context.Itens.ToList();
            return Ok(itens);
        }

        // 3. Consulta de item por chave primária
        // Rota: GET /api/itens/1
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // O método Find busca pela PK no banco
            var item = _context.Itens.Find(id);
            if (item == null)
            {
                return NotFound(new { erro = $"Item de consumo com ID {id} nao localizado no banco de dados." });
            }

            return Ok(item);
        }

        // 4. Inserção de novo registro no banco de dados
        // Rota: POST /api/itens
        [HttpPost]
        public IActionResult Criar([FromBody] Item novoItem)
        {
            if (novoItem == null)
            {
                return BadRequest(new { erro = "Dados do item nao informados." });
            }

            if (string.IsNullOrWhiteSpace(novoItem.Nome))
            {
                return BadRequest(new { erro = "O nome do item e obrigatorio." });
            }

            if (novoItem.Preco <= 0)
            {
                return BadRequest(new { erro = "O preco unitario deve ser maior que zero." });
            }

            // O ID NÃO é mais calculado manualmente em código!
            // O próprio SGBD gerará o autoincremento (IDENTITY)
            _context.Itens.Add(novoItem);

            // Grava fisicamente no disco/banco de dados
            _context.SaveChanges();

            // Ao retornar, novoItem.Id já contém o valor gerado pelo banco
            return CreatedAtAction(nameof(ObterPorId), new { id = novoItem.Id }, novoItem);
        }
    }
}
```

---

## 2. Testes de Validação Realizados às 22:00

1. Consulta com Banco Vazio:
   - `GET /api/itens`
   - Retorno: `[]` (HTTP 200 OK).

2. Cadastro de Itens (POST):
   - Item 1:
     ```json
     { "nome": "Agua Mineral 500ml", "preco": 5.00, "descricao": "Sem gas" }
     ```
   - Item 2:
     ```json
     { "nome": "Suco de Uva Integral", "preco": 12.00, "descricao": "Garrafa de vidro 300ml" }
     ```
   - Item 3:
     ```json
     { "nome": "Castanha de Caju", "preco": 16.50, "descricao": "Pacote 100g torrada" }
     ```

3. Consulta no Banco de Dados:
   Execute no SSMS ou no SQLite:
   ```sql
   SELECT Id, Nome, Preco, Descricao FROM Itens;
   ```
   Valide que os IDs 1, 2 e 3 foram gerados de forma sequencial pelo banco.

