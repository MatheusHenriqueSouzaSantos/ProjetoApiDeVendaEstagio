namespace ApiEstagioBicicletaria.Entities.UsuarioDomain
{
    public class Usuario : EntityBase
    {

        public string Email { get; set; }=string.Empty;

        public string Senha { get; set; } = String.Empty;
    }
}
