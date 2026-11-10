namespace Pousada.API.Models
{
    public class Chale
    {
        public int Id { get; private set; }
        public string Localizacao { get; private set; } = string.Empty;
        public int Capacidade { get; private set; }
        public decimal ValorAltaEstacao { get; private set; }
        public decimal ValorBaixaEstacao { get; private set; }
    }
}
