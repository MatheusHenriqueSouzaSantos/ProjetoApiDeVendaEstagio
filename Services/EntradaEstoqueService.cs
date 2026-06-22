using ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Input;
using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
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


        public EntradaEstoqueOutputDto Cadastrar(EntradaEstoqueCreateDto dto)
        {
            Fornecedor fornecedor=_fornecedorRepositorio.BuscarFornecedorPorId(dto.IdFornecedor)
            ?? throw new ExcecaoDeRegraDeNegocio(404,"Fornecedor nao encontrado");

            EntradaEstoque entradaEstoque = new(fornecedor, _geradorCodigo.GerarCodigo());

            List<ItemEntradaEstoque> itens=CriarItensEntradaEstoque(dto.Itens,entradaEstoque);
          
            _repositorio.Cadastrar(entradaEstoque);
            _contexto.SaveChanges();
            return EntidadeParaDto(entradaEstoque,itens);
        }

        //logs?


        public EntradaEstoqueOutputDto Atualizar(Guid id, EntradaEstoqueUpdateDto dto)
        {
            EntradaEstoque entradaEstoque = _contexto.EntradasEstoque.Include(e => e.Fornecedor).FirstOrDefault(e => e.Id == id && e.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Entrada Estoque não encontrada");

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

                    Estoque estoqueDoItem = _contexto.Estoques.First(e => e.Id == itemASerDeletado.IdEstoque && e.Ativo);

                    estoqueDoItem.AdicionarQuantidadeEmEstoque(itemASerDeletado.Quantidade);
                    _contexto.Estoques.Update(estoqueDoItem);
                    itemASerDeletado.Ativo = false;
                    itensEntrada.Remove(itemASerDeletado);
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
                    Estoque estoqueDoItem = _contexto.Estoques.First(e => e.Id == itemASerAtualizado.IdEstoque && e.Ativo);
                    estoqueDoItem.AbaterQuantidadeEmEstoque(itemASerAtualizado.Quantidade);
                    estoqueDoItem.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                    if (estoqueDoItem.QuantidadeEmEstoque<0)
                    {
                        throw new ExcecaoDeRegraDeNegocio(400, $"Estoque insufisciente para concluir a operação, " +
                            $"pois há apenas: {estoqueDoItem.QuantidadeEmEstoque} unidades");
                    }
                    _contexto.Estoques.Update(estoqueDoItem);
                    //log
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

                    estoqueDoProduto.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                    ItemEntradaEstoque itemCriado = new(entradaEstoque, estoqueDoProduto, itemDto.Quantidade);
                    _contexto.Estoques.Update(estoqueDoProduto);
                    itensEntrada.Add(itemCriado);   
                    _contexto.ItensEntradaEstoque.Add(itemCriado);
                }
            }
            entradaEstoque.Status = StatusEntradaEstoque.Atualizada;
            _contexto.SaveChanges();
            return EntidadeParaDto(entradaEstoque, itensEntrada);

        }


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
                
            }
            _repositorio.Inativar(entrada);
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
                ItemEntradaEstoque item = new(entradaEstoque, estoqueDoItem, itemDto.Quantidade);
                _itemEntradaRepositorio.Cadastrar(item);
                itens.Add(item);
                estoqueDoItem.AdicionarQuantidadeEmEstoque(itemDto.Quantidade);
                _estoqueRepositorio.AtualizarEstoque(estoqueDoItem);
            }
            return itens;
        }

        
    }

        // public void DeletarItensEntradaEstoque(Guid idEntradaEstoqueDosItens)
        // {

        // }

}
