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
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                        });

                        table.Cell().ColumnSpan(5);
                        table.Cell().ColumnSpan(3).TranslateX(143).TranslateY(-45).AlignRight().AlignTop().PaddingBottom(-80).Width(120).Height(60).Image("Resources/LogoBikeCiaShopParaEstagio.jpg").FitArea();
                    });
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
                            header.Cell().PaddingLeft(9).Text("Nome Do Produto").Bold();
                            header.Cell().AlignRight().PaddingRight(5).Text("Preço Unitário").Bold();
                            header.Cell().AlignRight().Text("Quantidade Vendida").Bold();
                        });
                        table.Cell().ColumnSpan(4).PaddingTop(8).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Darken3);
                        foreach (ProdutoMaisVendidoDto dto in _produtos)
                        {
                            table.Cell().AlignRight().PaddingRight(33).Text(dto.Produto.CodigoDeBarra);
                            table.Cell().PaddingLeft(9).Text(dto.Produto.NomeProduto);
                            table.Cell().AlignRight().PaddingRight(6).Text($"R$  {dto.Produto.PrecoUnitario}");
                            table.Cell().AlignRight().Text(dto.QuantidadeVendida.ToString());
                            table.Cell().ColumnSpan(4).PaddingTop(6).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Medium);
                        }
                    });
                });
                page.Footer().AlignRight().Text(text =>
                {
                    text.Span($"Gerado em: {DateTime.Now.ToString("HH:mm dd/MM/yyyy")} - ").FontSize(10);
                    text.Span("Página ").FontSize(10);
                    text.CurrentPageNumber().FontSize(10);
                    text.Span(" de ").FontSize(10);
                    text.TotalPages().FontSize(10);
                });
            });
                
        }

    }
}
