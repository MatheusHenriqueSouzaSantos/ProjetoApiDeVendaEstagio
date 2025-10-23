namespace ApiEstagioBicicletaria.Entities
{
    public class Produto
    {
        public Guid Id { get; private set; } = new Guid();

        //deixar alterar codigo de barra?
        public string CodigoDeBarra { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public string NomeProduto { get; set; }

        public string Descricao { get; set; }

        public int QuantidadeEmEstoque { get; set; } = 0;

        public decimal PrecoUnitario { get; set; }

        protected Produto()
        {

        }
        public Produto(string codigoDeBarra, string nomeProduto, string descricao, int quantidadeEmEstoque, decimal precoUnitario)
        {
            CodigoDeBarra = codigoDeBarra;
            NomeProduto = nomeProduto;
            Descricao = descricao;
            QuantidadeEmEstoque = quantidadeEmEstoque;
            PrecoUnitario = precoUnitario;
        }
    }
}
