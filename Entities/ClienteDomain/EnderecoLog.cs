using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.ClienteDomain
{
    public class EnderecoLog : EntidadeBaseLog
    {
        public Endereco Endereco { get; private set; }
        public Guid IdEndereco { get; private set; }

        public Cliente Cliente { get; private set; }

        public Guid IdCliente { get; private set; }
        protected EnderecoLog()
        {
        }

        public EnderecoLog(Endereco endereco, Cliente cliente,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Endereco = endereco;
            IdEndereco=endereco.Id;
            Cliente = cliente;
            IdCliente = cliente.Id;
        }



    }
}
