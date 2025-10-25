namespace ApiEstagioBicicletaria.Entities.UsuarioDomain
{
    public class Usuario
    {
        public Guid Id { get; private set; } = new Guid();

        public string Email { get; set; }=string.Empty;

        public string Senha { get; set; } = String.Empty;
    }
}
