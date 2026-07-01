using ApiEstagioBicicletaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteJuridicoInputDto : ClienteInputDto
    {
        [Required(ErrorMessage ="O campo Razao Social é obrtigatório")]
        [StringLength(120, ErrorMessage = "O campo Razão Social deve ter no máximo 120 caracteres")]
        public string RazaoSocial { get;  set; }

        [StringLength(30, ErrorMessage = "O campo Nome Fantasia deve ter no máximo 30 caracteres")]
        public string? NomeFantasia { get;  set; }=string.Empty;

        //[StringLength(10, ErrorMessage = "O campo Inscrição Estadual deve ter no máximo 10 caracteres")]
        public string? InscricaoEstadual { get;  set; }=string.Empty;


        public ClienteJuridicoInputDto()
        {

        }

        public ClienteJuridicoInputDto(EnderecoDto endereco, string telefone, string email) : base(endereco, telefone, email)
        {
        }

    }
}
