using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos
{
    public class ClienteFisicoInputDto : ClienteInputDto
    {
        [Required(ErrorMessage ="O campo Nome é obrigatório")]
        [StringLength(70,ErrorMessage ="O campo Nome deve ter no máximo 70 caracteres")]
        public string Nome { get;  set; }

        public ClienteFisicoInputDto() { }

        public ClienteFisicoInputDto(string nome,EnderecoDto endereco, string telefone, string email) : base(endereco, telefone, email)
        {
            Nome = nome;
        }
    }
}
