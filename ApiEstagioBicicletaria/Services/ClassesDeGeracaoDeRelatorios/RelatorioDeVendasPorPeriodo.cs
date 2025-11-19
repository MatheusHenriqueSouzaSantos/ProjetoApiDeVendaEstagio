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

        public RelatorioDeVendasPorPeriodo(List<VendaNoFormatoASerExibidoRelatorioDto> vendas, DateOnly dataDeInicioDoPeriodo, DateOnly dateDeFimDoPeriodo)
        {
            this._vendas = vendas;
            _dataDeInicioDoPeriodo = dataDeInicioDoPeriodo;
            _dateDeFimDoPeriodo = dateDeFimDoPeriodo;
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
                            header.Cell().Text("Tipo De Pagamento").Bold();
                            header.Cell().Text("Meio De Pagamento").Bold();
                            header.Cell().Text("Data Da Venda").Bold();
                            header.Cell().AlignRight().Text("Valor Total Pago").Bold();
                            header.Cell().AlignRight().Text("Valor Total").Bold();
                            header.Cell().AlignCenter().Text("Pago").Bold();
                        });

                        foreach (VendaNoFormatoASerExibidoRelatorioDto vendaDto in _vendas)
                        {
                            table.Cell().PaddingBottom(5).PaddingTop(5).Text(vendaDto.NomeCliente);
                            table.Cell().PaddingBottom(5).PaddingTop(5).Text(vendaDto.TipoDePagamento);
                            table.Cell().PaddingBottom(5).PaddingTop(5).Text(vendaDto.MeioDePagamento);
                            table.Cell().PaddingBottom(5).PaddingTop(5).Text(vendaDto.DataDaVenda);
                            table.Cell().PaddingBottom(5).PaddingTop(5).AlignRight().Text(vendaDto.ValorTotalPago.ToString("F2"));
                            table.Cell().PaddingBottom(5).PaddingTop(5).AlignRight().Text(vendaDto.ValorTotal.ToString("F2"));
                            table.Cell().PaddingBottom(5).PaddingTop(5).AlignCenter().Text(vendaDto.Pago);
                        }
                    });
                });
            });
        }

    }
}
