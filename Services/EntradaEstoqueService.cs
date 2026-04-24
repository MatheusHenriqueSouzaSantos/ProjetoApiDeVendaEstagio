using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Services.Interfaces;

namespace ApiEstagioBicicletaria.Services
{
    public class EntradaEstoqueService : IEntradaEstoqueService
    {
        private EntradaEstoqueRepositorio _repositorio;

        public EntradaEstoqueService(EntradaEstoqueRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public List<EntradaEstoqueOutputDto> BuscarTodos()
        {
            List<EntradaEstoque> EntradasEstoque=_repositorio.BuscarTodos();
            List<EntradaEstoqueOutputDto> EntradasEstoqueDto = new List<EntradaEstoqueOutputDto>();

        }

        public EntradaEstoqueOutputDto BuscarPorId(Guid id)
        {
            throw new NotImplementedException();
        }


        public EntradaEstoqueOutputDto Cadastrar(EntradaEstoqueInputDto dto)
        {
            throw new NotImplementedException();
        }


        public EntradaEstoqueOutputDto Atualizar(Guid id, EntradaEstoqueInputDto dto)
        {
            throw new NotImplementedException();
        }

        public void InativarEntradaEstoque(Guid id)
        {
            throw new NotImplementedException();
        }


        public EntradaEstoqueOutputDto EntidadeParaDto(EntradaEstoque entradaEstoque,List<ItemEntradaEstoque> itens)
        {
            List<ItemEntradaEstoqueOutputDto> itensDto=new List<ItemEntradaEstoqueOutputDto> ();

            foreach (ItemEntradaEstoque item in itens)
            {
                Estoque estoqueDoItem = item.Estoque;
                EstoqueSimplificadoOutputDto estoqueSimplificado = new(estoqueDoItem.Id, 
                    estoqueDoItem.ProdutoId, estoqueDoItem.Produto.NomeProduto, 
                    estoqueDoItem.QuantidadeEmEstoque);
                ItemEntradaEstoqueOutputDto itemDto = new(item.Id, item.DataCriacao, 
                    item.Ativo, estoqueSimplificado, item.Quantidade);
                itensDto.Add(itemDto);
            }
            EntradaEstoqueOutputDto entradaEstoqueDto = new(entradaEstoque.Id, entradaEstoque.DataCriacao,
                entradaEstoque.Ativo, itensDto, entradaEstoque.Fornecedor, entradaEstoque.CodigoEntrada);

            return entradaEstoqueDto;
        }
    }
}
