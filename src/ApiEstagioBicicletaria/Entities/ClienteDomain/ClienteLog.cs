using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.ClienteDomain
{
    public class ClienteLog : EntidadeBaseLog
    {
        public Cliente Cliente { get; private set; }

        public Guid IdCliente { get; private set; }

        public TipoCliente TipoCliente { get; private set; }


        protected ClienteLog()
        {
        }

        public ClienteLog(Cliente cliente, TipoCliente tipoCliente,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) :
            base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Cliente = cliente;
            IdCliente = cliente.Id;
            TipoCliente= tipoCliente;
        }
    }
}
