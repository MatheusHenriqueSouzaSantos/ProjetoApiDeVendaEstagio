namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ServicoVendaDto
    {
        public Guid IdServico { get; set; }

        public decimal? DescontoServico { get; set; } = 0.0m;

        //public decimal PrecoServicoNaVenda { get; set; } = 0.0m;
        //posso pegar pelo serviço e produto pelo id ou manda o preco unitario o front, pois é editavel???

        public ServicoVendaDto()
        {

        }

        public ServicoVendaDto(Guid idServico, decimal? descontoServico)
        {
            IdServico = idServico;
            DescontoServico = descontoServico;
            //PrecoServicoNaVenda = precoServicoNaVenda;
        }
    }
    
}
