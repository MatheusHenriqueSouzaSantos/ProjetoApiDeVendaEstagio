using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioEntradasEstoquePorPeriodo : IDocument
    {

        private List<EntradaEstoqueRelatorioDto> _entradas;

        private readonly DateOnly _dataInicialDoPeriodo;

        private readonly DateOnly _dataFinalDoPeriodo;

        public RelatorioEntradasEstoquePorPeriodo(List<EntradaEstoqueRelatorioDto> entradas, 
            DateOnly dataInicialDoPeriodo, DateOnly dataFinalDoPeriodo)
        {
            _entradas = entradas;
            _dataInicialDoPeriodo = dataInicialDoPeriodo;
            _dataFinalDoPeriodo = dataFinalDoPeriodo;
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
                    col.Item().Text($"Relatório Entradas Estoque por Período: {_dataInicialDoPeriodo} à " +
                        $"{_dataFinalDoPeriodo}").FontSize(20).Bold();
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
                            header.Cell().AlignCenter().Text("Código Entrada").Bold();
                            header.Cell().AlignCenter().Text("Razão Social Fornecedor").Bold();
                            header.Cell().AlignRight().Text("Cnpj").Bold();
                            header.Cell().AlignRight().Text("Quantidade De Produtos").Bold();
                        });
                        table.Cell().ColumnSpan(4).PaddingTop(8).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Darken3);

                        if (_entradas.Count == 0)
                        {
                            table.Cell().ColumnSpan(4).AlignCenter().Text("Nenhum Registro Encontrado");
                        }
                        else
                        {
                            foreach (ProdutoEmFaltaDto produto in _produtos)
                            {
                                table.Cell().AlignRight().PaddingRight(17).Text(produto.CodigoDeBarra);
                                table.Cell().AlignLeft().PaddingLeft(11).Text(produto.NomeProduto);
                                table.Cell().AlignRight().PaddingRight(1).Text($"R$ {produto.PrecoUnitario}");
                                table.Cell().AlignRight().Text(produto.QuantidadeEmEstoque.ToString());
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
