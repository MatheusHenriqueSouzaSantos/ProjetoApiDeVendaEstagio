using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioProdutosMaisVendidos : IDocument
    {

        private readonly List<ProdutoMaisVendidoDto> _produtos;

        public RelatorioProdutosMaisVendidos(List<ProdutoMaisVendidoDto> produtos)
        {
            this._produtos = produtos;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Content().Column(col =>
                {
                    col.Item().Text("Relatório de Produtos Mais Vendidos").FontSize(20).Bold();
                    col.Item().PaddingVertical(10);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(colunms =>
                        {
                            colunms.RelativeColumn();
                            colunms.RelativeColumn();
                            colunms.RelativeColumn();
                            colunms.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().Text("Código De Barra").Bold();
                            header.Cell().Text("Nome").Bold();
                            header.Cell().Text("Preço Unitário").Bold();
                            header.Cell().Text("Quantidade Vendida").Bold();
                        });

                        foreach(ProdutoMaisVendidoDto dto in _produtos)
                        {
                            table.Cell().Text(dto.Produto.CodigoDeBarra);
                            table.Cell().Text(dto.Produto.NomeProduto);
                            table.Cell().Text(dto.Produto.PrecoUnitario);
                            table.Cell().Text(dto.QuantidadeVendida);
                        }
                    });
                });
            });
                
        }

    }
}
