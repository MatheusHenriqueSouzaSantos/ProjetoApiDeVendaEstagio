using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.UsuarioDtos
{
    public class UsuarioLogOutputDto : BaseLogOutputDto
    {
        public Guid IdUsuario { get; set; }

        public string CodigoUsuario { get; set; }
        public UsuarioLogOutputDto(Guid idUsuario,string codigoUsuario,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao) 
            : base(TipoDtoLog.Usuario, acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdUsuario = idUsuario;
            CodigoUsuario = codigoUsuario;
        }
    }
}
