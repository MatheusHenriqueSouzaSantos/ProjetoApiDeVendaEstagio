using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos
{
    public class DocumentoClienteInputDto
    {
        [Required]
        public EnumTipoDocumentoASerBuscado TipoDocumento { get; set; }
        [Required]
        public string NumeroDocumento { get; set; } = string.Empty;

        public DocumentoClienteInputDto(EnumTipoDocumentoASerBuscado tipoDocumento, string numeroDocumento)
        {
            this.TipoDocumento = tipoDocumento;
            NumeroDocumento = numeroDocumento;
        }
    }
}
