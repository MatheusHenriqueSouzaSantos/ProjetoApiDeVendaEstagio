using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class AtualizarQuantidadeDeParcelasPagasInputDto
    {
        [Required(ErrorMessage ="O campo quantidade de pareclas pagas é obrigatório")]
        //fazer validação se há esse tanto de parcelas e colocar a venda em andamento
        //[Range(1,1000,ErrorMessage ="")]
        //fazer validação que tem que ser pelo menos uma parcela e não mais do que a quantidade de parcelas que existe na venda no service
        public int QuantidadeDeParcelasASerAtualizadaParaPaga { get; set; } = 0;

        protected AtualizarQuantidadeDeParcelasPagasInputDto()
        {

        }

        public AtualizarQuantidadeDeParcelasPagasInputDto(int quantidadeDeParcelasASerAtualizadaParaPaga)
        {
            QuantidadeDeParcelasASerAtualizadaParaPaga = quantidadeDeParcelasASerAtualizadaParaPaga;
        }
    }
}
