using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.ProdutoDtos
{
    public class ProdutoInputDto
    {
        [MaxLength(128,ErrorMessage = "O campo Codigo de barra deve ter no máximo 128 caracteres")]
        public string CodigoDeBarra { get; set;}=string.Empty;

        [Required(ErrorMessage ="O campo nome é obrigatório")]
        [MaxLength(50,ErrorMessage = "O campo Nome deve ter no máximo 50 caracteres")]
        public string NomeProduto { get; set; } = string.Empty;

        [MaxLength(150, ErrorMessage = "O campo descrição deve ter no máximo 150 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Preço é obrigatório")]
        [Range(0.0, 1000000.0, ErrorMessage = "O valor de Preço deve estar no intervalo de 0.0 até 1000000.0")]
        public decimal PrecoUnitario { get; set; } = 0.0m;

        protected ProdutoInputDto()
        {

        }

        public ProdutoInputDto(string nomeProduto, string descricao, decimal precoUnitario)
        {
            NomeProduto = nomeProduto;
            Descricao = descricao;
            PrecoUnitario = precoUnitario;
        }
    }
}
