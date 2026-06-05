namespace ApiEstagioBicicletaria.Dtos.EstoqueDtos
{
    public class EstoqueSimplificadoOutputDto
    {

        public Guid Id { get; private set; }


        public int QuantidadeEmEstoque { get; private set; }

        public EstoqueSimplificadoOutputDto(Guid id, int quantidadeEmEstoque)
        {
            Id = id;
            QuantidadeEmEstoque = quantidadeEmEstoque;
        }
       
    }
}
