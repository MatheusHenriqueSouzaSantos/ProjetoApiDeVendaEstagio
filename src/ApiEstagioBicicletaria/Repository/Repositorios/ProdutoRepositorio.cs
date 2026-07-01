using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Repositories;

namespace ApiEstagioBicicletaria.Repository.Repositorios
{
    public class ProdutoRepositorio
    {
        private ContextoDb _contexto;

        public ProdutoRepositorio(ContextoDb contexto)
        {
            _contexto = contexto;
        }

        public bool VerificarSeProdutoExistePorId(Guid id)
        {
            return _contexto.Produtos.Any(p=>p.Id==id && p.Ativo);
        }
    }
}