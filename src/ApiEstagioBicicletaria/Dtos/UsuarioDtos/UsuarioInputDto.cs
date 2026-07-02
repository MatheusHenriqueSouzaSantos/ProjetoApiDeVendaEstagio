using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.UsuarioDtos
{
    public class UsuarioInputDto
    {
        [Required(ErrorMessage ="O nome é obrigatório")]
        [MaxLength(70,ErrorMessage ="O nome deve ter no máximo 70 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage ="O campo email é obrigatório")]
        [MaxLength(150,ErrorMessage ="O tamanho máximo do campo email deve ser de 150 caracteres")]
        [EmailAddress(ErrorMessage ="O email deve estar em um formato válido")]
        public string Email { get; set; }

        [Required(ErrorMessage ="O campo senha é obrigatório")]
        [MaxLength(50,ErrorMessage ="a senha deve conter no máximo 50 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage ="O campo perfil usuário é obrigatório")]
        public PerfilUsuario PerfilUsuario { get; set; }

        public UsuarioInputDto(string nome, string email, string senha, PerfilUsuario perfilUsuario)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            PerfilUsuario = perfilUsuario;
        }
    }
}
