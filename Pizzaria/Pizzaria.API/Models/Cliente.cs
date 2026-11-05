namespace Pizzaria.API.Models
{
    public class Cliente
    {
        public int Id { get; private set; }
        public string Telefone { get; private set; } = string.Empty;
        public string Endereco { get; private set; } = string.Empty;
        public string? CPF { get; private set; }
    }
}
