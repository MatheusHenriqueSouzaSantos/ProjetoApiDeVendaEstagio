using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria;

public class ItemEntradaEstoqueOutputDto
{

    public Guid Id {get;private set;}

    public DateTime DataCriacao {get; private set;}

    public bool Ativo {get; private set;}

    public Guid IdProduto {get;private set;}

    public string NomeDoProduto {get; private set;}

    public int Quantidade {get; private set;}
    
    public ItemEntradaEstoqueOutputDto(Guid id, DateTime dataCriacao, bool ativo, Guid idProduto, string nomeDoProduto, int quantidade)
    {
        Id = id;
        DataCriacao = dataCriacao;
        Ativo = ativo;
        IdProduto = idProduto;
        NomeDoProduto = nomeDoProduto;
        Quantidade = quantidade;
    }
    
}
