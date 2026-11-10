namespace Pousada.API.Models
{
    public class Chale
    {
        public int Id { get; set; }
        public string Localizacao { get; set; } = string.Empty;
        public int Capacidade { get; set; }
        public decimal ValorAltaEstacao { get; set; }
        public decimal ValorBaixaEstacao { get; set; }
    }
}
