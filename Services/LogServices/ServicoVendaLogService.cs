using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class ServicoVendaLogService
    {
        private readonly LogRepositorio<ServicoVendaLog> _repositorio;

        public ServicoVendaLogService(LogRepositorio<ServicoVendaLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(ServicoVenda servicoVenda, Venda venda, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ServicoVenda).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade = propriedade.GetValue(servicoVenda);

                ServicoVendaLog log = new(servicoVenda,
                    venda,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(ServicoVenda servicoVendaAntigo, ServicoVenda servicoVendaAtualizado, Venda vendaDoServico, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ServicoVenda).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(servicoVendaAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(servicoVendaAtualizado);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    ServicoVendaLog log = new(
                        servicoVendaAtualizado,
                        vendaDoServico,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(ServicoVenda servicoVenda, Venda venda, Usuario usuarioResponsavel)
        {
            ServicoVendaLog log = new(servicoVenda,
                venda,
                LogAcao.Exclusao,
                "Ativo",
                "true",
                "false",
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }
    }
}
