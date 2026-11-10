using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pousada.API.Data;
using Pousada.API.Models;

namespace Pousada.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChalesController : ControllerBase
    {
        AppDbContext _dbContext;

        public ChalesController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Chales);
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Chale novoChale)
        {
            if (novoChale == null || string.IsNullOrWhiteSpace(novoChale.Localizacao))
            {
                return BadRequest("Informa esses dados direito!");
            }

            if (novoChale.ValorBaixaEstacao <= 0 || novoChale.ValorAltaEstacao <= 0)
            {
                return BadRequest("O valor do chale não pode ser menor que zero.");
            }

            //_itens.Add(novoItem);
            _dbContext.Chales.Add(novoChale);
            _dbContext.SaveChanges();

            return Ok("criado");
        }
    }
}
