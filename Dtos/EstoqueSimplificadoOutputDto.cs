namespace ApiEstagioBicicletaria.Dtos.ProdutoDtos
{
    public class EstoqueSimplificadoOutputDto
    {

        public Guid Id { get; private set; }

        public Guid IdProduto {get; private set;}

        public string NomeProduto {get; private set;}

        public int QuantidadeEmEstoque { get; private set; }

        public EstoqueSimplificadoOutputDto(Guid id, Guid idProduto, string nomeProduto, int quantidadeEmEstoque)
        {
            Id = id;
            IdProduto = idProduto;
            NomeProduto = nomeProduto;
            QuantidadeEmEstoque = quantidadeEmEstoque;
        }
       
    }
}
