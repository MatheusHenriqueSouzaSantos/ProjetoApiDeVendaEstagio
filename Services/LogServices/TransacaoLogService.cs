using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class TransacaoLogService
    {
        private readonly LogRepositorio<TransacaoLog> _repositorio;

        public TransacaoLogService(LogRepositorio<TransacaoLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Transacao transacao, Venda venda, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Transacao).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade = propriedade.GetValue(transacao);

                TransacaoLog log = new(transacao,
                    venda,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Transacao transacaoAntiga, Transacao transacaoAtualizada,Venda vendaDaTransacao, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Transacao).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(transacaoAntiga);
                var valorAtualizadoPropriedade = propriedade.GetValue(transacaoAtualizada);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    TransacaoLog log = new(
                        transacaoAtualizada,
                        vendaDaTransacao,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }
        public void CriarLogDeTransacaoEmCurso(Transacao transacao, Venda vendaDaTransacao, Usuario usuarioResponsavel)
        {
            TransacaoLog log = new(transacao,
                vendaDaTransacao,
                LogAcao.Exclusao,
                "TransacaoEmCurso",
                "false",
                "true",
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }

        public void CriarLogDeTransacaoPaga(Transacao transacao, Venda vendaDaTransacao, Usuario usuarioResponsavel)
        {
            TransacaoLog log = new(transacao,
                vendaDaTransacao,
                LogAcao.Exclusao,
                "Pago",
                "false",
                "true",
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }

        public void CriarLogsDeExclusao(Transacao transacao,Venda vendaDaTransacao, Usuario usuarioResponsavel)
        {
            TransacaoLog log = new(transacao,
                vendaDaTransacao,
                LogAcao.Exclusao,
                "Ativo",
                "true",
                "false",
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }
    }
}
