using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class VendedorComMaiorFaturamentoPorPeriodo
    {
        public Guid VendedorId { get; private set; }

        public string VendedorNome { get; private set; }

        public string Cpf {  get; private set; }

        public int QuantidadeDeVendas { get; private set; }

        public decimal Faturamento { get; private set; }

        public VendedorComMaiorFaturamentoPorPeriodo(Guid vendedorId, string vendedorNome, string cpf, int quantidadeDeVendas, decimal faturamento)
        {
            VendedorId = vendedorId;
            VendedorNome = vendedorNome;
            Cpf = cpf;
            QuantidadeDeVendas = quantidadeDeVendas;
            Faturamento = faturamento;
        }
    }
}
