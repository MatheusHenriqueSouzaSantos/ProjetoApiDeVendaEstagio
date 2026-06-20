using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria;

public class EntradaEstoqueCreateDto
{

    [Required(ErrorMessage ="O Id fornecedor deve ser informado")]
    public Guid IdFornecedor {get;private set;}

    [Required(ErrorMessage ="A lista de itens para ser registrado a entrada precisa ser informada")]
    public List<ItemEntradaEstoqueCreateDto> Itens {get;private set;}

    public EntradaEstoqueCreateDto(Guid idFornecedor, List<ItemEntradaEstoqueCreateDto> itens)
    {
        IdFornecedor = idFornecedor;
        Itens = itens;
    }

}
