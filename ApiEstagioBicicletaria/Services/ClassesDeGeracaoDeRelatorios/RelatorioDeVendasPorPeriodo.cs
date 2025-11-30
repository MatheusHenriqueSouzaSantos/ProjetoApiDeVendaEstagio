using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioDeVendasPorPeriodo : IDocument
    {
        private readonly List<VendaNoFormatoASerExibidoRelatorioDto> _vendas;

        private readonly DateOnly _dataDeInicioDoPeriodo;

        private readonly DateOnly _dateDeFimDoPeriodo;

        private readonly decimal _valorTotalPagoDasVendasDoPeriodo;

        private readonly decimal _valorTotalDasVendasDoPeriodo;

        public RelatorioDeVendasPorPeriodo(List<VendaNoFormatoASerExibidoRelatorioDto> vendas, DateOnly dataDeInicioDoPeriodo, DateOnly dateDeFimDoPeriodo, decimal valorTotalPagoDasVendasDoPeriodo, decimal valorTotalDasVendasDoPeriodo)
        {
            this._vendas = vendas;
            _dataDeInicioDoPeriodo = dataDeInicioDoPeriodo;
            _dateDeFimDoPeriodo = dateDeFimDoPeriodo;
            _valorTotalPagoDasVendasDoPeriodo = valorTotalPagoDasVendasDoPeriodo;
            _valorTotalDasVendasDoPeriodo = valorTotalDasVendasDoPeriodo;
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
                    col.Item().Text("Relatório de Vendas Por Período: " + _dataDeInicioDoPeriodo.ToString("dd/MM/yyyy") + " à " + _dateDeFimDoPeriodo.ToString("dd/MM/yyyy"))
                    .FontSize(20).Bold();
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
                            colunms.RelativeColumn();
                            colunms.RelativeColumn();
                            colunms.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().Text("Código Da Venda").Bold().FontSize(10);
                            header.Cell().Text("Nome Do Cliente").Bold().FontSize(10);
                            header.Cell().PaddingLeft(3).Text("Tipo De Pagamento").Bold().FontSize(10);
                            header.Cell().PaddingLeft(3).Text("Meio De Pagamento").Bold().FontSize(10);
                            header.Cell().PaddingLeft(12).PaddingRight(-8).AlignLeft().Text("Data Da Venda").Bold().FontSize(10);
                            header.Cell().AlignCenter().PaddingLeft(2).PaddingRight(-16).Text("Pago").Bold().FontSize(10);
                            header.Cell().AlignRight().Text("Valor Total Pago").Bold().FontSize(10);
                            header.Cell().AlignRight().Text("Valor Total").Bold().FontSize(10);
                        });
                        table.Cell().ColumnSpan(8).PaddingTop(8).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Darken3);

                        int tamanhoDaLista = _vendas.Count();
                        int i = 0;

                        foreach (VendaNoFormatoASerExibidoRelatorioDto vendaDto in _vendas)
                        {
                            i++;
                            table.Cell().PaddingBottom(5).PaddingTop(5).AlignLeft().PaddingRight(11).PaddingLeft(1).Text(vendaDto.CodigoVenda.ToString()).FontSize(10);
                            table.Cell().PaddingBottom(5).PaddingTop(5).Text(vendaDto.NomeCliente).FontSize(10);
                            table.Cell().PaddingBottom(5).PaddingTop(5).PaddingLeft(3).Text(vendaDto.TipoDePagamento).FontSize(10);
                            table.Cell().PaddingBottom(5).PaddingLeft(3).PaddingTop(5).PaddingRight(-1).Text(vendaDto.MeioDePagamento).FontSize(10);
                            table.Cell().PaddingBottom(5).PaddingTop(5).PaddingLeft(11).PaddingRight(-9).AlignCenter().Text(vendaDto.DataDaVenda).FontSize(10);
                            table.Cell().PaddingBottom(5).PaddingTop(5).PaddingLeft(2).PaddingRight(-13).AlignCenter().Text(vendaDto.Pago).FontSize(10);
                            table.Cell().PaddingBottom(5).PaddingTop(5).AlignRight().Text("R$ " + vendaDto.ValorTotalPago.ToString("F2")).FontSize(10);
                            table.Cell().PaddingBottom(5).PaddingTop(5).AlignRight().Text("R$ " + vendaDto.ValorTotal.ToString("F2")).FontSize(10);
                            if (i != tamanhoDaLista)
                            {
                                table.Cell().ColumnSpan(8).PaddingTop(6).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Medium);
                            }
                        }
                        table.Cell().ColumnSpan(8).PaddingTop(8).PaddingBottom(6).Border(1).BorderColor(Colors.Grey.Darken3);

                        table.Cell().ColumnSpan(6);
                        table.Cell().AlignRight().Text("Total Pago Das Vendas:").FontSize(10);
                        table.Cell().AlignRight().Text("Total Das Vendas:").FontSize(10);

                        table.Cell().ColumnSpan(6);
                        table.Cell().AlignRight().Text("R$ " + _valorTotalPagoDasVendasDoPeriodo.ToString("F2")).FontSize(10);
                        table.Cell().AlignRight().Text("R$ " + _valorTotalDasVendasDoPeriodo.ToString("F2")).FontSize(10);
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
