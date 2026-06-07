using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.EstoqueDomain
{
    public class EstoqueLog : EntidadeBaseLog
    {
        public Estoque Estoque { get; private set; }

        public Guid IdEstoque { get; private set; }

        public Produto Produto { get; private set; }

        public Guid IdProduto { get; private set; }
        protected EstoqueLog()
        {
        }

        public EstoqueLog(Estoque estoque,Produto produto,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Estoque=estoque;
            IdEstoque=estoque.Id;
            Produto=produto;
            IdProduto=produto.Id;
        }



    }
}
