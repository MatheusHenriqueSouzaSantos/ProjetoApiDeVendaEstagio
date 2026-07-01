using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteJuridicoCreateDto : ClienteJuridicoInputDto
    {
        [Required(ErrorMessage = "o cnpj é obrigatório")]
        public string Cnpj { get; set; }
        public ClienteJuridicoCreateDto()
        {
        }

        public ClienteJuridicoCreateDto(string cnpj,EnderecoDto endereco, string telefone, string email) : base(endereco, telefone, email)
        {
            Cnpj = cnpj;
        }

        
        

    }
}
