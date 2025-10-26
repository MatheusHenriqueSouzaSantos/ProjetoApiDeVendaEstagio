using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class TransacaoDto
    {
        public TipoPagamento TipoPagamento { get; set; }

        public MeioPagamaneto MeioPagamaneto { get; set; }

        public int QuantidadeDeParcelas { get; set; }
        //precisa ter uma opção de criar a venda já como pago????
        //o resto dass informações pego pelo valor e soma de cada item e etc...

        protected TransacaoDto()
        {

        }

        public TransacaoDto(TipoPagamento tipoPagamento, MeioPagamaneto meioPagamaneto, int quantidadeDeParcelas)
        {
            TipoPagamento = tipoPagamento;
            MeioPagamaneto = meioPagamaneto;
            QuantidadeDeParcelas = quantidadeDeParcelas;
        }

    }
}
