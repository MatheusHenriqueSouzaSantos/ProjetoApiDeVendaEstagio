using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.FornecedorDtos
{
    public class FornecedorLogOutputDto : BaseLogOutputDto
    {
        public Guid IdFornecedor {  get; private set; }

        public FornecedorLogOutputDto(Guid idFornecedor,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo
            , Guid idUsuarioResponsavel, DateTime dataCriacao) 
            : base(TipoDtoLog.Fornecedor,acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdFornecedor = idFornecedor;
        }



    }
}
