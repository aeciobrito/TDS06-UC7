## 2. Fundamentação Teórica Direta ao Ponto

### O Papel de uma Model no Backend
Uma Model (modelo de domínio ou entidade) é uma classe em C# cujas propriedades representam a estrutura de dados de um elemento do mundo real com o qual o sistema opera.
Por exemplo, no Controle de Estoque temos Formas de Pagamento, Produtos, Fornecedores e Usuários. No Sistema de Hospedagem temos Itens de Consumo, Chalés, Clientes e Reservas.

Neste primeiro momento, criamos models simples, sem validações complexas e sem dependências com outras tabelas, permitindo focar na lógica de manipulação e tráfego de dados.

### Por que simular com `static List<T>`?
A cada requisição HTTP que chega a uma Web API, o framework instancia uma nova cópia do Controller e a descarta após responder (ciclo de vida transiente/stateless). Se declararmos uma lista comum dentro do Controller, ela será recriada vazia a cada chamada.
Ao utilizar `static readonly List<T>`, os dados permanecem na memória do processo da aplicação durante todo o tempo em que o servidor estiver rodando, permitindo simular inclusões e consultas sucessivas antes de conectarmos o Entity Framework Core com o banco de dados.

---

## 3. Demonstração Prática Passo a Passo (Projeto ControleEstoque)

### Passo 1: Criação da Model `FormaPagamento.cs`
Crie o arquivo `Models/FormaPagamento.cs`:

```csharp
namespace ControleEstoque.API.Models
{
    // Model simples de representação de forma de pagamento
    public class FormaPagamento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
```

### Passo 2: Criação do Controlador `FormasPagamentoController.cs`
Crie o arquivo `Controllers/FormasPagamentoController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using ControleEstoque.API.Models;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormasPagamentoController : ControllerBase
    {
        // Lista estática em memória compartilhada entre requisições
        private static readonly List<FormaPagamento> _formasPagamento = new()
        {
            new FormaPagamento { Id = 1, Nome = "Dinheiro", Ativo = true },
            new FormaPagamento { Id = 2, Nome = "Cartao de Credito", Ativo = true },
            new FormaPagamento { Id = 3, Nome = "Pix", Ativo = true }
        };

        // 1. Obter todas as formas de pagamento
        // Rota: GET /api/formaspagamento
        [HttpGet]
        public IActionResult ObterTodas()
        {
            return Ok(_formasPagamento);
        }

        // 2. Obter forma de pagamento por ID
        // Rota: GET /api/formaspagamento/1
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

        // 3. Cadastrar nova forma de pagamento
        // Rota: POST /api/formaspagamento
        [HttpPost]
        public IActionResult Criar([FromBody] FormaPagamento novaForma)
        {
            if (novaForma == null || string.IsNullOrWhiteSpace(novaForma.Nome))
            {
                return BadRequest(new { erro = "O nome da forma de pagamento e obrigatorio." });
            }

            // Gera o próximo ID autoincremental baseado no maior ID existente
            int proximoId = _formasPagamento.Count > 0 ? _formasPagamento.Max(f => f.Id) + 1 : 1;
            novaForma.Id = proximoId;

            _formasPagamento.Add(novaForma);

            // Retorna status 201 Created com a rota para consulta do novo item
            return CreatedAtAction(nameof(ObterPorId), new { id = novaForma.Id }, novaForma);
        }
    }
}
```

### Passo 3: Execução e Demonstração no Swagger
1. Execute `dotnet run`.
2. Acesse `/swagger`.
3. Execute o `GET /api/formaspagamento` e visualize a lista inicial com 3 itens.
4. Execute o `POST /api/formaspagamento` enviando:
```json
{
  "nome": "Boleto Bancario",
  "ativo": true
}
```
5. Observe o status retornado: `201 Created` e o cabeçalho `Location`.
6. Execute o `GET /api/formaspagamento/4` e comprove que o novo item foi armazenado e retornado.

---

## 4. Orientações para a Atividade dos Alunos (20:45 às 22:00)

1. Envie o arquivo `atividade.md` para a turma.
2. Os alunos devem criar a Model `Item` (itens de consumo da pousada) e o `ItensController` em memória no projeto `Hospedagem.API`.
3. Verifique o entendimento de LINQ (`FirstOrDefault`, `Max`) e do método `CreatedAtAction`.
4. Às 22:00, realize a correção guiada com `solucao.md`.
