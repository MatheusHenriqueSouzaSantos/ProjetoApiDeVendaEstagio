namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class EntradaEstoqueRelatorioDto
    {
        public string CodigoEntrada { get;private set; }

        public string RazaoSocial { get;private set; }

        public string Cnpj {  get;private set; }

        public int QuatidadeProdutosQueRealizaramEntrada { get;private set; }
    
        public List<ItemEntradaEstoqueRelatorio> Itens { get; private set; }

        public EntradaEstoqueRelatorioDto(string codigoEntrada, string razaoSocial, string cnpj,
            int quatidadeProdutosQueRealizaramEntrada, List<ItemEntradaEstoqueRelatorio> itens)
        {
            CodigoEntrada = codigoEntrada;
            RazaoSocial = razaoSocial;
            Cnpj = cnpj;
            QuatidadeProdutosQueRealizaramEntrada = quatidadeProdutosQueRealizaramEntrada;
            Itens = itens;
        }
    }
}
