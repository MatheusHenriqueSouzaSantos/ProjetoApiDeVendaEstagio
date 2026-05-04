using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.EntityFrameworkCore;

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
            return _contexto.Estoques.Include(e=>e.Produto).FirstOrDefault(e=>e.ProdutoId == idProduto && e.Ativo);
        }

        public void AtualizarEstoque(Estoque estoque)
        {
            _contexto.Estoques.Update(estoque);
        }
}
