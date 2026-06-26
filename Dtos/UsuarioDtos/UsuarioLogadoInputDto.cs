using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.UsuarioDtos
{
    public class UsuarioLogadoInputDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(70, ErrorMessage = "O nome deve ter no máximo 70 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo email é obrigatório")]
        [MaxLength(150, ErrorMessage = "O tamanho máximo do campo email deve ser de 150 caracteres")]
        [EmailAddress(ErrorMessage = "O email deve estar em um formato válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório")]
        [MaxLength(20, ErrorMessage = "a senha deve conter no máximo 20 caracteres")]
        public string Senha { get; set; }

        public UsuarioLogadoInputDto(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }
    }
}
