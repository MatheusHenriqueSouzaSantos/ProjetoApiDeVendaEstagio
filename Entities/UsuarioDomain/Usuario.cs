namespace ApiEstagioBicicletaria.Entities.UsuarioDomain
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; set; }=string.Empty;

        public string Email { get; set; }=string.Empty;

        public string Senha { get; set; } = string.Empty;

        public PerfilUsuario PerfilUsuario {  get; set; }
        
        protected Usuario() { }

        public Usuario(string nome, string email, string senha, PerfilUsuario perfilUsuario)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            PerfilUsuario = perfilUsuario;
        }

        public Usuario Copia()
        {
            return new Usuario(Nome, Email, Senha,PerfilUsuario);
        }
    }
}
