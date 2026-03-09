namespace ApiEstagioBicicletaria.Entities.ProdutoDomain
{
    public class Produto
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        //deixar alterar codigo de barra?
        public string CodigoDeBarra { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public string NomeProduto { get; set; }

        public string Descricao { get; set; }


        public decimal PrecoUnitario { get; set; }

        public bool Ativo { get; set; } = true;

        protected Produto()
        {

        }
        public Produto(string codigoDeBarra, string nomeProduto, string descricao, decimal precoUnitario)
        {
            CodigoDeBarra = codigoDeBarra;
            NomeProduto = nomeProduto;
            Descricao = descricao;
            PrecoUnitario = precoUnitario;
        }
    }
}
