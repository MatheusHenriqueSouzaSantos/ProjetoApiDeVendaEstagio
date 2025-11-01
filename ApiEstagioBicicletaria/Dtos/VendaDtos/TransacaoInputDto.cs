using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class TransacaoInputDto
    {
        [Required(ErrorMessage ="O campo tipo de pagamento é obrigatório")]
        [EnumDataType(typeof(TipoPagamento),ErrorMessage ="tipo inválido")]
        public TipoPagamento TipoPagamento { get; set; }

        [Required(ErrorMessage = "O campo meio de pagamento é obrigatório")]
        [EnumDataType(typeof(MeioPagamaneto), ErrorMessage = "tipo inválido")]
        public MeioPagamaneto MeioPagamaneto { get; set; }

        [Required(ErrorMessage ="O campo quantidade de parcelas é obrigatório")]
        [Range(0, 1000000, ErrorMessage = "A quantidade de parcelas não pode ser negativa")]
        public int QuantidadeDeParcelas { get; set; }
        //precisa ter uma opção de criar a venda já como pago????
        //o resto dass informações pego pelo valor e soma de cada item e etc...

        protected TransacaoInputDto()
        {

        }

        public TransacaoInputDto(TipoPagamento tipoPagamento, MeioPagamaneto meioPagamaneto, int quantidadeDeParcelas)
        {
            TipoPagamento = tipoPagamento;
            MeioPagamaneto = meioPagamaneto;
            QuantidadeDeParcelas = quantidadeDeParcelas;
        }

    }
}
