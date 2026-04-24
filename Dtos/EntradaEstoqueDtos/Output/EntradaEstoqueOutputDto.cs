using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria;

public class EntradaEstoqueOutputDto
{

    public Guid Id { get; private set; }

    public DateTime DataCriacao {  get; private set; }

    public bool Ativo { get; private set; }

    public List<ItemEntradaEstoqueOutputDto> Itens {get; private set;}

    public Fornecedor Fornecedor {get; private set;}

    public string CodigoEntrada { get; private set; }

    public EntradaEstoqueOutputDto(Guid id, DateTime dataCriacao, 
        bool ativo, List<ItemEntradaEstoqueOutputDto> itens, Fornecedor fornecedor, string codigoEntrada)
    {
        Id = id;
        DataCriacao = dataCriacao;
        Ativo = ativo;
        Itens = itens;
        Fornecedor = fornecedor;
        CodigoEntrada = codigoEntrada;
    }
}
