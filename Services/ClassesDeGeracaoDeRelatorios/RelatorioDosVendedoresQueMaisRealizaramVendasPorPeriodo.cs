using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioDosVendedoresQueMaisRealizaramVendasPorPeriodo : IDocument
    {
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            
        }
    }
}
