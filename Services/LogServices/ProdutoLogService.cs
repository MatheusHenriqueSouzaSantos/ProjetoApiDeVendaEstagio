using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.ServicesLogs
{
    public class ProdutoLogService
    {
        private readonly LogRepositorio<ProdutoLog> _repositorio;

        public ProdutoLogService(LogRepositorio<ProdutoLog> repositorio)
        {
            _repositorio = repositorio;
        }
        public void CriarLogsDeCriacao(Produto produto,Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Produto).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade=propriedade.GetValue(produto);

                ProdutoLog log = new(produto,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Produto produtoAntigo,Produto produtoAtualizado, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Produto).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(produtoAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(produtoAtualizado);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    ProdutoLog log = new(
                        produtoAtualizado,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(Produto produto,Usuario usuarioResponsavel) 
        {
            ProdutoLog log = new ProdutoLog(produto, LogAcao.Inativacao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel);

            _repositorio.CriarLog(log);
        }
    }
}
