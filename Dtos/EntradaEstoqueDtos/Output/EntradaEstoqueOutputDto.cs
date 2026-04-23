using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria;

public class EntradaEstoqueOutputDto
{

    public Guid Id { get; private set; }

    public DateTime DataCriacao {  get; private set; }

    public bool Ativo { get; private set; }

    public List<ItemEntradaEstoqueOutputDto> Itens {get; private set;}

    public Vendedor Vendedor {get; private set;}

    public string CodigoEntrada { get; private set; }

    public EntradaEstoqueOutputDto(Guid id, DateTime dataCriacao, bool ativo, List<ItemEntradaEstoqueOutputDto> itens, Vendedor vendedor)
    {
        Id = id;
        DataCriacao = dataCriacao;
        Ativo = ativo;
        Itens = itens;
        Vendedor = vendedor;
    }

}
