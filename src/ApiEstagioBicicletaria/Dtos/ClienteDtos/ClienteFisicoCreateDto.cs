using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteFisicoCreateDto : ClienteFisicoInputDto
    {
        [Required(ErrorMessage ="o cpf é obrigatório")]
        public string Cpf { get; set; }
        public ClienteFisicoCreateDto()
        {
        }


        public ClienteFisicoCreateDto(EnderecoDto endereco, string telefone, string email, string nome, string cpf) : base(nome,endereco, telefone, email)
        {
            Cpf = cpf;
        }


    }
}
