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
        private readonly EstoqueLogRepositorio _repositorio;

        public EstoqueLogService(EstoqueLogRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public void criarLogsDeCriacao(Estoque estoque, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Estoque).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(estoque);

                _repositorio.criarLog(estoque,
                    Entities.LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade != null ? valorPropriedade.ToString() : null,
                    usuarioResponsavel);
            }
        }
        public void CriarLogsDeExclusao(Estoque estoque, Usuario usuarioResponsavel)
        {
            _repositorio.criarLog(estoque, Entities.LogAcao.Exclusao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel);
        }
    }
}
