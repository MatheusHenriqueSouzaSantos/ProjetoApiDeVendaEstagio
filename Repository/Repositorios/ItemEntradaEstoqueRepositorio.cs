using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Repositories;

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
            return _contexto.ItensEntradaEstoque
                .Where(i=>i.IdEntradaEstoque==idEntradaEstoque && i.Ativo).ToList();
        }
        public void Cadastrar(ItemEntradaEstoque item)
        {
            _contexto.Add(item);
            _contexto.SaveChanges();
        }
        public void InativarItem(ItemEntradaEstoque item)
        {
            item.Ativo = false;
            _contexto.ItensEntradaEstoque.Update(item);
            _contexto.SaveChanges();
        }
        
    }
}
