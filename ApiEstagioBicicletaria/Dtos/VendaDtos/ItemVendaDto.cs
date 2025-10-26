namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ItemVendaDto
    {
        public Guid IdProduto { get; set; }

        public int Quantidade { get; set; }

        public decimal? DescontoUnitario { get; set; } = 0.0m;

        public decimal PrecoUnitarioNaVenda { get; set; } = 0.0m;

        public ItemVendaDto()
        {

        }

        public ItemVendaDto(Guid idProduto, int quantidade, decimal? descontoUnitario, decimal precoUnitarioNaVenda)
        {
            IdProduto = idProduto;
            Quantidade = quantidade;
            DescontoUnitario = descontoUnitario;
            PrecoUnitarioNaVenda = precoUnitarioNaVenda;
        }
    }
}
