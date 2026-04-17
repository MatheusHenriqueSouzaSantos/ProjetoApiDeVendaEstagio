namespace ApiEstagioBicicletaria.Entities.ProdutoDomain
{
    public class Produto : EntidadeBase
    {

        //deixar alterar codigo de barra?
        public string CodigoDeBarra { get; private set; }

        public string NomeProduto { get; set; }

        public string Descricao { get; set; }

        public decimal Preco { get; set; }


        protected Produto()
        {

        }
        public Produto(string codigoDeBarra, string nomeProduto, string descricao, decimal preco)
        {
            CodigoDeBarra = codigoDeBarra;
            NomeProduto = nomeProduto;
            Descricao = descricao;
            Preco = preco;
        }
    }
}
