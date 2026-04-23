using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria;

public class ItemEntradaEstoqueInputDto
{
    [Required(ErrorMessage ="O Id do produto a ser registrado a entrada precisa ser informado")]
    public Guid IdProduto {get;private set;}
    [Required(ErrorMessage ="A Quantidade a ser realizadoa a entrada desse item precisa ser informada")]
    public int Quantidade {get; private set;}
    public ItemEntradaEstoqueInputDto(Guid idProduto, int quantidade)
    {
        IdProduto = idProduto;
        Quantidade = quantidade;
    }
}
