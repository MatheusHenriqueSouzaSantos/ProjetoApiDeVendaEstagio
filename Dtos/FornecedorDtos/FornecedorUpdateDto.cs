using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.FornecedorDtos
{
    public class FornecedorUpdateDto
    {
        [Required(ErrorMessage = "O campo telefone é origatório")]
        [MaxLength(20, ErrorMessage = "A quantidade máxima para o Telefone é 20 caracteres")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo Email é origatório")]
        [MaxLength(200, ErrorMessage = "A quantidade máxima para o Email é 200 caracteres")]
        [EmailAddress(ErrorMessage = "O Email Deve estar em um formato válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Razão Social é obrigatório")]
        [MaxLength(100, ErrorMessage = "A quantidade máxima é de 100 caracteres")]
        public string RazaoSocial { get; set; }

        [MaxLength(100, ErrorMessage = "A quantidade máxima é de 100 caracteres")]
        public string NomeFantasia { get; set; }

        [MaxLength(15, ErrorMessage = "A quantidade máxima é de 15 caracteres")]
        public string InscricaoEstadual { get; set; }

        public FornecedorUpdateDto(string telefone, string email, string razaoSocial, string nomeFantasia, string inscricaoEstadual)
        {
            Telefone = telefone;
            Email = email;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            InscricaoEstadual = inscricaoEstadual;
        }
    }
}
