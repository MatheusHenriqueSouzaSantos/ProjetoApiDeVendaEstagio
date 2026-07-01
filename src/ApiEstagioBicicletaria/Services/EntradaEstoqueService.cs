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

        private readonly GeradorCodigoIndentificador<EntradaEstoque> _geradorCodigo;

        private readonly EntradaEstoqueLogService _entradaEstoqueLogService;

        private readonly ItemEntradaEstoqueLogService _itemEntradaEstoqueLogService;

        private readonly EstoqueLogService _estoqueLogService;

        private readonly Usuario _usuarioLogado;

        public EntradaEstoqueService(ContextoDb contexto, EntradaEstoqueRepositorio repositorio, 
            ItemEntradaEstoqueRepositorio itemEntradaRepositorio, FornecedorRepositorio fornecedorRepositorio, 
            ProdutoRepositorio produtoRepositorio, EstoqueRepositorio estoqueRepositorio, GeradorCodigoIndentificador<EntradaEstoque> geradorCodigo,
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

        public List<EntradaEstoqueOutputDto> BuscarEntradasAtivas()
        {
            List<EntradaEstoque> entradasEstoque = _contexto.EntradasEstoque.Where(e=>e.Ativo).Include(e => e.Fornecedor).OrderByDescending(e=>e.DataCriacao).ToList();
            List<EntradaEstoqueOutputDto> entradasEstoqueDto = new List<EntradaEstoqueOutputDto>();

            foreach(EntradaEstoque entradaEstoque in entradasEstoque)
            {
                List<ItemEntradaEstoque> itensEntradaEstoque = _contexto.ItensEntradaEstoque.Include(i => i.Produto)
                    .Where(i => i.IdEntradaEstoque == entradaEstoque.Id).ToList();
                EntradaEstoqueOutputDto entradaEstoqueDto = EntidadeParaDto(entradaEstoque, itensEntradaEstoque);
                entradasEstoqueDto.Add(entradaEstoqueDto);
            }

            return entradasEstoqueDto;

        }

        public List<EntradaEstoqueOutputDto> BuscarEntradasInativas()
        {
            List<EntradaEstoque> entradasEstoque = _contexto.EntradasEstoque.Where(e=>!e.Ativo).Include(e => e.Fornecedor).OrderByDescending(e=>e.DataCriacao).ToList();
            List<EntradaEstoqueOutputDto> entradasEstoqueDto = new List<EntradaEstoqueOutputDto>();

            foreach (EntradaEstoque entradaEstoque in entradasEstoque)
            {
                List<ItemEntradaEstoque> itensEntradaEstoque = _contexto.ItensEntradaEstoque.Include(i => i.Produto)
                    .Where(i => i.IdEntradaEstoque == entradaEstoque.Id).ToList();
                EntradaEstoqueOutputDto entradaEstoqueDto = EntidadeParaDto(entradaEstoque, itensEntradaEstoque);
                entradasEstoqueDto.Add(entradaEstoqueDto);
            }

            return entradasEstoqueDto;

        }
        //colocar o end point de buscar inativas..., ajustar com o front

        public EntradaEstoqueOutputDto BuscarEntradasAtivasPorId(Guid id)
        {
            EntradaEstoque entradaEstoque = _contexto.EntradasEstoque.FirstOrDefault(e=>e.Id==id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada Estoque não Encontrada");
            List<ItemEntradaEstoque> itensEntradaEstoque=_contexto.ItensEntradaEstoque.Where(i=>i.IdEntradaEstoque==entradaEstoque.Id).ToList();

            return EntidadeParaDto(entradaEstoque, itensEntradaEstoque);
        }


        public EntradaEstoqueOutputDto Cadastrar(EntradaEstoqueCreateDto dto)
        {
            Fornecedor fornecedor=_contexto.Fornecedores.FirstOrDefault(f=>f.Id==dto.IdFornecedor && f.Ativo)
            ?? throw new ExcecaoDeRegraDeNegocio(404,"Fornecedor não encontrado ou inativo");

            EntradaEstoque entradaEstoque = new(fornecedor, _geradorCodigo.GerarCodigoMovimentacao(),StatusEntradaEstoque.Criada);

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
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada Estoque não encontrada ou inativa");

            EntradaEstoque entradaEstoqueCopia = entradaEstoque.Copia(); 

            List<ItemEntradaEstoque> itensEntrada=_contexto.ItensEntradaEstoque.Include(i=>i.EntradaEstoque).Include(i=>i.Produto).
                Where(i=>i.IdEntradaEstoque==entradaEstoque.Id && i.Ativo).ToList();

            if (dto.IdFornecedor != null)
            {
                Fornecedor fornecedorDaEntradaAtualizado = _contexto.Fornecedores.FirstOrDefault(f => f.Id == dto.IdFornecedor && f.Ativo)
                    ?? throw new ExcecaoDeRegraDeNegocio(404, "Fornecedor não encontrado ou inativo");
                entradaEstoque.Fornecedor=fornecedorDaEntradaAtualizado;
                entradaEstoque.IdFornecedor = fornecedorDaEntradaAtualizado.Id;
            }

            if (dto.IdsItensASeremDeletados != null)
            {
                foreach(Guid idASerDeletado in dto.IdsItensASeremDeletados)
                {
                    ItemEntradaEstoque itemASerDeletado = itensEntrada.FirstOrDefault(i => i.Id == idASerDeletado && i.Ativo)
                        ?? throw new ExcecaoDeRegraDeNegocio(404, $"nenhuma item encontrado com esse id: {idASerDeletado}");
                    Produto produtoDoItem = _contexto.Produtos.FirstOrDefault(p => p.Id == itemASerDeletado.IdProduto)
                        ?? throw new ExcecaoDeRegraDeNegocio(500, "produto não encontrado");

                    Estoque estoqueDoItem = _contexto.Estoques.Include(e=>e.Produto).First(e => e.ProdutoId == produtoDoItem.Id);

                    if (estoqueDoItem.QuantidadeEmEstoque - itemASerDeletado.Quantidade < 0)
                    {
                        throw new ExcecaoDeRegraDeNegocio(400, $"não é possível excluir o item de entrada com id:{itemASerDeletado.Id}, pois se não o estoque ficaria negativo");
                    }
                    int quantidadeEmEstoqueAntiga = estoqueDoItem.QuantidadeEmEstoque;
                    estoqueDoItem.AbaterQuantidadeEmEstoque(itemASerDeletado.Quantidade);
                    _contexto.Estoques.Update(estoqueDoItem);
                    _estoqueLogService.CriarLogDeAtualizacaoQuantidadeEmEstoque(estoqueDoItem, produtoDoItem, quantidadeEmEstoqueAntiga, estoqueDoItem.QuantidadeEmEstoque,
                        AcaoQueAlterouEstoque.AtualizacaoEntradaEstoque, _usuarioLogado);
                    itemASerDeletado.Ativo = false;
                    itensEntrada.Remove(itemASerDeletado);
                    _itemEntradaEstoqueLogService.CriarLogsDeExclusao(itemASerDeletado, entradaEstoque, _usuarioLogado);
                    _contexto.ItensEntradaEstoque.Remove(itemASerDeletado);
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
                    Produto produtoDoItem = _contexto.Produtos.FirstOrDefault(p => p.Id == itemASerAtualizado.IdProduto)
                        ?? throw new ExcecaoDeRegraDeNegocio(500, "produto não encontrado");
                    if (!produtoDoItem.Ativo)
                    {
                        throw new ExcecaoDeRegraDeNegocio(400, $"não é possível alterar esse item, pois o produto com id: {produtoDoItem.Id} está inativo, exclua este item");
                    }
                    Estoque estoqueDoItem = _contexto.Estoques.Include(e=>e.Produto).First(e => e.ProdutoId==produtoDoItem.Id);
                    int quantidadeEmEstoqueAntiga = estoqueDoItem.QuantidadeEmEstoque;
                    estoqueDoItem.AbaterQuantidadeEmEstoque(itemASerAtualizado.Quantidade);
                    estoqueDoItem.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                    if (estoqueDoItem.QuantidadeEmEstoque<0)
                    {
                        throw new ExcecaoDeRegraDeNegocio(400, $"Estoque insufisciente para concluir a operação, do item com id: {itemASerAtualizado.Id} " +
                            $"pois há apenas: {estoqueDoItem.QuantidadeEmEstoque} unidades");
                    }
                    _contexto.Estoques.Update(estoqueDoItem);
                    _estoqueLogService.CriarLogDeAtualizacaoQuantidadeEmEstoque(estoqueDoItem, produtoDoItem, quantidadeEmEstoqueAntiga,
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
                        ?? throw new ExcecaoDeRegraDeNegocio(404, $"Nenhum produto encontrado com o id: {itemDto.IdProduto} ou ele está inativo");

                    Estoque estoqueDoProduto = _contexto.Estoques.First(e => e.ProdutoId == produtoDoItem.Id && e.Ativo);
                    int quantidadeEmEstoqueAntiga = estoqueDoProduto.QuantidadeEmEstoque;
                    estoqueDoProduto.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                    ItemEntradaEstoque itemCriado = new(entradaEstoque, produtoDoItem, itemDto.Quantidade);
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
                
                Estoque estoqueDoItem = _contexto.Estoques.Include(e=>e.Produto).FirstOrDefault(e => e.ProdutoId == item.IdProduto)
                    ?? throw new ExcecaoDeRegraDeNegocio(500,"estoque não encontrado");
                if(item.Quantidade> estoqueDoItem.QuantidadeEmEstoque)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, $"Não é possível cancelar essa entrada, pois o item com id: {item.Id} não pode ser excluído, " +
                        $"pois o estoque do produto {item.Produto.NomeProduto} não pode ficar negativo ");
                }
                int quantidadeEmEstoqueAntiga = estoqueDoItem.QuantidadeEmEstoque;
                estoqueDoItem.AbaterQuantidadeEmEstoque(item.Quantidade);
                _estoqueRepositorio.AtualizarEstoque(estoqueDoItem);
                _estoqueLogService.CriarLogDeAtualizacaoQuantidadeEmEstoque(estoqueDoItem, estoqueDoItem.Produto, quantidadeEmEstoqueAntiga,
                    estoqueDoItem.QuantidadeEmEstoque, AcaoQueAlterouEstoque.ExclusaoEntradaEstoque, _usuarioLogado);
                item.Ativo = false;
                _contexto.ItensEntradaEstoque.Update(item);
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
                Produto produtoDoItem=item.Produto;
                ItemEntradaEstoqueOutputDto itemDto = new(item.Id, item.DataCriacao, 
                    item.Ativo,produtoDoItem.Id,produtoDoItem.NomeProduto, item.Quantidade);
                itensDto.Add(itemDto);
            }
            EntradaEstoqueOutputDto entradaEstoqueDto = new(entradaEstoque.Id, entradaEstoque.DataCriacao,
                entradaEstoque.Ativo, itensDto, entradaEstoque.Fornecedor, entradaEstoque.CodigoEntrada,entradaEstoque.Status);

            return entradaEstoqueDto;
        }

        private List<ItemEntradaEstoque> CriarItensEntradaEstoque(List<ItemEntradaEstoqueCreateDto> dtos,EntradaEstoque entradaEstoque)
        {
            List<ItemEntradaEstoque> itens=new List<ItemEntradaEstoque>();
            foreach(ItemEntradaEstoqueCreateDto itemDto in dtos)
            {
                Produto produtoDoItem=_contexto.Produtos.FirstOrDefault(p=>p.Id==itemDto.IdProduto && p.Ativo)
                    ?? throw new ExcecaoDeRegraDeNegocio(400,$"Produto com id:{itemDto.IdProduto} não encontrado ou inativo");
                Estoque estoqueDoItem=_contexto.Estoques.First(e=>e.ProdutoId==produtoDoItem.Id);
                int quantidadeAnteriorDoEstoque = estoqueDoItem.QuantidadeEmEstoque;
                ItemEntradaEstoque item = new(entradaEstoque, produtoDoItem, itemDto.Quantidade);
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
            EntradaEstoque entradaEstoque = _contexto.EntradasEstoque.FirstOrDefault(e => e.Id == idEntrada)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada estoque não encontrada");

            List<EntradaEstoqueLogOutputDto> logsEntradaDto=_contexto.EntradasEstoqueLogs.Where(l=>l.IdEntradaEstoque==entradaEstoque.Id)
                .Select(l=>new EntradaEstoqueLogOutputDto(l.IdEntradaEstoque,l.Acao,l.CampoAlterado,l.ValorAntigo,l.ValorNovo,l.IdUsuarioResponsavel,l.DataCriacao)).ToList();

            List<ItemEntradaEstoqueLogOutputDto> logsItemEntradaDto = _contexto.ItensEntradaEstoqueLogs
                .Include(l => l.ItemEntradaEstoque).ThenInclude(i => i.Produto)
                .Where(l => l.IdEntradaEstoque == entradaEstoque.Id)
                .Select(l => new ItemEntradaEstoqueLogOutputDto(l.IdItemEntradaEstoque, l.ItemEntradaEstoque.IdProduto,
                l.ItemEntradaEstoque.Produto.NomeProduto, l.Acao, l.CampoAlterado, l.ValorAntigo, l.ValorNovo, l.IdUsuarioResponsavel, l.DataCriacao)).ToList();

            List<BaseLogOutputDto> logsEntradaEItensDto=new List<BaseLogOutputDto>();

            logsEntradaEItensDto.AddRange(logsEntradaDto);
            logsEntradaEItensDto.AddRange(logsItemEntradaDto);

            return logsEntradaEItensDto.
                OrderByDescending(l=>l.DataCriacao).
                Cast<Object>().
                ToList();
        }

        public List<Object> BuscarLogsPorCodigoEntrada(string codigoEntrada)
        {
            EntradaEstoque entradaEstoque=_contexto.EntradasEstoque.FirstOrDefault(e=>e.CodigoEntrada==codigoEntrada)
                ?? throw new ExcecaoDeRegraDeNegocio(404,"Entrada estoque não encontrada");

            List<EntradaEstoqueLogOutputDto> logsEntradaDto = _contexto.EntradasEstoqueLogs.Where(l => l.IdEntradaEstoque == entradaEstoque.Id)
                .Select(l => new EntradaEstoqueLogOutputDto(l.IdEntradaEstoque, l.Acao, l.CampoAlterado, l.ValorAntigo, l.ValorNovo, l.IdUsuarioResponsavel, l.DataCriacao)).ToList();

            List<ItemEntradaEstoqueLogOutputDto> logsItemEntradaDto = _contexto.ItensEntradaEstoqueLogs
                .Include(l => l.ItemEntradaEstoque).ThenInclude(i => i.Produto)
                .Where(l => l.IdEntradaEstoque == entradaEstoque.Id)
                .Select(l => new ItemEntradaEstoqueLogOutputDto(l.IdItemEntradaEstoque, l.ItemEntradaEstoque.IdProduto,
                l.ItemEntradaEstoque.Produto.NomeProduto, l.Acao, l.CampoAlterado, l.ValorAntigo, l.ValorNovo, l.IdUsuarioResponsavel, l.DataCriacao)).ToList();

            List<BaseLogOutputDto> logsEntradaEItensDto = new List<BaseLogOutputDto>();

            logsEntradaEItensDto.AddRange(logsEntradaDto);
            logsEntradaEItensDto.AddRange(logsItemEntradaDto);

            return logsEntradaEItensDto.
                OrderByDescending(l => l.DataCriacao).
                Cast<Object>().
                ToList();
        }

    }

        // public void DeletarItensEntradaEstoque(Guid idEntradaEstoqueDosItens)
        // {

        // }

}
