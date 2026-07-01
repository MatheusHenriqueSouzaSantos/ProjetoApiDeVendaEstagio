using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices.InterfacesLog
{
    public interface IVendedorLogService
    {
        void CriarLogsDeCriacao(Vendedor vendedor, Usuario usuarioResponsavel);
        void CriarLogsDeAtualizacao(Vendedor vendedorAntigo, Vendedor vendedorAtualizado, Usuario usuarioResponsavel);

        void CriarLogsDeInativacao(Vendedor vendedor, Usuario usuarioResponsavel);

        void CriarLogsDeReativacao(Vendedor vendedor, Usuario usuarioResponsavel);
           
        
    }

}
