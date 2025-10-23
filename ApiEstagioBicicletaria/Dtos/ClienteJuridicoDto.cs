using ApiEstagioBicicletaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos
{
    public class ClienteJuridicoDto : ClienteDto
    {
        [Required(ErrorMessage ="O campo Razao Social é obrtigatório")]
        [StringLength(70, ErrorMessage = "O campo Razão Social deve ter no máximo 70 caracteres")]
        public string RazaoSocial { get;  set; }

        [StringLength(30, ErrorMessage = "O campo Nome Fantasia deve ter no máximo 30 caracteres")]
        public string NomeFantasia { get;  set; }=string.Empty;

        [StringLength(10, ErrorMessage = "O campo Inscrição Estadual deve ter no máximo 10 caracteres")]
        public string InscricaoEstadual { get;  set; }=string.Empty;

        //[Required(ErrorMessage ="O campo Cnpj é obrigatório")]
        public string Cnpj { get;  set; }

        public ClienteJuridicoDto()
        {

        }

        public ClienteJuridicoDto(EnderecoDto endereco,string telefone,string email,
            string razaoSocial, string nomeFantasia, string inscricaoEstadual, string cnpj) 
            : base(endereco,telefone,email)
        {
            this.RazaoSocial = razaoSocial;
            this.NomeFantasia = nomeFantasia ?? "";
            this.InscricaoEstadual = inscricaoEstadual ?? "";
            this.Cnpj = cnpj;
        }
    }
}
