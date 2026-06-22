using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Input
{
    public class EntradaEstoqueUpdateDto
    {
        public Guid? IdFornecedor { get;  set; }

        public List<Guid>? IdsItensASeremDeletados {  get;  set; }

        
        public List<ItemEntradaEstoqueUpdateDto>? ItensAtualizados { get;  set; }

        
        public List<ItemEntradaEstoqueCreateDto>? ItensNovos { get;  set; }

        public EntradaEstoqueUpdateDto(Guid? idFornecedor, List<Guid>? idsItensASeremDeletados, List<ItemEntradaEstoqueUpdateDto>? itensAtualizados, List<ItemEntradaEstoqueCreateDto>? itensNovos)
        {
            IdFornecedor = idFornecedor;
            IdsItensASeremDeletados = idsItensASeremDeletados;
            ItensAtualizados = itensAtualizados;
            ItensNovos = itensNovos;
        }
    }
}
