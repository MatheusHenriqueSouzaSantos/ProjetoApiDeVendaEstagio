namespace ApiEstagioBicicletaria.Entities.UsuarioDomain
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; set; }=string.Empty;

        public string Email { get; set; }=string.Empty;

        public string Senha { get; set; } = string.Empty;
        
        protected Usuario() { }

        public Usuario(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }
    }
}
