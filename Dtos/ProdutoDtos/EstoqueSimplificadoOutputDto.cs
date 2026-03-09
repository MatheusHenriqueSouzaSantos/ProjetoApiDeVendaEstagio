namespace ApiEstagioBicicletaria.Dtos.ProdutoDtos
{
    public class EstoqueSimplificadoOutputDto
    {
        public Guid Id { get; set; }

        public int QuantidadeEmEstoque { get; set; }

        public EstoqueSimplificadoOutputDto(Guid id, int quantidadeEmEstoque)
        {
            Id = id;
            QuantidadeEmEstoque = quantidadeEmEstoque;
        }
    }
}
