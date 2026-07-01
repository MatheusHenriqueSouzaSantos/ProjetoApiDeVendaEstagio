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

        public AcaoQueAlterouEstoque AcaoQueAlterouEstoque { get;private set; }
        protected EstoqueLog()
        {
        }

        public EstoqueLog(AcaoQueAlterouEstoque acaoQueAlterouEstoque,Estoque estoque,Produto produto,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            AcaoQueAlterouEstoque = acaoQueAlterouEstoque;
            Estoque=estoque;
            IdEstoque=estoque.Id;
            Produto=produto;
            IdProduto=produto.Id;
        }



    }
}
