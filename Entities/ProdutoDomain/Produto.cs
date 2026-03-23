namespace ApiEstagioBicicletaria.Entities.ProdutoDomain
{
    public class Produto : EntityBase
    {

        //deixar alterar codigo de barra?
        public string CodigoDeBarra { get; private set; }

        public string NomeProduto { get; set; }

        public string Descricao { get; set; }

        public decimal PrecoUnitario { get; set; }


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
