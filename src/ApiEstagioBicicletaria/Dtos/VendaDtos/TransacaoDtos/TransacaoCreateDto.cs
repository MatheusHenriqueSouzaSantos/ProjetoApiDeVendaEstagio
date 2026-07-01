using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos
{
    public class TransacaoCreateDto
    {
        [Required(ErrorMessage ="O campo tipo de pagamento é obrigatório")]
        [EnumDataType(typeof(TipoPagamento),ErrorMessage ="tipo inválido")]
        public TipoPagamento TipoPagamento { get; set; }

        [Required(ErrorMessage = "O campo meio de pagamento é obrigatório")]
        [EnumDataType(typeof(MeioPagamento), ErrorMessage = "tipo inválido")]
        public MeioPagamento MeioPagamento { get; set; }

        [Required(ErrorMessage ="O campo quantidade de parcelas é obrigatório")]
        [Range(1, 1000000, ErrorMessage = "A quantidade de parcelas deve ser maior que 0")]
        public int QuantidadeDeParcelas { get; set; }
        //precisa ter uma opção de criar a venda já como pago????
        //o resto dass informações pego pelo valor e soma de cada item e etc...

        [Required(ErrorMessage = "O campo Data De Vencinmento Primeira Parcela é obrigatório")]
        public DateOnly DataDeVencimentoPrimeiraParcela { get; set; }

        protected TransacaoCreateDto()
        {

        }

        public TransacaoCreateDto(TipoPagamento tipoPagamento, MeioPagamento meioPagamento, int quantidadeDeParcelas, DateOnly dataDeVencimentoPrimeiraParcela)
        {
            TipoPagamento = tipoPagamento;
            MeioPagamento = meioPagamento;
            QuantidadeDeParcelas = quantidadeDeParcelas;
            DataDeVencimentoPrimeiraParcela = dataDeVencimentoPrimeiraParcela;
        }
    }
}
