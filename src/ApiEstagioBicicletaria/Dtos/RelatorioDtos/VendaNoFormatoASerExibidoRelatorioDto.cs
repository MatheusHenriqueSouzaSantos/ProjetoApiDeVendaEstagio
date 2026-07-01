namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class VendaNoFormatoASerExibidoRelatorioDto
    {
        public string CodigoVenda { get; set; }
        public string NomeCliente { get; set; }

        public string CpfOuCnpj { get; set; }

        public string TipoDePagamento {  get; set; }

        public string DataDaVenda { get; set; }

        public decimal ValorTotalPago { get; set; }

        public decimal ValorTotal { get; set; }

        public string Pago { get; set; }

        public VendaNoFormatoASerExibidoRelatorioDto(string codigoVenda,string nomeCliente, string cpfOuCnpj, string tipoDePagamento, string dataDaVenda, decimal valorTotalPago, decimal valorTotal, string pago)
        {
            CodigoVenda= codigoVenda;
            NomeCliente = nomeCliente;
            CpfOuCnpj= cpfOuCnpj;
            TipoDePagamento = tipoDePagamento;
            DataDaVenda = dataDaVenda;
            ValorTotalPago = valorTotalPago;
            ValorTotal = valorTotal;
            Pago = pago;
        }
    }
}
