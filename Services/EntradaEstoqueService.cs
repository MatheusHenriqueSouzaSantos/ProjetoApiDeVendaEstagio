using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Utils;
//code review
namespace ApiEstagioBicicletaria.Services
{
    public class EntradaEstoqueService : IEntradaEstoqueService
    {
        private readonly ContextoDb _contexto;

        private readonly EntradaEstoqueRepositorio _repositorio;

        private readonly ItemEntradaEstoqueRepositorio _itemEntradaRepositorio;

        private readonly FornecedorRepositorio _fornecedorRepositorio;

        private readonly ProdutoRepositorio _produtoRepositorio;

        private readonly EstoqueRepositorio _estoqueRepositorio;

        private readonly GeradorCodigoIndentificadorMovimentacao<EntradaEstoque> _geradorCodigo;

        public EntradaEstoqueService(ContextoDb contexto, EntradaEstoqueRepositorio repositorio, 
            ItemEntradaEstoqueRepositorio itemEntradaRepositorio, FornecedorRepositorio fornecedorRepositorio, 
            ProdutoRepositorio produtoRepositorio, EstoqueRepositorio estoqueRepositorio, GeradorCodigoIndentificadorMovimentacao<EntradaEstoque> geradorCodigo)
        {
            _contexto = contexto;
            _repositorio = repositorio;
            _itemEntradaRepositorio = itemEntradaRepositorio;
            _fornecedorRepositorio = fornecedorRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _estoqueRepositorio = estoqueRepositorio;
            _geradorCodigo = geradorCodigo;
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

            EntradaEstoque entradaEstoque = new(fornecedor, _geradorCodigo.GerarCodigo());

            List<ItemEntradaEstoque> itens=CriarItensEntradaEstoque(dto.Itens,entradaEstoque);
          
            _repositorio.Cadastrar(entradaEstoque);
            _contexto.SaveChanges();
            return EntidadeParaDto(entradaEstoque,itens);
        }


        // public EntradaEstoqueOutputDto Atualizar(Guid id, EntradaEstoqueInputDto dto)
        // {
        //     EntradaEstoque entradaEstoque=_repositorio.BuscarPorId(id)
        //     ?? throw new ExcecaoDeRegraDeNegocio(404,"Entrada estoque não encontrada");

        //     if (entradaEstoque.Status == StatusEntradaEstoque.Cancelada)
        //     {
        //         throw new ExcecaoDeRegraDeNegocio(400)
        //     }

        //     Fornecedor fornecedorAtualizado=_fornecedorRepositorio.BuscarFornecedorPorId(id)
        //     ?? throw new ExcecaoDeRegraDeNegocio(404,"Fornecedor não encontrado");

        //     List<ItemEntradaEstoque> itens=CriarItensEntradaEstoque(dto.Itens,entradaEstoque);

        //     //validar se já não foi cancelada?
            
        //     DeletarItensEntradaEstoque(entradaEstoque.Id)
        //     //vou permitir excluir fisicamente os itens da entrada mais vou criar logs, para registrar, 
        //     // uso delete lógico só quando é exclusão, para atualização uso o delete físico
        // }

        public void InativarEntradaEstoque(Guid id)
        {
            EntradaEstoque entrada = _repositorio.BuscarPorId(id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada de estoque não encontrada");
            List<ItemEntradaEstoque> itensEntradaEstoque = _itemEntradaRepositorio
                .BuscarItensPorIdEntradaEstoque(entrada.Id);
            foreach(ItemEntradaEstoque item in itensEntradaEstoque)
            {
                if(item.Quantidade> item.Estoque.QuantidadeEmEstoque)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Não é possível cancelar essa entrada, pois o estoque é insufisciente");
                }
                _itemEntradaRepositorio.InativarItem(item);
                item.Estoque.AbaterQuantidadeEmEstoque(item.Quantidade);
                _estoqueRepositorio.AtualizarEstoque(item.Estoque);
                _contexto.SaveChanges();
            }
            _repositorio.Inativar(entrada);
    
        }


        public EntradaEstoqueOutputDto EntidadeParaDto(EntradaEstoque entradaEstoque,List<ItemEntradaEstoque> itens)
        {
            List<ItemEntradaEstoqueOutputDto> itensDto=new List<ItemEntradaEstoqueOutputDto> ();

            foreach (ItemEntradaEstoque item in itens)
            {
                Produto produtoDoItem=item.Estoque.Produto;
                ItemEntradaEstoqueOutputDto itemDto = new(item.Id, item.DataCriacao, 
                    item.Ativo,produtoDoItem.Id,produtoDoItem.NomeProduto, item.Quantidade);
                itensDto.Add(itemDto);
            }
            EntradaEstoqueOutputDto entradaEstoqueDto = new(entradaEstoque.Id, entradaEstoque.DataCriacao,
                entradaEstoque.Ativo, itensDto, entradaEstoque.Fornecedor, entradaEstoque.CodigoEntrada,entradaEstoque.Status);

            return entradaEstoqueDto;
        }

        public List<ItemEntradaEstoque> CriarItensEntradaEstoque(List<ItemEntradaEstoqueInputDto> dtos,EntradaEstoque entradaEstoque)
        {
            List<ItemEntradaEstoque> itens=new List<ItemEntradaEstoque>();
            foreach(ItemEntradaEstoqueInputDto itemDto in dtos)
            {
                if (!_produtoRepositorio.VerificarSeProdutoExistePorId(itemDto.IdProduto))
                {
                    throw new ExcecaoDeRegraDeNegocio(404,$"produto com id: {itemDto.IdProduto} não existe");
                }
                Estoque estoqueDoItem=_estoqueRepositorio.BuscarEstoquePorProdutoId(itemDto.IdProduto)
                ?? throw new ExcecaoDeRegraDeNegocio(500,"Estoque do produto não encontrado");
                ItemEntradaEstoque item = new(entradaEstoque, estoqueDoItem, itemDto.Quantidade);
                _itemEntradaRepositorio.Cadastrar(item);
                itens.Add(item);
                estoqueDoItem.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                _estoqueRepositorio.AtualizarEstoque(estoqueDoItem);
            }
            return itens;
        }

        // public void DeletarItensEntradaEstoque(Guid idEntradaEstoqueDosItens)
        // {
            
        // }

        
    }
}
