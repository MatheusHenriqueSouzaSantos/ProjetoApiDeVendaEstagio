using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Entities
{
    public class Estoque
    {
        public Guid Id { get; set; }=Guid.NewGuid();

        public Produto Produto { get; set; }

        public DateTime DataCriacao { get; set; }=DateTime.Now;

        public int QuantidadeEmEstoque { get; set; } = 0;

        public bool Ativo {  get; set; }

        public Estoque(Produto produto)
        {
            Produto = produto;
        }
    }
}
