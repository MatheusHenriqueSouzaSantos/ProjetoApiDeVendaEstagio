using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioEntradasEstoquePorPeriodo : IDocument
    {

        private List<EntradaEsoqueComSeusItensDto> entradas;

        public RelatorioEntradasEstoquePorPeriodo(List<EntradaEsoqueComSeusItensDto> entradas)
        {
            this.entradas = entradas;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
