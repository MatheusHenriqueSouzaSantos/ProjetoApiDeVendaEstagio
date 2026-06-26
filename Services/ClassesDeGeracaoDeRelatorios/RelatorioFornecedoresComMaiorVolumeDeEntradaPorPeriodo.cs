using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioFornecedoresComMaiorVolumeDeEntradaPorPeriodo : IDocument
    {

        private readonly List<FornecedorComMaisEntradasDto> _fornecedoresComDadosEntrada;

        private readonly DateOnly _dataInicialDoPeriodo;

        private readonly DateOnly _dataFinalDoPeriodo;

        public RelatorioFornecedoresComMaiorVolumeDeEntradaPorPeriodo(List<FornecedorComMaisEntradasDto> fornecedoresComDadosEntrada,
            DateOnly dataInicialDoPeriodo, DateOnly dataFinalDoPeriodo)
        {
            _fornecedoresComDadosEntrada = fornecedoresComDadosEntrada;
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
                    col.Item().Text($"Relatório de Fornecedores Com Maior Volume de Entrada Por Período: {_dataInicialDoPeriodo} à " +
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
                            colunms.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().AlignCenter().Text("Razao Social").Bold().FontSize(10);
                            header.Cell().AlignCenter().Text("Cnpj").Bold().FontSize(10);
                            header.Cell().AlignRight().Text("Total De Entradas").Bold().FontSize(10);
                            header.Cell().AlignRight().PaddingRight(-5).Text("Total De Produtos").Bold().FontSize(10);
                            header.Cell().AlignRight().Text("Total De Itens").Bold().FontSize(10);
                        });
                        table.Cell().ColumnSpan(5).PaddingTop(8).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Darken3);

                        if (_fornecedoresComDadosEntrada.Count == 0)
                        {
                            table.Cell().ColumnSpan(5).AlignCenter().Text("Nenhum Registro Encontrado");
                        }
                        else
                        {
                            foreach (FornecedorComMaisEntradasDto fornecedorComDadosEntrada in _fornecedoresComDadosEntrada)
                            {
                                table.Cell().AlignLeft().PaddingRight(10).Text(fornecedorComDadosEntrada.RazaoSocial);
                                table.Cell().AlignLeft().PaddingLeft(11).Text(fornecedorComDadosEntrada.Cnpj);
                                table.Cell().AlignRight().PaddingRight(1).Text(fornecedorComDadosEntrada.QuantidadeDeEntradas);
                                table.Cell().AlignRight().PaddingRight(-5).Text(fornecedorComDadosEntrada.QuantidadeDeProdutosDasEntrada);
                                table.Cell().AlignRight().Text(fornecedorComDadosEntrada.QuantidadeTotalDosItens);
                                table.Cell().ColumnSpan(5).PaddingTop(6).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Medium);
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
