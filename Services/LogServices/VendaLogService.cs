using ApiEstagioBicicletaria.Entities;
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
    }
}
