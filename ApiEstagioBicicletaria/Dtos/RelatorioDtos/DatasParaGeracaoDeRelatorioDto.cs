namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class DatasParaGeracaoDeRelatorioDto
    {
        public string DataDeInicioDoPeriodo { get; set; }

        public string DataDeFimDoPeriodo { get; set; }



        public DatasParaGeracaoDeRelatorioDto(string dataDeInicioDoPeriodo, string dataDeFimDoPeriodo)
        {
            DataDeInicioDoPeriodo = dataDeInicioDoPeriodo;
            DataDeFimDoPeriodo = dataDeFimDoPeriodo;
        }
    }
}
