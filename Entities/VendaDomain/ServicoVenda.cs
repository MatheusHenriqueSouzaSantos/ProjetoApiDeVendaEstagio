using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ServicoVenda : EntidadeBase
    {
        [AnotacaoDeAtributoASerIgnoradoLog]
        public Venda Venda { get; private set; }

        public Guid IdVenda { get; private set; }

        [AnotacaoDeAtributoASerIgnoradoLog]
        public Servico Servico { get; private set; }

        public Guid IdServico {  get; private set; }

        public decimal DescontoServico { get; set; }

        public decimal PrecoServicoNaVendaSemDesconto { get;  set; }


        protected ServicoVenda()
        {

        }

        public ServicoVenda(Venda venda, Servico servico, decimal descontoServico, decimal precoServicoNaVendaSemDesconto)
        {
            Venda = venda;
            IdVenda = venda.Id;
            Servico = servico;
            IdServico = servico.Id;
            DescontoServico = descontoServico;
            PrecoServicoNaVendaSemDesconto = precoServicoNaVendaSemDesconto;
        }

        public ServicoVenda Copia()
        {
            return new ServicoVenda(Venda, Servico, DescontoServico, PrecoServicoNaVendaSemDesconto);
        }
    }
}
