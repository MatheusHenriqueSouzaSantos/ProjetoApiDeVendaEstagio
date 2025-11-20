using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ServicoVenda
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Venda Venda { get; private set; }

        public Guid IdVenda { get; private set; }

        public Servico Servico { get; private set; }

        public Guid IdServico {  get; private set; }

        public DateTime DataCriacao { get; private set; }=DateTime.Now;

        public decimal DescontoServico { get; set; }

        public decimal PrecoServicoNaVendaSemDesconto { get;  set; }

        public bool Ativo { get; set; }=true;

        protected ServicoVenda()
        {

        }

        public ServicoVenda(Venda venda, Servico servico, decimal descontoServico, decimal precoServicoNaVendaSemDesconto)
        {
            Venda = venda;
            Servico = servico;
            DescontoServico = descontoServico;
            PrecoServicoNaVendaSemDesconto = precoServicoNaVendaSemDesconto;
        }
    }
}
