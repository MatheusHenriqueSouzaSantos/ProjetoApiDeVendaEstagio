using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class DatasParaGeracaoDeRelatorioDto
    {
        [Required(ErrorMessage = "A Data De Inicio Do Periodo é obrigatória")]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$",ErrorMessage ="O campo deve estar no formato yyyy=MM-dd")]
        public string DataDeInicioDoPeriodo { get; set; }

        [Required(ErrorMessage = "A Data Final Do Periodo é obrigatória")]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "O campo deve estar no formato yyyy=MM-dd")]
        public string DataFinalDoPeriodo { get; set; }



        public DatasParaGeracaoDeRelatorioDto(string dataDeInicioDoPeriodo, string dataFinalDoPeriodo)
        {
            DataDeInicioDoPeriodo = dataDeInicioDoPeriodo;
            DataFinalDoPeriodo = dataFinalDoPeriodo;
        }
    }
}
