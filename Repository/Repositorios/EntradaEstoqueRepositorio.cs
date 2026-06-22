using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Repository.Repositorios
{
    public class EntradaEstoqueRepositorio
    {
        private ContextoDb _contexto;

        public EntradaEstoqueRepositorio(ContextoDb contexto)
        {
            _contexto = contexto;
        }

        public List<EntradaEstoque> BuscarTodos()
        {
            return _contexto.EntradasEstoque.
            OrderBy(e=>e.Status== StatusEntradaEstoque.Criada? 1 : e.Status==StatusEntradaEstoque.Atualizada? 2 : 3)
            .Include(e=>e.Fornecedor)
            .ToList();
        }

        public EntradaEstoque? BuscarPorId(Guid id)
        {
            return _contexto.EntradasEstoque.Include(e => e.Fornecedor).FirstOrDefault(e => e.Id == id);
        }

        public EntradaEstoque Cadastrar(EntradaEstoque entidade)
        {
            _contexto.EntradasEstoque.Add(entidade);
            return entidade;    
        }

        // public EntradaEstoque Atualizar(EntradaEstoque entidade)
        // {
        //     entidade.Status=StatusEntradaEstoque.Atualizada;
        //     _contexto.EntradasEstoque.Update(entidade);
        //     _contexto.SaveChanges();
        //     return entidade;
        // }
        public void Inativar(EntradaEstoque entradaEstoque)
        {
            entradaEstoque.Ativo = false;
            entradaEstoque.Status=StatusEntradaEstoque.Cancelada;
            _contexto.EntradasEstoque.Update(entradaEstoque);
        }

    }
}
