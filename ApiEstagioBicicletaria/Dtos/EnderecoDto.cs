using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos
{
    public class EnderecoDto
    {
        [Required(ErrorMessage ="O campo logradouro é obrigat´rio")]
        [StringLength(80, ErrorMessage ="O campo Logradouro deve ter no máximo 80 caracteres")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage ="O campo número é obrigatório")]
        [StringLength(15, ErrorMessage ="ó campo Nomero deve ter no máximo 15 caracteres")]
        public string Numero { get;  set; }

        [Required(ErrorMessage ="O campo cidade é obrigatório")]
        [StringLength(35,ErrorMessage ="O campo Cidade deve ter no máximo 35 caracteres")]
        public string Cidade { get;  set; }

        [Required(ErrorMessage ="O campo SiglaUf é obrigatório")]
        [StringLength(2,ErrorMessage ="O campo Sigla uf deve ter 2 caracteres")]
        public string SiglaUf { get;  set; }

        public EnderecoDto()
        {

        }

        public EnderecoDto(string logradouro, string numero, string cidade, string siglaUf)
        {
            Logradouro = logradouro;
            Numero = numero;
            Cidade = cidade;
            SiglaUf = siglaUf;
        }
    }
}
