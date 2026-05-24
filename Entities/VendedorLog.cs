using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities
{
    public class VendedorLog : EntidadeBaseLog
    {
        public Vendedor Vendedor { get; private set; }

        public Guid IdVendedor { get; private set; }

        protected VendedorLog()
        {
        }

        public VendedorLog(Vendedor vendedor,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Vendedor=vendedor;
            IdVendedor = vendedor.Id;
        }

    }
}
