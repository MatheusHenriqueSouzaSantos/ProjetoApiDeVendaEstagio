using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class VendedorLogService
    {
        
        private readonly LogRepositorio<VendedorLog> _repositorio;

        public VendedorLogService(LogRepositorio<VendedorLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Vendedor vendedor, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Vendedor).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(vendedor);

                VendedorLog log = new(vendedor,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Vendedor vendedorAntigo, Vendedor vendedorAtualizado, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Vendedor).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AnotacaoDeAtributoASerIgnoradoLog)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(vendedorAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(vendedorAtualizado);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    VendedorLog log = new(vendedorAtualizado,
                    LogAcao.Atualizacao,
                    propriedade.Name,
                    valorAntigoPropriedade?.ToString(),
                    valorAtualizadoPropriedade?.ToString(),
                    usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(Vendedor vendedor, Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new VendedorLog(vendedor, LogAcao.Exclusao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel));
        }
    }
}

