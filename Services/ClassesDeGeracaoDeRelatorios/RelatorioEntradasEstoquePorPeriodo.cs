using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioEntradasEstoquePorPeriodo : IDocument
    {

        private List<EntradaEstoqueRelatorioDto> _entradas;

        public RelatorioEntradasEstoquePorPeriodo(List<EntradaEstoqueRelatorioDto> entradas)
        {
            _entradas = entradas;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
