using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos
{
    public class ClienteFisicoDto : ClienteDto
    {
        [Required(ErrorMessage ="O campo Nome é obrigatório")]
        [StringLength(70,ErrorMessage ="O campo Nome deve ter no máximo 70 caracteres")]
        public string Nome { get;  set; }
        //[Required]
        //pois ao atualizar não pode mandar o cpf, cpf não altera
        //aplicar retirada de pontos e verificar se esta certo no service? ou aqui?
        public string Cpf { get;  set; }

        public ClienteFisicoDto() { }

        public ClienteFisicoDto(EnderecoDto endereco, string telefone, string email, 
             string nome, string cpf)
        : base(endereco, telefone, email)
        {
            this.Nome= nome;
            this.Cpf= cpf;
        }

    }
}
