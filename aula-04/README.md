## 1. Roteiro do Tempo (19:00 às 22:30)

| Horário | Etapa | Foco do Professor |
|---|---|---|
| 19:00 - 19:30 | Acolhimento e Revisão | Recepção, revisão de GET e POST com coleções em memória, esclarecimento de dúvidas sobre a lógica de autoincremento. |
| 19:30 - 20:00 | Exposição Prática | Fechamento do ciclo CRUD com os verbos PUT (atualização total/idempotente) e DELETE (exclusão de recurso); tabela de códigos de status HTTP semânticos (200, 201, 204, 400, 404). |
| 20:00 - 20:30 | Problema Guiado | Implementação dos métodos PUT e DELETE no `FormasPagamentoController` do ControleEstoque, com validação de ID da rota vs ID do body e resposta `NoContent()`. Testes no Swagger. |
| 20:30 - 20:45 | Intervalo | Pausa de 15 minutos para descanso. |
| 20:45 - 22:00 | Laboratório "Mão na Massa" | Alunos completam o CRUD no `ItensController` do projeto Hospedagem, implementando PUT e DELETE com validação e códigos HTTP adequados. |
| 22:00 - 22:30 | Correção Coletiva e Fechamento | Demonstração do gabarito em tela, consolidação do aprendizado de CRUD e anúncio da introdução ao Entity Framework Core na Aula 05. |

---

## 2. Fundamentação Teórica Direta ao Ponto

### A Matriz de Operações CRUD e Verbos HTTP

| Operação CRUD | Verbo HTTP | Rota Típica | Código HTTP de Sucesso | Código HTTP de Falha Comum |
|---|---|---|---|---|
| Create (Criar) | POST | `/api/recurso` | `201 Created` | `400 BadRequest` |
| Read (Ler Todos) | GET | `/api/recurso` | `200 OK` | `500 InternalServerError` |
| Read (Ler Um) | GET | `/api/recurso/{id}` | `200 OK` | `404 NotFound` |
| Update (Alterar) | PUT | `/api/recurso/{id}` | `204 NoContent` ou `200 OK` | `400 BadRequest` / `404 NotFound` |
| Delete (Excluir) | DELETE | `/api/recurso/{id}` | `204 NoContent` | `404 NotFound` |

### O Significado do Status `204 NoContent`
Ao atualizar ou excluir com sucesso um registro, o servidor completou a solicitação com êxito e não há necessidade de reenviar dados no corpo da resposta. O status `204 NoContent` informa ao cliente exatamente isso: operação bem-sucedida com carga útil vazia, economizando tráfego de rede.

### Validação de Rota vs Body no PUT
Em requisições PUT (`PUT /api/recurso/3`), é indispensável verificar se o ID recebido no caminho da URL coincide com o ID transportado no objeto do corpo. Se divergirem, responde-se imediatamente com `400 BadRequest` ("O ID da rota difere do ID do objeto informado").

---

## 3. Demonstração Prática Passo a Passo (Projeto ControleEstoque)

Abra o arquivo `Controllers/FormasPagamentoController.cs` e adicione os métodos de PUT e DELETE:

```csharp
using Microsoft.AspNetCore.Mvc;
using ControleEstoque.API.Models;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormasPagamentoController : ControllerBase
    {
        private static readonly List<FormaPagamento> _formasPagamento = new()
        {
            new FormaPagamento { Id = 1, Nome = "Dinheiro", Ativo = true },
            new FormaPagamento { Id = 2, Nome = "Cartao de Credito", Ativo = true },
            new FormaPagamento { Id = 3, Nome = "Pix", Ativo = true }
        };

        [HttpGet]
        public IActionResult ObterTodas()
        {
            return Ok(_formasPagamento);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var item = _formasPagamento.FirstOrDefault(f => f.Id == id);
            if (item == null)
            {
                return NotFound(new { mensagem = $"Forma de pagamento com ID {id} nao encontrada." });
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

            int proximoId = _formasPagamento.Count > 0 ? _formasPagamento.Max(f => f.Id) + 1 : 1;
            novaForma.Id = proximoId;

            _formasPagamento.Add(novaForma);

            return CreatedAtAction(nameof(ObterPorId), new { id = novaForma.Id }, novaForma);
        }

        // 4. Atualizar registro existente (PUT)
        // Rota: PUT /api/formaspagamento/2
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] FormaPagamento formaAtualizada)
        {
            if (formaAtualizada == null)
            {
                return BadRequest(new { erro = "Objeto de dados nao fornecido." });
            }

            // Validação de integridade entre rota e corpo
            if (id != formaAtualizada.Id)
            {
                return BadRequest(new { erro = "O ID da rota difere do ID do objeto fornecido." });
            }

            var itemExistente = _formasPagamento.FirstOrDefault(f => f.Id == id);
            if (itemExistente == null)
            {
                return NotFound(new { erro = $"Forma de pagamento com ID {id} nao encontrada para atualizacao." });
            }

            if (string.IsNullOrWhiteSpace(formaAtualizada.Nome))
            {
                return BadRequest(new { erro = "O nome nao pode ficar em branco." });
            }

            // Atualização dos campos em memória
            itemExistente.Nome = formaAtualizada.Nome;
            itemExistente.Ativo = formaAtualizada.Ativo;

            // Retorna 204 NoContent para indicar sucesso sem corpo de resposta
            return NoContent();
        }

        // 5. Excluir registro existente (DELETE)
        // Rota: DELETE /api/formaspagamento/1
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var itemExistente = _formasPagamento.FirstOrDefault(f => f.Id == id);
            if (itemExistente == null)
            {
                return NotFound(new { erro = $"Forma de pagamento com ID {id} nao encontrada para exclusao." });
            }

            _formasPagamento.Remove(itemExistente);

            return NoContent();
        }
    }
}
```

### Demonstração dos Testes no Swagger:
1. Executar `PUT /api/formaspagamento/2` alterando o nome de "Cartao de Credito" para "Cartao de Credito / Debito".
   - Demonstrar o retorno `204 NoContent`.
   - Executar `GET /api/formaspagamento/2` para comprovar que o valor foi alterado.
2. Executar `PUT /api/formaspagamento/2` enviando `{ "id": 5, "nome": "Teste" }` e mostrar o retorno `400 BadRequest` devido à divergência de IDs.
3. Executar `DELETE /api/formaspagamento/1` e demonstrar a exclusão com `204 NoContent`.
4. Executar `GET /api/formaspagamento` e verificar que o item 1 não consta mais na lista.

---