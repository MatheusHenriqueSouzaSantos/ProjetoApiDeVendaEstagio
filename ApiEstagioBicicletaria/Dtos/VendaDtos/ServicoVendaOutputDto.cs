using ApiEstagioBicicletaria.Entities.ServicoDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ServicoVendaOutputDto
    {
        public Guid Id { get; private set; }

        public Servico Servico { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public decimal DescontoServico { get; set; }

        public decimal PrecoServicoNaVenda { get; set; }

        public decimal PrecoServicoNaVendaComDescontoAplicado { get; set; }

        protected ServicoVendaOutputDto()
        {

        }

        public ServicoVendaOutputDto(Guid id, Servico servico, DateTime dataCriacao, decimal descontoServico, decimal precoServicoNaVenda, decimal precoServicoNaVendaComDescontoAplicado)
        {
            Id = id;
            Servico = servico;
            DataCriacao = dataCriacao;
            DescontoServico = descontoServico;
            PrecoServicoNaVenda = precoServicoNaVenda;
            PrecoServicoNaVendaComDescontoAplicado = precoServicoNaVendaComDescontoAplicado;
        }
    }
}
