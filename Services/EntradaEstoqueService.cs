using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Services.Interfaces;

namespace ApiEstagioBicicletaria.Services
{
    public class EntradaEstoqueService : IEntradaEstoqueService
    {
        private EntradaEstoqueRepositorio _repositorio;

        private ItemEntradaEstoqueRepositorio _ItemEntradaRepositorio;

        public EntradaEstoqueService(EntradaEstoqueRepositorio repositorio,
            ItemEntradaEstoqueRepositorio itemEntradaRepositorio)
        {
            _repositorio = repositorio;
            _ItemEntradaRepositorio = itemEntradaRepositorio;
        }

        public List<EntradaEstoqueOutputDto> BuscarTodos()
        {
            List<EntradaEstoque> entradasEstoque=_repositorio.BuscarTodos();
            List<EntradaEstoqueOutputDto> entradasEstoqueDto = new List<EntradaEstoqueOutputDto>();

            foreach(EntradaEstoque entradaEstoque in entradasEstoque)
            {
                List<ItemEntradaEstoque> itensEntradaEstoque = _ItemEntradaRepositorio
                    .BuscarItensPorIdEntradaEstoque(entradaEstoque.Id);
                EntradaEstoqueOutputDto entradaEstoqueDto = EntidadeParaDto(entradaEstoque, itensEntradaEstoque);
                entradasEstoqueDto.Add(entradaEstoqueDto);
            }

            return entradasEstoqueDto;

        }

        public EntradaEstoqueOutputDto BuscarPorId(Guid id)
        {
            EntradaEstoque entradaEstoque = _repositorio.BuscarPorId(id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada Estoque não Encontrada");
            List<ItemEntradaEstoque> itensEntradaEstoque=_ItemEntradaRepositorio.BuscarItensPorIdEntradaEstoque(entradaEstoque.Id);

            return EntidadeParaDto(entradaEstoque, itensEntradaEstoque);
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
