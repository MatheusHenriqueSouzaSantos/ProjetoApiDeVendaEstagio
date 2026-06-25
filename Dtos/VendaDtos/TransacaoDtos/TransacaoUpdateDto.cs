using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos
{
    public class TransacaoUpdateDto
    {
        [EnumDataType(typeof(TipoPagamento), ErrorMessage = "tipo inválido")]
        public TipoPagamento? TipoPagamento { get; set; }

        [EnumDataType(typeof(MeioPagamento), ErrorMessage = "tipo inválido")]
        public MeioPagamento? MeioPagamento { get; set; }
        [Range(1, 1000000, ErrorMessage = "A quantidade de parcelas deve ser maior que 0")]
        public int? QuantidadeDeParcelas { get; set; }

        public DateOnly? DataDeVencimentoPrimeiraParcela { get; set; }

        protected TransacaoUpdateDto()
        {

        }

        public TransacaoUpdateDto(TipoPagamento? tipoPagamento, MeioPagamento? meioPagamento, int? quantidadeDeParcelas, DateOnly? dataDeVencimentoPrimeiraParcela)
        {
            TipoPagamento = tipoPagamento;
            MeioPagamento = meioPagamento;
            QuantidadeDeParcelas = quantidadeDeParcelas;
            DataDeVencimentoPrimeiraParcela = dataDeVencimentoPrimeiraParcela;
        }
    }
}
