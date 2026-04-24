using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Repositories;

namespace ApiEstagioBicicletaria;

public class EstoqueRepositorio
{
     private ContextoDb _contexto;

        public EstoqueRepositorio(ContextoDb contexto)
        {
            _contexto = contexto;
        }

        public Estoque? BuscarEstoquePorProdutoId(Guid idProduto)
        {
            return _contexto.Estoques.FirstOrDefault(e=>e.ProdutoId == idProduto && e.Ativo);
        }
}
