using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.ServicesLogs
{
    public class ProdutoLogService
    {
        private readonly ProdutoLogRepositorio _repositorio;

        public ProdutoLogService(ProdutoLogRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public void CriarLogsDeCriacao(Produto produto,Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Produto).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade=propriedade.GetValue(produto);

                _repositorio.criarLog(produto,
                    Entities.LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade!=null ? valorPropriedade.ToString() : null,
                    usuarioResponsavel);
            }
        }

        public void CriarLogsDeExclusao(Produto produto,Usuario usuarioResponsavel) 
        {
            _repositorio.criarLog(produto,Entities.LogAcao.Exclusao,"Ativo",true.ToString(),false.ToString(),usuarioResponsavel);
        }
    }
}
