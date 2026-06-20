using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Input
{
    public class EntradaEstoqueUpdateDto
    {
        [Required(ErrorMessage = "O Id fornecedor deve ser informado")]
        public Guid IdFornecedor { get; private set; }

        [Required(ErrorMessage = "A lista de itens atualizados da entrada precisa ser informada")]
        public List<ItemEntradaEstoqueUpdateDto> ItensAtualizados { get; private set; }

        [Required(ErrorMessage = "A lista de itens novos da entrada precisa ser informada")]
        public List<ItemEntradaEstoqueCreateDto> ItensNovos { get; private set; }



        public EntradaEstoqueUpdateDto(Guid idFornecedor, List<ItemEntradaEstoqueCreateDto> itensNovos)
        {
            IdFornecedor = idFornecedor;
            ItensNovos = itensNovos;
        }
    }
}
