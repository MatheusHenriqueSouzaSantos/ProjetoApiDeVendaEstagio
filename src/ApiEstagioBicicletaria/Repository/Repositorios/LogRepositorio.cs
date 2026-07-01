using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Repository.Repositorios
{
    public class LogRepositorio<T> where T: class
    {
        private readonly ContextoDb _contexto;

        public LogRepositorio(ContextoDb contexto)
        {
            _contexto = contexto;
        }

        public void CriarLog(T entidade)
        {
            _contexto.Set<T>().Add(entidade);
        }
    }
}
