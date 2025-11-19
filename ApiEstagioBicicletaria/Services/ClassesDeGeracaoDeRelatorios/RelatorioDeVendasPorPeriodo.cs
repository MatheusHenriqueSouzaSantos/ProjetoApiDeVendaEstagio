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
                        });
                        table.Header(header =>
                        {
                            header.Cell().Text("Nome Do Cliente").Bold();
                            header.Cell().PaddingLeft(3).Text("Tipo De Pagamento").Bold();
                            header.Cell().PaddingLeft(3).Text("Meio De Pagamento").Bold();
                            header.Cell().Text("Data Da Venda").Bold();
                            header.Cell().AlignCenter().PaddingLeft(6).Text("Pago").Bold();
                            header.Cell().AlignRight().Text("Valor Total Pago").Bold();
                            header.Cell().AlignRight().Text("Valor Total").Bold();
                        });

                        foreach (VendaNoFormatoASerExibidoRelatorioDto vendaDto in _vendas)
                        {
                            table.Cell().PaddingBottom(5).PaddingTop(5).Text(vendaDto.NomeCliente);
                            table.Cell().PaddingLeft(3).PaddingBottom(5).PaddingTop(5).Text(vendaDto.TipoDePagamento);
                            table.Cell().PaddingLeft(3).PaddingBottom(5).PaddingTop(5).Text(vendaDto.MeioDePagamento);
                            table.Cell().PaddingBottom(5).PaddingTop(5).Text(vendaDto.DataDaVenda);
                            table.Cell().PaddingBottom(5).PaddingTop(5).PaddingLeft(6).AlignCenter().Text(vendaDto.Pago);
                            table.Cell().PaddingBottom(5).PaddingTop(5).AlignRight().Text(vendaDto.ValorTotalPago.ToString("F2"));
                            table.Cell().PaddingBottom(5).PaddingTop(5).AlignRight().Text(vendaDto.ValorTotal.ToString("F2"));
                        }
                        table.Cell().ColumnSpan(7).PaddingTop(10).PaddingBottom(10).Border(2).BorderColor(Colors.Grey.Darken3);

                        table.Cell().ColumnSpan(5);
                        table.Cell().AlignRight().Text("Total Pago Das Vendas:");
                        table.Cell().AlignRight().Text("Total Das Vendas:");

                        table.Cell().ColumnSpan(5);
                        table.Cell().AlignRight().Text(_valorTotalPagoDasVendasDoPeriodo.ToString("F2"));
                        table.Cell().AlignRight().Text(_valorTotalDasVendasDoPeriodo.ToString("F2"));
                    });
                });
            });
        }

    }
}
