using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.FornecedorDomain
{
    public class FornecedorLog : EntidadeBaseLog
    {
        public Fornecedor Fornecedor { get; private set; }
        public Guid IdFornecedor { get; private set; }
        protected FornecedorLog()
        {
        }

        public FornecedorLog(Fornecedor fornecedor,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Fornecedor=fornecedor;
            IdFornecedor = fornecedor.Id;
        }

    }
}
