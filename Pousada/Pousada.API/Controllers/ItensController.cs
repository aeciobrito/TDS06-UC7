using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pousada.API.Data;
using Pousada.API.Models;

namespace Pousada.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensController : ControllerBase
    {
        AppDbContext _dbContext;

        public ItensController(AppDbContext context)
        {
            _dbContext = context;
        }

        private static readonly List<Item> _itens = new()
        {
            new Item { Id = 1, Nome = "Agua Mineral 500ml", Preco = 5.00m, Descricao = "Agua sem gas"},
            new Item { Id = 2, Nome = "Refrigerante Lata", Preco = 8.50m, Descricao = "Lata 350ml"},
            new Item { Id = 3, Nome = "Pacote de Biscoito Artesanal", Preco = 12.00m, Descricao = "Biscoito amanteigado caseiro" }
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Itens);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var itemBuscado = _itens.FirstOrDefault(itm => itm.Id == id);

            if (itemBuscado == null)
            {
                return NotFound($"Item nao localizado para o id {id}.");
            }

            return Ok(itemBuscado);
        }

        [HttpGet("porNome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            var itemBuscado = _itens.FirstOrDefault(itm => itm.Nome == nome);

            if (itemBuscado == null)
            {
                return NotFound($"Item nao localizado para o id {nome}.");
            }

            return Ok(itemBuscado);
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Item novoItem)
        {
            if(novoItem == null || string.IsNullOrWhiteSpace(novoItem.Nome))
            {
                return BadRequest("Informa esses dados direito!");
            }

            if (novoItem.Preco <= 0)
            {
                return BadRequest("De graça nem injeção na testa.");
            }

            //_itens.Add(novoItem);
            _dbContext.Itens.Add(novoItem);
            _dbContext.SaveChanges();

            return Ok("criado");
        }


        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            var caputraItemDalista = _itens.FirstOrDefault(x => x.Id == id);

            if (caputraItemDalista != null)
                _itens.Remove(caputraItemDalista);

            return NoContent();
        }
    }
}
