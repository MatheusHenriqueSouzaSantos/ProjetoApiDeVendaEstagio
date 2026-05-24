using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.EntradaEstoque
{
    public class EntradaEstoqueLog : EntidadeBaseLog
    {
        public EntradaEstoque EntradaEstoque { get; private set; }
        public Guid IdEntradaEstoque { get; private set; }
        protected EntradaEstoqueLog()
        {
        }

        public EntradaEstoqueLog(EntradaEstoque entradaEstoque, LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            EntradaEstoque=entradaEstoque;
            IdEntradaEstoque = entradaEstoque.Id;
        }



    }
}
