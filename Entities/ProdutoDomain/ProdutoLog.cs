using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.ProdutoDomain
{
    public class ProdutoLog : EntidadeBaseLog
    {
        public Produto Produto { get; private set; }
        public Guid IdProduto { get; private set; }
        protected ProdutoLog()
        {
        }

        public ProdutoLog(Produto produto,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Produto = produto;
            IdProduto = produto.Id;
        }

    }
}
