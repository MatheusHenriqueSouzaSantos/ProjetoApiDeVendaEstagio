using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Input;
using ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Output;
using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using ApiEstagioBicicletaria.Utils;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;
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

        private readonly EntradaEstoqueLogService _entradaEstoqueLogService;

        private readonly ItemEntradaEstoqueLogService _itemEntradaEstoqueLogService;

        private readonly EstoqueLogService _estoqueLogService;

        private readonly Usuario _usuarioLogado;

        public EntradaEstoqueService(ContextoDb contexto, EntradaEstoqueRepositorio repositorio, 
            ItemEntradaEstoqueRepositorio itemEntradaRepositorio, FornecedorRepositorio fornecedorRepositorio, 
            ProdutoRepositorio produtoRepositorio, EstoqueRepositorio estoqueRepositorio, GeradorCodigoIndentificadorMovimentacao<EntradaEstoque> geradorCodigo,
            EntradaEstoqueLogService entradaEstoqueLogService,ItemEntradaEstoqueLogService itemEntradaEstoqueLogService,EstoqueLogService estoqueLogService
            ,UsuarioLogadoService usuarioLogadoService)
        {
            _contexto = contexto;
            _repositorio = repositorio;
            _itemEntradaRepositorio = itemEntradaRepositorio;
            _fornecedorRepositorio = fornecedorRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _estoqueRepositorio = estoqueRepositorio;
            _geradorCodigo = geradorCodigo;
            _entradaEstoqueLogService = entradaEstoqueLogService;
            _itemEntradaEstoqueLogService= itemEntradaEstoqueLogService;
            _estoqueLogService = estoqueLogService;
            _usuarioLogado = usuarioLogadoService.ObterUsuario();
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


        public EntradaEstoqueOutputDto Cadastrar(EntradaEstoqueCreateDto dto)
        {
            Fornecedor fornecedor=_fornecedorRepositorio.BuscarFornecedorPorId(dto.IdFornecedor)
            ?? throw new ExcecaoDeRegraDeNegocio(404,"Fornecedor nao encontrado");

            EntradaEstoque entradaEstoque = new(fornecedor, _geradorCodigo.GerarCodigo(),StatusEntradaEstoque.Criada);

            List<ItemEntradaEstoque> itens=CriarItensEntradaEstoque(dto.Itens,entradaEstoque);
          
            _repositorio.Cadastrar(entradaEstoque);
            _entradaEstoqueLogService.CriarLogsDeCriacao(entradaEstoque, _usuarioLogado);
            _contexto.SaveChanges();
            return EntidadeParaDto(entradaEstoque,itens);
        }

        //logs?


        public EntradaEstoqueOutputDto Atualizar(Guid id, EntradaEstoqueUpdateDto dto)
        {
            EntradaEstoque entradaEstoque = _contexto.EntradasEstoque.Include(e => e.Fornecedor).FirstOrDefault(e => e.Id == id && e.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada Estoque não encontrada");

            EntradaEstoque entradaEstoqueCopia = entradaEstoque.Copia(); 

            List<ItemEntradaEstoque> itensEntrada=_contexto.ItensEntradaEstoque.Where(i=>i.IdEntradaEstoque==entradaEstoque.Id && i.Ativo).ToList();

            if (dto.IdFornecedor != null)
            {
                Fornecedor fornecedorDaEntradaAtualizado = _contexto.Fornecedores.FirstOrDefault(f => f.Id == entradaEstoque.IdFornecedor && f.Ativo)
                    ?? throw new ExcecaoDeRegraDeNegocio(404, "Fornecedor não encontrado");
                entradaEstoque.Fornecedor=fornecedorDaEntradaAtualizado;
                entradaEstoque.IdFornecedor = fornecedorDaEntradaAtualizado.Id;
            }

            if (dto.IdsItensASeremDeletados != null)
            {
                foreach(Guid idASerDeletado in dto.IdsItensASeremDeletados)
                {
                    ItemEntradaEstoque itemASerDeletado = itensEntrada.FirstOrDefault(i => i.Id == idASerDeletado && i.Ativo)
                        ?? throw new ExcecaoDeRegraDeNegocio(404, $"nenhuma item encontrado com esse id: {idASerDeletado}");

                    Estoque estoqueDoItem = _contexto.Estoques.Include(e=>e.Produto).First(e => e.Id == itemASerDeletado.IdEstoque && e.Ativo);

                    if (estoqueDoItem.QuantidadeEmEstoque - itemASerDeletado.Quantidade < 0)
                    {
                        throw new ExcecaoDeRegraDeNegocio(400, $"não é possível excluir o item de entrada com id:{itemASerDeletado.Id}, pois se não o estoque ficaria negativo");
                    }
                    int quantidadeEmEstoqueAntiga = estoqueDoItem.QuantidadeEmEstoque;
                    estoqueDoItem.AbaterQuantidadeEmEstoque(itemASerDeletado.Quantidade);
                    _contexto.Estoques.Update(estoqueDoItem);
                    _estoqueLogService.CriarLogDeAtualizacaoQuantidadeEmEstoque(estoqueDoItem, estoqueDoItem.Produto, quantidadeEmEstoqueAntiga, estoqueDoItem.QuantidadeEmEstoque,
                        AcaoQueAlterouEstoque.AtualizacaoEntradaEstoque, _usuarioLogado);
                    itemASerDeletado.Ativo = false;
                    itensEntrada.Remove(itemASerDeletado);
                    _itemEntradaEstoqueLogService.CriarLogsDeExclusao(itemASerDeletado, entradaEstoque, _usuarioLogado);
                    _contexto.ItensEntradaEstoque.Update(itemASerDeletado);
                    //log
                }
                
            }
            if (dto.ItensAtualizados!= null)
            {
                foreach(ItemEntradaEstoqueUpdateDto itemDto in dto.ItensAtualizados)
                {
                    ItemEntradaEstoque itemASerAtualizado = itensEntrada.FirstOrDefault(i => i.Id == itemDto.IdDoItem && i.Ativo)
                        ?? throw new ExcecaoDeRegraDeNegocio(404, $"nenhum item encontrado com esse id: {itemDto.IdDoItem}");
                    ItemEntradaEstoque itemEntradaEstoqueCopia = itemASerAtualizado.Copia();
                    Estoque estoqueDoItem = _contexto.Estoques.Include(e=>e.Produto).First(e => e.Id == itemASerAtualizado.IdEstoque && e.Ativo);
                    int quantidadeEmEstoqueAntiga = estoqueDoItem.QuantidadeEmEstoque;
                    estoqueDoItem.AbaterQuantidadeEmEstoque(itemASerAtualizado.Quantidade);
                    estoqueDoItem.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                    if (estoqueDoItem.QuantidadeEmEstoque<0)
                    {
                        throw new ExcecaoDeRegraDeNegocio(400, $"Estoque insufisciente para concluir a operação, do item com id: {itemASerAtualizado.Id} " +
                            $"pois há apenas: {estoqueDoItem.QuantidadeEmEstoque} unidades");
                    }
                    _contexto.Estoques.Update(estoqueDoItem);
                    _estoqueLogService.CriarLogDeAtualizacaoQuantidadeEmEstoque(estoqueDoItem, estoqueDoItem.Produto, quantidadeEmEstoqueAntiga,
                        estoqueDoItem.QuantidadeEmEstoque, AcaoQueAlterouEstoque.AtualizacaoEntradaEstoque, _usuarioLogado);
                    itemASerAtualizado.Quantidade = itemDto.Quantidade;
                    _itemEntradaEstoqueLogService.CriarLogsDeAtualizacao(itemEntradaEstoqueCopia, itemASerAtualizado, entradaEstoque, _usuarioLogado);
                }
                _contexto.ItensEntradaEstoque.UpdateRange(itensEntrada);
            }
            if (dto.ItensNovos != null)
            {
                foreach(ItemEntradaEstoqueCreateDto itemDto in dto.ItensNovos)
                {
                    Produto produtoDoItem = _contexto.Produtos.FirstOrDefault(p => p.Id == itemDto.IdProduto && p.Ativo)
                        ?? throw new ExcecaoDeRegraDeNegocio(404, $"Nenhum produto encontrado com o id: {itemDto.IdProduto}");

                    Estoque estoqueDoProduto = _contexto.Estoques.First(e => e.ProdutoId == produtoDoItem.Id && produtoDoItem.Ativo);
                    int quantidadeEmEstoqueAntiga = estoqueDoProduto.QuantidadeEmEstoque;
                    estoqueDoProduto.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                    ItemEntradaEstoque itemCriado = new(entradaEstoque, estoqueDoProduto, itemDto.Quantidade);
                    _contexto.Estoques.Update(estoqueDoProduto);
                    _estoqueLogService.CriarLogDeAtualizacaoQuantidadeEmEstoque(estoqueDoProduto, produtoDoItem, quantidadeEmEstoqueAntiga, estoqueDoProduto.QuantidadeEmEstoque,
                        AcaoQueAlterouEstoque.AtualizacaoEntradaEstoque, _usuarioLogado);
                    itensEntrada.Add(itemCriado);
                    _itemEntradaEstoqueLogService.CriarLogsDeCriacao(itemCriado, entradaEstoque, _usuarioLogado);
                    _contexto.ItensEntradaEstoque.Add(itemCriado);
                }
            }
            entradaEstoque.Status = StatusEntradaEstoque.Atualizada;
            _entradaEstoqueLogService.CriarLogsDeAtualizacao(entradaEstoqueCopia, entradaEstoque, _usuarioLogado);
            _contexto.SaveChanges();
            return EntidadeParaDto(entradaEstoque, itensEntrada);

        }


        public void InativarEntradaEstoque(Guid id)
        {
            EntradaEstoque entrada = _repositorio.BuscarPorId(id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada de estoque não encontrada");
            StatusEntradaEstoque statusAntigo=entrada.Status;
            List<ItemEntradaEstoque> itensEntradaEstoque = _itemEntradaRepositorio
                .BuscarItensPorIdEntradaEstoque(entrada.Id);
            foreach(ItemEntradaEstoque item in itensEntradaEstoque)
            {
                if(item.Quantidade> item.Estoque.QuantidadeEmEstoque)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, $"Não é possível cancelar essa entrada, pois o item com id: {item.Id} não pode ser excluído, " +
                        $"pois o estoque do produto {item.Estoque.Produto.NomeProduto} não pode ficar negativo ");
                }
                Estoque estoqueDoItem = _contexto.Estoques.Include(e=>e.Produto).FirstOrDefault(e => e.Id == item.IdEstoque && e.Ativo)
                    ?? throw new ExcecaoDeRegraDeNegocio(500,"estoque não encontrado");
                int quantidadeEmEstoqueAntiga = estoqueDoItem.QuantidadeEmEstoque;
                estoqueDoItem.AbaterQuantidadeEmEstoque(item.Quantidade);
                _estoqueRepositorio.AtualizarEstoque(estoqueDoItem);
                _estoqueLogService.CriarLogDeAtualizacaoQuantidadeEmEstoque(estoqueDoItem, estoqueDoItem.Produto, quantidadeEmEstoqueAntiga,
                    estoqueDoItem.QuantidadeEmEstoque, AcaoQueAlterouEstoque.ExclusaoEntradaEstoque, _usuarioLogado);
                item.Ativo = false;
                _itemEntradaRepositorio.InativarItem(item);
                _itemEntradaEstoqueLogService.CriarLogsDeExclusao(item, entrada, _usuarioLogado);
                
            }
            entrada.Ativo = false;
            entrada.Status = StatusEntradaEstoque.Cancelada;
            _repositorio.Inativar(entrada);
            _entradaEstoqueLogService.CriarLogsDeExclusao(entrada, statusAntigo, _usuarioLogado);
            _contexto.SaveChanges();
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

        public List<ItemEntradaEstoque> CriarItensEntradaEstoque(List<ItemEntradaEstoqueCreateDto> dtos,EntradaEstoque entradaEstoque)
        {
            List<ItemEntradaEstoque> itens=new List<ItemEntradaEstoque>();
            foreach(ItemEntradaEstoqueCreateDto itemDto in dtos)
            {
                if (!_produtoRepositorio.VerificarSeProdutoExistePorId(itemDto.IdProduto))
                {
                    throw new ExcecaoDeRegraDeNegocio(404,$"produto com id: {itemDto.IdProduto} não existe");
                }
                Estoque estoqueDoItem=_estoqueRepositorio.BuscarEstoquePorProdutoId(itemDto.IdProduto)
                ?? throw new ExcecaoDeRegraDeNegocio(500,"Estoque do produto não encontrado");
                int quantidadeAnteriorDoEstoque = estoqueDoItem.QuantidadeEmEstoque;
                ItemEntradaEstoque item = new(entradaEstoque, estoqueDoItem, itemDto.Quantidade);
                _itemEntradaRepositorio.Cadastrar(item);
                _itemEntradaEstoqueLogService.CriarLogsDeCriacao(item, entradaEstoque, _usuarioLogado);
                itens.Add(item);
                estoqueDoItem.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                _estoqueLogService.CriarLogDeAtualizacaoQuantidadeEmEstoque(estoqueDoItem, estoqueDoItem.Produto, quantidadeAnteriorDoEstoque,
                    estoqueDoItem.QuantidadeEmEstoque, AcaoQueAlterouEstoque.CriacaoEntradaEstoque, _usuarioLogado);
                _estoqueRepositorio.AtualizarEstoque(estoqueDoItem);
            }
            return itens;
        }

        public List<Object> BuscarLogsPorIdEntradaEstoque(Guid idEntrada)
        {
            List<EntradaEstoqueLogOutputDto> logsEntradaDto=_contexto.EntradasEstoqueLogs.Where(l=>l.IdEntradaEstoque==idEntrada)
                .Select(l=>new EntradaEstoqueLogOutputDto(l.IdEntradaEstoque,l.Acao,l.CampoAlterado,l.ValorAntigo,l.ValorNovo,l.IdUsuarioResponsavel,l.DataCriacao)).ToList();

            List<ItemEntradaEstoqueLogOutputDto> logsItemEntradaDto = _contexto.ItensEntradaEstoqueLogs
                .Include(l => l.ItemEntradaEstoque).ThenInclude(i => i.Estoque).ThenInclude(e => e.Produto)
                .Where(l => l.IdEntradaEstoque == idEntrada)
                .Select(l => new ItemEntradaEstoqueLogOutputDto(l.IdItemEntradaEstoque, l.ItemEntradaEstoque.Estoque.Produto.Id,
                l.ItemEntradaEstoque.Estoque.Produto.NomeProduto, l.Acao, l.CampoAlterado, l.ValorAntigo, l.ValorNovo, l.IdUsuarioResponsavel, l.DataCriacao)).ToList();

            List<BaseLogOutputDto> logsEntradaEItensDto=new List<BaseLogOutputDto>();

            logsEntradaEItensDto.AddRange(logsEntradaDto);
            logsEntradaEItensDto.AddRange(logsItemEntradaDto);

            return logsEntradaEItensDto.
                OrderByDescending(l=>l.DataCriacao).
                Cast<Object>().
                ToList();
        }
        
    }

        // public void DeletarItensEntradaEstoque(Guid idEntradaEstoqueDosItens)
        // {

        // }

}
