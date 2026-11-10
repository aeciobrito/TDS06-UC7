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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Itens);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var itemBuscado = _dbContext.Itens.FirstOrDefault(itm => itm.Id == id);

            if (itemBuscado == null)
            {
                return NotFound($"Item nao localizado para o id {id}.");
            }

            return Ok(itemBuscado);
        }

        [HttpGet("porNome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            var itemBuscado = _dbContext.Itens.FirstOrDefault(itm => itm.Nome == nome);

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
            var caputraItemDalista = _dbContext.Itens.FirstOrDefault(x => x.Id == id);

            if (caputraItemDalista != null)
                _dbContext.Itens.Remove(caputraItemDalista);

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
