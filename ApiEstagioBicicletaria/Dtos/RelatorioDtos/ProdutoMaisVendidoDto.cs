using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class ProdutoMaisVendidoDto
    {
        public Produto Produto { get; set; }

        public int QuantidadeVendida { get; set; }
    }
}