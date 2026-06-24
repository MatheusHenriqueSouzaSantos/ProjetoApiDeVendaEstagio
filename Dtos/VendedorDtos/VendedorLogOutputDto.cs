using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendedorDtos
{
    public class VendedorLogOutputDto : BaseLogOutputDto
    {
        public Guid IdVendedor { get; private set; }

        public VendedorLogOutputDto(Guid idVendedor,LogAcao acao, string campoAlterado, 
            string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdVendedor= idVendedor; 
        }

        
    }
}
