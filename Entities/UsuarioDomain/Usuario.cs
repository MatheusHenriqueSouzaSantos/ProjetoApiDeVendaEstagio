namespace ApiEstagioBicicletaria.Entities.UsuarioDomain
{
    public class Usuario : EntidadeBase
    {

        public string Email { get; set; }=string.Empty;

        public string Senha { get; set; } = String.Empty;
    }
}
