namespace Pizzaria.API.Models
{
    public class Pizza
    {
        public int Id { get; private set; }
        public string Sabor { get; private set; } = string.Empty;
        public string Ingredientes { get; private set; } = string.Empty;
        public decimal Valor { get; private set; }
        public bool Disponivel { get; private set; } = true;
    }
}
