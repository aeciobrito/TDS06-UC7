using Hospedagem.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospedagem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormasPagamentoController : ControllerBase
    {
        List<FormaPagamento> _formasPagamento = new()
        {
            new FormaPagamento { Id = 1, Nome = "Dinheiro", Ativo = true },
            new FormaPagamento { Id = 2, Nome = "Cartão de Crédito", Ativo = true },
            new FormaPagamento { Id = 3, Nome = "Pix", Ativo = true }
        };

        [HttpGet]
        public IActionResult ObterTodos()
        {
            return Ok(_formasPagamento);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var formaPagamento = _formasPagamento.FirstOrDefault(x => x.Id == id);

            if (formaPagamento == null)
            {                
                return NotFound(new { erro = $"Forma de pagamento com ID {id} não encontrada" });
            }

            return Ok(formaPagamento);
        }

        [HttpPost]
        public IActionResult Criar(FormaPagamento novaForma)
        {
            _formasPagamento.Add(novaForma);
            return Created();
        }
    }
}
