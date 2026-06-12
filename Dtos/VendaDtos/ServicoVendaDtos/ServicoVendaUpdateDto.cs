namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos
{
    public class ServicoVendaUpdateDto : ServicoVendaInputDto
    {
        public Guid IdServicoVenda { get; set; }
        public ServicoVendaUpdateDto()
        {
        }

        public ServicoVendaUpdateDto(Guid idServicoVenda,decimal? descontoServico) : base(descontoServico)
        {
            IdServicoVenda = idServicoVenda;
        }




    }
}
