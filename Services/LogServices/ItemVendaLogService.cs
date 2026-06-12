using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class ItemVendaLogService
    {
        private readonly LogRepositorio<ItemVendaLog> _repositorio;

        public ItemVendaLogService(LogRepositorio<ItemVendaLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(ItemVenda itemVenda,Venda venda, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ItemVenda).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(itemVenda);

                ItemVendaLog log = new(itemVenda,
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
