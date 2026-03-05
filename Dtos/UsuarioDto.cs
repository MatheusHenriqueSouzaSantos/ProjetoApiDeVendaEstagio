using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos
{
    public class UsuarioDto
    {
        [Required(ErrorMessage ="O email não pode ser vazio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "a enha não pode ser vazio")]
        public string Senha { get; set; }

        protected UsuarioDto() 
        {
            
        }

        public UsuarioDto(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }
    }
}
