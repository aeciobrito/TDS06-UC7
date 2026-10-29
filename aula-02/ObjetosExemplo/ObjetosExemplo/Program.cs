class Produto
{
    string nome;
    int preco;
    bool ativo;

    public Produto(string nome, int preco, bool ativo)
    {
        this.nome = nome;
        this.preco = preco;
        this.ativo = ativo;
    }
}

namespace Aplicacao
{
    class Program
    {
        var p1 = new 
        { 
            nome = "batata", 
            valor = 90, 
            ativo = true 
        };
    }
}

