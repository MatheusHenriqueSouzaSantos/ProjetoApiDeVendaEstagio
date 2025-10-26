namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaDto
    {
        public Guid IdCliente { get; set; }

        public decimal? Desconto { get; set; } = 0.0m;

        public List<ProdutoDto> ItensVenda { get; set; }

        public List<ServicoVendaDto> ServicosVenda { get; set; }

        protected VendaDto()
        {

        }

        public VendaDto(Guid idCliente, decimal? desconto, List<ProdutoDto> itensVenda, List<ServicoVendaDto> servicosVenda)
        {
            IdCliente = idCliente;
            Desconto = desconto;
            //Desconto = desconto ?? 0.0m; fazer isso no service pois se estiver vazio o asp net usa o contrutor sem parametros, se null desconsidera, se diferente considera se não usa o valor
            ItensVenda = itensVenda;
            ServicosVenda = servicosVenda;
        }

        
    }
}
