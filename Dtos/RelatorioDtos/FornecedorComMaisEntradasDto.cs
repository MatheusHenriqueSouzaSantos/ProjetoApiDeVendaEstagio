namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class FornecedorComMaisEntradasDto
    {
        public string RazaoSocial { get; private set; }

        public string Cnpj {  get; private set; }

        public int QuantidadeDeEntradas { get; private set; }

        public int QuantidadeDeProdutosDasEntrada { get; private set; }

        public int QuantidadeTotalDosItens { get; private set; }

        public FornecedorComMaisEntradasDto(string razaoSocial, string cnpj, int quantidadeDeEntradas, 
            int quantidadeDeProdutosDasEntrada, int quantidadeTotalDosItens)
        {
            RazaoSocial = razaoSocial;
            Cnpj = cnpj;
            QuantidadeDeEntradas = quantidadeDeEntradas;
            QuantidadeDeProdutosDasEntrada = quantidadeDeProdutosDasEntrada;
            QuantidadeTotalDosItens = quantidadeTotalDosItens;
        }
    }
}
