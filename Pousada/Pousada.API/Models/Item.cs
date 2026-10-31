namespace Pousada.API.Models
{
    public class Item
    {
        // sem encapsulamento
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
    }
}
