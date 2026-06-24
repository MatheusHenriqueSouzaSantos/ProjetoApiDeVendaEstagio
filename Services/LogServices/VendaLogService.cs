using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class VendaLogService
    {
        private readonly LogRepositorio<VendaLog> _repositorio;

        public VendaLogService(LogRepositorio<VendaLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Venda venda, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Venda).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade = propriedade.GetValue(venda);

                VendaLog log = new(venda,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Venda vendaAntiga, Venda vendaAtualizada, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Venda).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(vendaAntiga);
                var valorAtualizadoPropriedade = propriedade.GetValue(vendaAtualizada);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    VendaLog log = new(
                        vendaAtualizada,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(Venda venda, Usuario usuarioResponsavel)
        {
            VendaLog log = new(venda,
                LogAcao.Exclusao,
                "Ativo",
                "true",
                "false",
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }
    }
}
