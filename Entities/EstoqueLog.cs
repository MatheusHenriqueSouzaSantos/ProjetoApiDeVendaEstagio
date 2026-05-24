using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities
{
    public class EstoqueLog : EntidadeBaseLog
    {
        public Estoque Estoque { get; private set; }

        public Guid IdEstoque { get; private set; }
        protected EstoqueLog()
        {
        }

        public EstoqueLog(Estoque estoque,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Estoque=estoque;
            IdEstoque=estoque.Id;
        }



    }
}
