namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public record ProdutoEmFaltaDto(Guid Id, string CodigoDeBarra, string NomeProduto, decimal PrecoUnitario, int QuantidadeEmEstoque);
}
