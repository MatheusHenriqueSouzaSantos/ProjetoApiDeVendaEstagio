using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class EstoqueLogService
    {
        private readonly LogRepositorio<EstoqueLog> _repositorio;

        public EstoqueLogService(LogRepositorio<EstoqueLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Estoque estoque, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Estoque).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(estoque);

                EstoqueLog log = new EstoqueLog(estoque,
                    Entities.LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }
        public void CriarLogsDeExclusao(Estoque estoque, Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new EstoqueLog(estoque, Entities.LogAcao.Exclusao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel));
            _repositorio.CriarLog(new EstoqueLog(estoque, Entities.LogAcao.Exclusao, "QuantidadeEmEstoque", estoque.QuantidadeEmEstoque.ToString(), 
                estoque.QuantidadeEmEstoque.ToString(), usuarioResponsavel));
        }
    }
}
