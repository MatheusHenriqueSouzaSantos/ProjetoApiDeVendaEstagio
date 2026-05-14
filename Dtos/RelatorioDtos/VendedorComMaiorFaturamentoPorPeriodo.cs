using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class VendedorComMaiorFaturamentoPorPeriodo
    {
        public Guid VendedorId { get; private set; }

        public string VendedorNome { get; private set; }

        public int QuantidadeDeVendas { get; private set; }

        public decimal Faturamento { get; private set; }

        public VendedorComMaiorFaturamentoPorPeriodo(Guid vendedorId, string vendedorNome, int quantidadeDeVendas, decimal faturamento)
        {
            VendedorId = vendedorId;
            VendedorNome = vendedorNome;
            QuantidadeDeVendas = quantidadeDeVendas;
            Faturamento = faturamento;
        }
    }
}
