using ApiEstagioBicicletaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public abstract class ClienteDto
    {
        [Required(ErrorMessage ="O campo Endereço é obrigatório")]
        public EnderecoDto Endereco { get;  set; }

        [Required(ErrorMessage ="O campo Telefone é obrigatório")]
        [StringLength(20,ErrorMessage ="O campo Telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get;  set; }

        [Required(ErrorMessage ="O campo Email é obrigatório")]
        [EmailAddress(ErrorMessage ="Email com formato incorreto")]
        [StringLength(100, ErrorMessage = "O campo Email deve ter no máximo 100 caracteres")]
        public string Email { get;  set; }

        protected ClienteDto() { }

        protected ClienteDto(EnderecoDto endereco, string telefone, string email)
        {
            Endereco = endereco;
            Telefone = telefone;
            Email = email;
        }
    }
}
