using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Repository.Repositorios
{
    public class ItemEntradaEstoqueRepositorio
    {
        private ContextoDb _contexto;

        public ItemEntradaEstoqueRepositorio(ContextoDb contexto)
        {
            _contexto = contexto;
        }

        public List<ItemEntradaEstoque> BuscarItensPorIdEntradaEstoque(Guid idEntradaEstoque)
        {
            return _contexto.ItensEntradaEstoque.Include(i=>i.Produto)
                .Where(i=>i.IdEntradaEstoque==idEntradaEstoque).ToList();
        }
        public void Cadastrar(ItemEntradaEstoque item)
        {
            _contexto.Add(item);
        }
        public void InativarItem(ItemEntradaEstoque item)
        {
            _contexto.ItensEntradaEstoque.Update(item);
        }
        
    }
}
