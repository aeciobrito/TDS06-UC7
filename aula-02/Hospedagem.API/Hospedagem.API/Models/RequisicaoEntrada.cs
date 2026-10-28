namespace Hospedagem.API.Models
{
    // receber uma requisição de cadastro de produto
    public class RequisicaoEntrada
    {
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoCusto { get; set; }
    }
}
