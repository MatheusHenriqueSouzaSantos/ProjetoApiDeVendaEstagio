using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ItemVendaOutputDto
    {
        public Guid Id { get; private set; }

        //public Guid IdVenda { get; private set; }

        public Produto Produto { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public int Quantidade { get; set; }

        public decimal DescontoUnitario { get; set; }

        public decimal PrecoUnitarioDoProdutoNaVenda { get; set; }

        public decimal PrecoUnitarioDoProdutoNaVendaComDescontoAplicado { get; set; }

        protected ItemVendaOutputDto()
        {

        }

        public ItemVendaOutputDto(Guid id, Produto produto, DateTime dataCriacao, int quantidade, decimal descontoUnitario, decimal precoUnitarioDoProdutoNaVenda, decimal precoUnitarioDoProdutoNaVendaComDescontoAplicado)
        {
            Id = id;
            Produto = produto;
            DataCriacao = dataCriacao;
            Quantidade = quantidade;
            DescontoUnitario = descontoUnitario;
            PrecoUnitarioDoProdutoNaVenda = precoUnitarioDoProdutoNaVenda;
            PrecoUnitarioDoProdutoNaVendaComDescontoAplicado = precoUnitarioDoProdutoNaVendaComDescontoAplicado;
        }
    }
}
