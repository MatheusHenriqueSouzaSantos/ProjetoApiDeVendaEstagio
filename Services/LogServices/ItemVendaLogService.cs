using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
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
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
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

        public void CriarLogsDeAtualizacao(ItemVenda itemVendaAntigo, ItemVenda itemVendaAtualizado,Venda vendaDoItem, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ItemVenda).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(itemVendaAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(itemVendaAtualizado);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    ItemVendaLog log = new(
                        itemVendaAtualizado,
                        vendaDoItem,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(ItemVenda itemVenda, Venda venda, Usuario usuarioResponsavel)
        {
            ItemVendaLog log = new(itemVenda,
                venda,
                LogAcao.Inativacao,
                "Ativo",
                "true",
                "false",
                usuarioResponsavel);

            _repositorio.CriarLog(log);
            
        }
    }
}
