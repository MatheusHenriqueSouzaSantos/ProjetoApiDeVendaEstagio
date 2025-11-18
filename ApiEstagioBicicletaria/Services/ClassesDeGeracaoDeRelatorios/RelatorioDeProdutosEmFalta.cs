using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioDeProdutosEmFalta : IDocument
    {
        private readonly List<Produto> _produtos;

        public RelatorioDeProdutosEmFalta(List<Produto> produtos)
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
                    col.Item().Text("Relatório de Produtos Em Falta").FontSize(20).Bold();
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
                            header.Cell().Text("Nome Do Produto").Bold();
                            header.Cell().PaddingLeft(-15).PaddingRight(20).AlignRight().Text("Preço Unitário").Bold();
                            header.Cell().PaddingLeft(-15).AlignRight().Text("Quantidade Em Estoque").Bold();
                        });

                        foreach (Produto produto in _produtos)
                        {
                            table.Cell().AlignRight().PaddingRight(32).Text(produto.CodigoDeBarra);
                            table.Cell().Text(produto.NomeProduto);
                            table.Cell().PaddingLeft(-15).PaddingRight(20).AlignRight().Text($"R$ {produto.PrecoUnitario}");
                            table.Cell().PaddingLeft(-15).AlignRight().Text(produto.QuantidadeEmEstoque.ToString());
                        }
                    });
                });
            });

        }
    }
}
