using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using QuestPDF.Infrastructure;

namespace ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios
{
    public class RelatorioDeVendedoresComMaiorFaturamentoPorPeriodo : IDocument
    {

        private readonly List<VendedoreComMaiorFaturamentoPorPeriodo> _vendedores;

        public RelatorioDeVendedoresComMaiorFaturamentoPorPeriodo(List<VendedoreComMaiorFaturamentoPorPeriodo> vendedores)
        {
            _vendedores = vendedores;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            
        }
    }
}
