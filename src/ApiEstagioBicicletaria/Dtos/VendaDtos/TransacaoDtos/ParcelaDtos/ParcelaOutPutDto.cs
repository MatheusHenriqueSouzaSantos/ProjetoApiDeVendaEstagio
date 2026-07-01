using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos.ParcelaDtos
{
    public class ParcelaOutPutDto
    {
        public Guid Id { get; set; }

        public DateTime DataCriacao { get;  set; }

        public bool Ativo { get; set; }

        public Guid IdTransacao { get; private set; }

        public int NumeroDaParcelaDaVenda { get; set; }

        public decimal ValorParcela { get; set; } = 0.0m;

        public bool Pago { get; set; } = false;

        public DateOnly DataVencimento { get; set; }

        public ParcelaOutPutDto(Guid id, DateTime dataCriacao, bool ativo, Guid idTransacao, int numeroDaParcelaDaVenda, decimal valorParcela, bool pago, DateOnly dataVencimento)
        {
            Id = id;
            DataCriacao = dataCriacao;
            Ativo = ativo;
            IdTransacao = idTransacao;
            NumeroDaParcelaDaVenda = numeroDaParcelaDaVenda;
            ValorParcela = valorParcela;
            Pago = pago;
            DataVencimento = dataVencimento;
        }
    }
}
