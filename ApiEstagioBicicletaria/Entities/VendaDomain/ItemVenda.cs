using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ItemVenda
    {
        //todos os atributos deixo que nunca podme ser alterados? se errou crie outro item venda?
        //deixar privado pois ao enviar a venda já esta concluida
        public Guid Id { get; private set; } = new Guid();

        public Venda Venda { get; private set; }

        public Produto Produto { get; private set; }

        public int Quantidade { get; private  set; }

        public decimal DescontoPorUnidade { get; private set; }

        public decimal PrecoUnitarioNaVenda { get; private set; }

        protected ItemVenda()
        {

        }

        public ItemVenda(Guid id, Venda venda, Produto produto, int quantidade, decimal descontoPorUnidade, decimal precoUnitarioNaVenda)
        {
            Id = id;
            Venda = venda;
            Produto = produto;
            Quantidade = quantidade;
            DescontoPorUnidade = descontoPorUnidade;
            PrecoUnitarioNaVenda = precoUnitarioNaVenda;
        }
    }
}
