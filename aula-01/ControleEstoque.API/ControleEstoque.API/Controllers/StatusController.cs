using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public string GetStatus()
        {
            return "API retornou com sucesso!";
        }

        [HttpGet("saudacao/{operador}")]
        // ROTA FINAL: /api/status/saudacao/{operador}
        public string ObterSaudacaoRota(string operador)
        {
            return "Olá " + operador + "! Bem vindo ao Sistema.";
        }

        [HttpGet("consulta")]
        // ROTA FINAL: /api/status/consulta
        public string ConsultaModulo([FromQuery] string? modulo)
        {
            if(string.IsNullOrEmpty(modulo))
            {
                return "Consultou a lista com todos os múdlos: vendas, compras, cobranças";
            }

            return "Módulo consultado: " + modulo;
        }
    }
}
