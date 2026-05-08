using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Repository.Repositorios
{
    public class VendedorRepositorio
    {
        private readonly DbSet<Vendedor> _vendedores;

        public VendedorRepositorio(ContextoDb contexto)
        {
            _vendedores = contexto.Vendedores;
        }

        public Vendedor? BuscarPorId(Guid id)
        {
            return _vendedores.FirstOrDefault(v => v.Id == id && v.Ativo);
        }
    }
}
