using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.FornecedorDtos
{
    public class FornecedorLogDto : BaseLogDto
    {
        public Guid IdFornecedor {  get; private set; }

        public FornecedorLogDto(Guid idFornecedor,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo
            , Guid idUsuarioResponsavel, DateTime dataCriacao) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdFornecedor = idFornecedor;
        }



    }
}
