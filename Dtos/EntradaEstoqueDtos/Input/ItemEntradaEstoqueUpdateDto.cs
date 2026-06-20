using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Input
{
    public class ItemEntradaEstoqueUpdateDto
    {
        [Required(ErrorMessage = "O Id do item da entrada precisa ser informado")]
        public Guid IdDoItem { get;  set; }
        [Required(ErrorMessage = "A Quantidade a ser realizadoa a entrada desse item precisa ser informada")]
        [Range(1, 10000, ErrorMessage = "O valor mínimo de quantidade precisa ser maior que 0 e menor ou igual 100000")]
        public int Quantidade { get;  set; }

        public ItemEntradaEstoqueUpdateDto(Guid idDoItem, int quantidade)
        {
            IdDoItem = idDoItem;
            Quantidade = quantidade;
        }
        protected ItemEntradaEstoqueUpdateDto()
        {

        }
    }
}
