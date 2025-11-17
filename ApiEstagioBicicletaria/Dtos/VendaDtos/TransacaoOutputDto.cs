using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class TransacaoOutputDto
    {
        public Guid IdTransacao { get; set; }

        public DateTime DataCriacao { get; private set; }

        public TipoPagamento TipoPagamento { get; set; }

        public MeioPagamaneto MeioPagamento { get; set; }

        public bool TransacaoEmCurso { get; set; } = false;

        public bool Pago { get; set; } = false;

        public int NumeroDeParcelasNaoPagas { get; set; }

        public int NumeroDeParcelasPagas { get; set; } = 0;

        public decimal ValorPago { get; set; }


        //public List<Parcela> Parcelas { get; set; }

        protected TransacaoOutputDto()
        {

        }

        public TransacaoOutputDto(Guid idTransacao, DateTime dataCriacao, TipoPagamento tipoPagamento, MeioPagamaneto meioPagamento, bool transacaoEmCurso, bool pago, int numeroDeParcelasNaoPagas, int numeroDeParcelasPagas, decimal valorPago)
        {
            IdTransacao = idTransacao;
            DataCriacao = dataCriacao;
            TipoPagamento = tipoPagamento;
            MeioPagamento = meioPagamento;
            TransacaoEmCurso = transacaoEmCurso;
            Pago = pago;
            NumeroDeParcelasNaoPagas = numeroDeParcelasNaoPagas;
            NumeroDeParcelasPagas = numeroDeParcelasPagas;
            ValorPago = valorPago;
        }


        //, List<Parcela> parcelas



    }
}
