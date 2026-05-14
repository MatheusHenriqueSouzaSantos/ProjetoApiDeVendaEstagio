using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioDeVendedoresComMaiorFaturamentoPorPeriodo : IDocument
    {

        private readonly List<VendedorComMaiorFaturamentoPorPeriodo> _vendedores;
        private readonly DateOnly DataInicial;
        private readonly DateOnly DataFinal;

        public RelatorioDeVendedoresComMaiorFaturamentoPorPeriodo(List<VendedorComMaiorFaturamentoPorPeriodo> vendedores, DateOnly dataInicial, DateOnly dataFinal)
        {
            _vendedores = vendedores;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
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
                    col.Item().Text($"Relatório de Vendedores Com Maior faturamento Por Período: {DataInicial} à {DataFinal}").FontSize(20).Bold();
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
                            header.Cell().AlignCenter().Text("Indentificador").Bold();
                            header.Cell().AlignCenter().Text("Nome Do Vendedor").Bold();
                            header.Cell().AlignRight().Text("Quantidade De Vendas Realizadas").Bold();
                            header.Cell().AlignRight().Text("Faturamento").Bold();
                        });
                        table.Cell().ColumnSpan(4).PaddingTop(8).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Darken3);

                        if (_vendedores.Count == 0)
                        {
                            table.Cell().ColumnSpan(4).AlignCenter().Text("Nenhum Registro Encontrado");
                        }
                        else
                        {
                            foreach (VendedorComMaiorFaturamentoPorPeriodo vendedor in _vendedores)
                            {
                                table.Cell().AlignRight().PaddingRight(17).Text(vendedor.VendedorId);
                                table.Cell().AlignLeft().PaddingLeft(11).Text(vendedor.VendedorNome);
                                table.Cell().AlignRight().PaddingRight(1).Text(vendedor.QuantidadeDeVendas);
                                table.Cell().AlignRight().Text($"R$ {vendedor.Faturamento}");
                                table.Cell().ColumnSpan(4).PaddingTop(6).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Medium);
                            }
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
