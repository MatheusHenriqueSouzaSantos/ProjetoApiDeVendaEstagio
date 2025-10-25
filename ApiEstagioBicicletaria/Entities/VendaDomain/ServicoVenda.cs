using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ServicoVenda
    {
        //criar a venda no momento do envio ou criar ela e ir mandando adição de item...(mais complexo/melhor)
        public Guid Id { get; private set; } = new Guid();

        public Venda Venda { get; private set; }

        public Servico Servico { get; private set; }

        //deixar essa prop??

        public decimal DescontoServico { get; private set; }

        public decimal PrecoServicoNaVenda { get; private set; }

        //não usar delete lógico e sim fisico, não sei se vai ter como alterar venda

        public ServicoVenda(Venda venda, Servico servico, decimal descontoServico, decimal precoServicoNaVenda)
        {
            Venda = venda;
            Servico = servico;
            DescontoServico = descontoServico;
            PrecoServicoNaVenda = precoServicoNaVenda;
        }
    }
}
