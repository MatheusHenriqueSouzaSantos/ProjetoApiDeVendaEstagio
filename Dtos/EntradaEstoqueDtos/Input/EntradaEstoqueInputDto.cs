using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria;

public class EntradaEstoqueInputDto
{

    [Required(ErrorMessage ="O Id fornecedor deve ser informado")]
    public Guid IdFornecedor {get;private set;}

    [Required(ErrorMessage ="A lista de itens para ser registrado a entrada precisa ser informada")]
    public List<ItemEntradaEstoqueInputDto> Itens {get;private set;}

    public EntradaEstoqueInputDto(Guid idFornecedor, List<ItemEntradaEstoqueInputDto> itens)
    {
        IdFornecedor = idFornecedor;
        Itens = itens;
    }

}
