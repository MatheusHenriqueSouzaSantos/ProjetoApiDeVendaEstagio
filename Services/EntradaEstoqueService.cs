using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Utils;

namespace ApiEstagioBicicletaria.Services
{
    public class EntradaEstoqueService : IEntradaEstoqueService
    {
        private readonly EntradaEstoqueRepositorio _repositorio;

        private readonly ItemEntradaEstoqueRepositorio _itemEntradaRepositorio;

        private readonly FornecedorRepositorio _fornecedorRepositorio;

        private readonly ProdutoRepositorio _produtoRepositorio;

        private readonly EstoqueRepositorio _estoqueRepositorio;

        private readonly GeradorCodigoIndentificadorMovimentacao<EntradaEstoque> geradorCodigo;

        public EntradaEstoqueService(EntradaEstoqueRepositorio repositorio, 
            ItemEntradaEstoqueRepositorio itemEntradaRepositorio, FornecedorRepositorio fornecedorRepositorio, 
            ProdutoRepositorio produtoRepositorio, EstoqueRepositorio estoqueRepositorio, 
            GeradorCodigoIndentificadorMovimentacao<EntradaEstoque> geradorCodigo)
        {
            _repositorio = repositorio;
            _itemEntradaRepositorio = itemEntradaRepositorio;
            _fornecedorRepositorio = fornecedorRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _estoqueRepositorio = estoqueRepositorio;
            this.geradorCodigo = geradorCodigo;
        }

        public List<EntradaEstoqueOutputDto> BuscarTodos()
        {
            List<EntradaEstoque> entradasEstoque=_repositorio.BuscarTodos();
            List<EntradaEstoqueOutputDto> entradasEstoqueDto = new List<EntradaEstoqueOutputDto>();

            foreach(EntradaEstoque entradaEstoque in entradasEstoque)
            {
                List<ItemEntradaEstoque> itensEntradaEstoque = _itemEntradaRepositorio
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
            List<ItemEntradaEstoque> itensEntradaEstoque=_itemEntradaRepositorio.BuscarItensPorIdEntradaEstoque(entradaEstoque.Id);

            return EntidadeParaDto(entradaEstoque, itensEntradaEstoque);
        }


        public EntradaEstoqueOutputDto Cadastrar(EntradaEstoqueInputDto dto)
        {
            Fornecedor fornecedor=_fornecedorRepositorio.BuscarFornecedorPorId(dto.IdFornecedor)
            ?? throw new ExcecaoDeRegraDeNegocio(404,"Fornecedor nao encontrado");

            EntradaEstoque entradaEstoque = new(fornecedor, geradorCodigo.GerarCodigo());

            List<ItemEntradaEstoque> itens=new List<ItemEntradaEstoque>();
            foreach(ItemEntradaEstoqueInputDto itemDto in dto.Itens)
            {
                if (!_produtoRepositorio.VerificarSeProdutoExistePorId(itemDto.IdProduto))
                {
                    throw new ExcecaoDeRegraDeNegocio(404,$"produto com id: {itemDto.IdProduto} não existe");
                }
                Estoque estoqueDoItem=_estoqueRepositorio.BuscarEstoquePorProdutoId(itemDto.IdProduto)
                ?? throw new ExcecaoDeRegraDeNegocio(500,"Estoque do produto não encontrado");
                ItemEntradaEstoque item = new(entradaEstoque, estoqueDoItem, itemDto.Quantidade);
                _itemEntradaRepositorio.Cadastrar(item);
                estoqueDoItem.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
            }
            _repositorio.Cadastrar(entradaEstoque);
        }


        public EntradaEstoqueOutputDto Atualizar(Guid id, EntradaEstoqueInputDto dto)
        {
            throw new NotImplementedException();
        }

        public void InativarEntradaEstoque(Guid id)
        {
            EntradaEstoque entrada = _repositorio.BuscarPorId(id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada de estoque não encontrada");
            List<ItemEntradaEstoque> itensEntradaEstoque = _itemEntradaRepositorio
                .BuscarItensPorIdEntradaEstoque(entrada.Id);
            foreach(ItemEntradaEstoque item in itensEntradaEstoque)
            {
                _itemEntradaRepositorio.InativarItem(item);
            }

            _repositorio.Inativar(entrada);
    
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
