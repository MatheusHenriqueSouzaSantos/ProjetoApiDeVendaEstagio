namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class ItemEntradaEstoqueRelatorio
    {
        public string CodigoDeBarra {  get; private set; }
        public string NomeProduto { get; private set; }

        public int Quantidade { get; private set; }

        public ItemEntradaEstoqueRelatorio(string codigoDeBarra, string nomeProduto, int quantidade)
        {
            CodigoDeBarra = codigoDeBarra;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
        }
    }
}
