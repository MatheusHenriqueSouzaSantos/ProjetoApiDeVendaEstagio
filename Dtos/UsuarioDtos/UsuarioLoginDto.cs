using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.Usuario
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage ="O email não pode ser vazio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "a enha não pode ser vazio")]
        public string Senha { get; set; }

        protected UsuarioLoginDto() 
        {
            
        }

        public UsuarioLoginDto(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }
    }
}
