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
    }
}
