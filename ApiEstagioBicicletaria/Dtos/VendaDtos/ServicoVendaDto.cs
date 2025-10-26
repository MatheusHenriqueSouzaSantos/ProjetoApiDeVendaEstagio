namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ServicoVendaDto
    {
        public Guid IdServico { get; set; }

        public decimal? DescontoServico { get; set; } = 0.0m;

        public decimal PrecoServicoNaVenda { get; set; } = 0.0m;

        public ServicoVendaDto()
        {

        }

        public ServicoVendaDto(Guid idServico, decimal? descontoServico, decimal precoServicoNaVenda)
        {
            IdServico = idServico;
            DescontoServico = descontoServico;
            PrecoServicoNaVenda = precoServicoNaVenda;
        }
    }
    
}
