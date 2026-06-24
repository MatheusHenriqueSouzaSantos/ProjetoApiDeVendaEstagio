using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class ParcelaLogService
    {
        private readonly LogRepositorio<ParcelaLog> _repositorio;

        public ParcelaLogService(LogRepositorio<ParcelaLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Parcela parcela,Transacao transacaoDaParcela, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Parcela).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade = propriedade.GetValue(parcela);

                ParcelaLog log = new(parcela,
                    transacaoDaParcela,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Parcela parcelaAntiga, Parcela parcelaAtualizada,Transacao transacaoDaParcela, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Parcela).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(parcelaAntiga);
                var valorAtualizadoPropriedade = propriedade.GetValue(parcelaAtualizada);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    ParcelaLog log = new(
                        parcelaAtualizada,
                        transacaoDaParcela,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogDeParcelaPaga(Parcela parcela, Transacao transacaoDaParcela, Usuario usuarioResponsavel)
        {
            ParcelaLog log = new(parcela,
                transacaoDaParcela,
                LogAcao.Exclusao,
                "Pago",
                "false",
                "true",
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }

        public void CriarLogDeExclusao(Parcela parcela,Transacao transacaoDaParcela, Usuario usuarioResponsavel)
        {
            ParcelaLog log = new(parcela,
                transacaoDaParcela,
                LogAcao.Exclusao,
                "Ativo",
                "true",
                "false",
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }
    }
}
