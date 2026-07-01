using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendedorDtos
{
    public class VendedorCreateDto
    {
        [Required(ErrorMessage ="O campo telefone é origatório")]
        [MaxLength(20,ErrorMessage ="A quantidade máxima para o Telefone é 20 caracteres")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "O campo Email é origatório")]
        [MaxLength(200, ErrorMessage = "A quantidade máxima para o Email é 200 caracteres")]
        [EmailAddress(ErrorMessage ="O Email Deve estar em um formato válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Nome Completo é origatório")]
        [MaxLength(100, ErrorMessage = "A quantidade máxima para o Nome Completo é 100 caracteres")]
        public string NomeCompleto { get; set; }
        [Required(ErrorMessage = "O campo Cpf Completo é origatório")]
        public string Cpf {  get; set; }

        public VendedorCreateDto(string telefone, string email, string nomeCompleto, string cpf)
        {
            Telefone = telefone;
            Email = email;
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
        }
    }
}
