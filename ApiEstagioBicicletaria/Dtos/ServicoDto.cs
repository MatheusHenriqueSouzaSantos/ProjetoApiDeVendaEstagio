using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos
{
    public class ServicoDto
    {
        [MaxLength(128,ErrorMessage = "O campo Codigo Serviço deve ter no máximo 128 caracteres")]
        public string CodigoServico { get; set;}=string.Empty;

        [Required(ErrorMessage ="O campo nome é obrigatório")]
        [MaxLength(50,ErrorMessage = "O campo Nome deve ter no máximo 50 caracteres")]
        public string NomeServico { get; set; } = string.Empty;

        [MaxLength(150, ErrorMessage = "O campo descrição deve ter no máximo 150 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Preço é obrigatório")]
        [Range(0.0, 1000000.0, ErrorMessage = "O valor de Preço deve estar no intervalo de 0.0 até 1000000.0")]
        public decimal PrecoServico { get; set; } = 0.0m;

        protected ServicoDto()
        {

        }

        public ServicoDto(string codigoServico, string nomeServico, string descricao, decimal precoServico)
        {
            CodigoServico = codigoServico;
            NomeServico = nomeServico;
            Descricao = descricao;
            PrecoServico = precoServico;
        }
    }
}
