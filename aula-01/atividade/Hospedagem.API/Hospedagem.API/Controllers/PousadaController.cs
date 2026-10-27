using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospedagem.API.Controllers
{
    // /api/Pousada/
    [Route("api/[controller]")]
    [ApiController]
    public class PousadaController : ControllerBase
    {
        [HttpGet]
        public string ObterStatus()
        {
            return "Sistema de Gerenciamento da Pousada Vista Bela em operacao.";
        }

        [HttpGet("boas-vindas/{nomeHospede}")]
        // ROTA: api/Pusada/boas-vindas/fulano 
        public string BoasVindasHospede(string nomeHospede)
        {
            return $"Seja muito bem-vindo(a) a Pousada Vista Bela, {nomeHospede}! Desejamos uma excelente estadia.";
        }

        [HttpGet("unidade")]
        // ROTA: api/Pusada/unidade?chale={numeroChale}
        public string ConsultarUnidade([FromQuery] string? numeroChale)
        {
            if(numeroChale == null || numeroChale == "")
            {
                return "Chale não informado, redirecionando para o atendimento";
            }

            return $"Unidade Chale {numeroChale} selecionada para atendimento.";
        }
    }
}
