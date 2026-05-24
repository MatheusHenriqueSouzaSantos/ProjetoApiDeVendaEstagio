namespace ApiEstagioBicicletaria.Entities.UsuarioDomain
{
    public class UsuarioLog : EntidadeBaseLog
    {

        public Usuario Usuario { get; private set; }
        public Guid IdUsuario { get; private set; }
        protected UsuarioLog()
        {
        }

        public UsuarioLog(Usuario usuario, LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Usuario = usuario;
            IdUsuario = usuario.Id;
        }


    }
}
