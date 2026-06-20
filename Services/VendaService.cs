using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using ApiEstagioBicicletaria.Utils;
using ApiEstagioBicicletaria.Validacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace ApiEstagioBicicletaria.Services
{
    public class VendaService : IVendaService
    {
        //private readonly int _numeroMaximoDePaginas = 5;
        //private readonly int _numeroDeLinhasPorPagina = 42;
        private ContextoDb _contexto;
        private readonly GeradorCodigoIndentificadorMovimentacao<Venda> _geradorCodigoVenda;
        private readonly IEstoqueService _estoqueService;
        private readonly VendedorRepositorio _vendedorRepositorio;
        private readonly VendaLogService _vendaLogService;
        private readonly ItemVendaLogService _itemVendaLogService;
        private readonly ServicoVendaLogService _servicoVendaLogService;
        private readonly Usuario _usuarioLogado;

        public VendaService(ContextoDb contexto, GeradorCodigoIndentificadorMovimentacao<Venda> geradorCodigoVenda, 
            IEstoqueService estoqueService, VendedorRepositorio vendedorRepositorio, VendaLogService vendaLogService,
            ServicoVendaLogService servicoVendaLogService,ItemVendaLogService itemVendaLogService,
            UsuarioLogadoService usuarioLogadoService)
        {
            _contexto = contexto;
            _geradorCodigoVenda = geradorCodigoVenda;
            _estoqueService = estoqueService;
            _vendedorRepositorio = vendedorRepositorio;
            _vendaLogService = vendaLogService;
            _itemVendaLogService = itemVendaLogService;
            _servicoVendaLogService = servicoVendaLogService;
            _usuarioLogado = usuarioLogadoService.ObterUsuario();
        }

        public List<VendaTransacaoOutputDto> BuscarTodasVendas()
        {
            List<Venda> vendas = _contexto.Vendas.Include(v => v.Vendedor).Include(v => v.Cliente).ThenInclude(c=>c.Endereco).Where(v=>v.Ativo).ToList();

            List<VendaTransacaoOutputDto> vendaTransacaoDtos=new List<VendaTransacaoOutputDto>();

            foreach(Venda venda in vendas)
            {
                VendaTransacaoOutputDto vendaTransacaoOutputDto= EntityToDto(venda);
                vendaTransacaoDtos.Add(vendaTransacaoOutputDto); 
            }
            return vendaTransacaoDtos.OrderBy(vt => !vt.Transacao.TransacaoEmCurso ? 1 : vt.Transacao.TransacaoEmCurso && !vt.Transacao.Pago ? 2 : 3)
                .ThenBy(vt=>vt.Venda.DataCriacao).ToList();
             
        }
        
        public VendaTransacaoOutputDto BuscarVendaPorId(Guid id)
        {
            Venda venda= _contexto.Vendas.Include(v => v.Vendedor).Include(v => v.Cliente).ThenInclude(c => c.Endereco).FirstOrDefault(v => v.Id == id && v.Ativo)
             ?? throw new ExcecaoDeRegraDeNegocio(404, "Venda não encontrada!!!");

            return EntityToDto(venda);
        }

        public VendaTransacaoOutputDto CadastrarVenda(VendaTransacaoCreateDto dto)
        {
            Cliente? clienteDaVenda = _contexto.Clientes.Where(c => c.Id == dto.Venda.IdCliente && c.Ativo).Include(c => c.Endereco).FirstOrDefault()
            ?? throw new ExcecaoDeRegraDeNegocio(404, "Cliente não encontrado!!!");

            Vendedor vendedorDaVenda = _vendedorRepositorio.BuscarPorId(dto.Venda.VendedorId)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Vendedor não encontrado");

            Console.WriteLine(vendedorDaVenda.NomeCompleto);

            List<ItemVendaCreateDto> itensVenda = dto.Venda.ItensVenda;
            List<ServicoVendaCreateDto> servicosVenda = dto.Venda.ServicosVenda;
            decimal valorTotalDaVendaSemDescontoTotalAplicado=CalcularTotalVendaSemDescontoTotalAplicadoParaVendaCriada(itensVenda,servicosVenda);
            decimal descontoVenda = dto.Venda.DescontoSobreTotalVenda ?? 0.0m;
            if (descontoVenda < 0)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"O desconto não pode ser negativo");
            }

            if(descontoVenda> valorTotalDaVendaSemDescontoTotalAplicado)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O desconto não pode ser maior que o total da venda");
            }

            if(itensVenda.Count<1 && servicosVenda.Count < 1)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"A venda deve conter pelo menos um item ou serviço");
            }
            if (dto.Transacao.QuantidadeDeParcelas > 60)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A Quantidade De Parcelas não deve ser maior que 60");
            }

            decimal valorTotalDaVendaComDescontoAplicado=Math.Round((valorTotalDaVendaSemDescontoTotalAplicado-descontoVenda),2,MidpointRounding.AwayFromZero);

            string codigoVenda = _geradorCodigoVenda.GerarCodigo();

            Venda vendaCriada = new Venda(codigoVenda, clienteDaVenda, descontoVenda,valorTotalDaVendaSemDescontoTotalAplicado, valorTotalDaVendaComDescontoAplicado,vendedorDaVenda);

            _vendaLogService.CriarLogsDeCriacao(vendaCriada, _usuarioLogado);

            List<ItemVenda> listaDeItensDaVendaCriada = new List<ItemVenda>();

            List<ServicoVenda> listaDeServicosDaVendaCriada =new List<ServicoVenda>();

            foreach(ItemVendaCreateDto itemEnviado in dto.Venda.ItensVenda.ToList())
            {
                Produto? produtoDoItem = _contexto.Produtos.FirstOrDefault(p => p.Id == itemEnviado.IdProduto && p.Ativo);

                if(produtoDoItem == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado!!!");
                }
                Estoque estoqueDoProdutoDoItem = _contexto.Estoques.FirstOrDefault(e => e.Produto.Id == produtoDoItem.Id && e.Ativo)
                    ?? throw new ExcecaoDeRegraDeNegocio(500, "Estoque não encontrado"); 
                if (itemEnviado.Quantidade == 0)
                {
                    throw new ExcecaoDeRegraDeNegocio(400,"Não é possível adicionar um produto com 0 unidades");
                }
                if(estoqueDoProdutoDoItem.QuantidadeEmEstoque< itemEnviado.Quantidade)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Estoque do produto: " + produtoDoItem.NomeProduto + " insuficiente, pois tem apenas: " +
                        estoqueDoProdutoDoItem.QuantidadeEmEstoque + " unidades em estoque");
                }
                decimal descontoPorUnidade = itemEnviado.DescontoUnitario ?? 0.0m;
                if (itemEnviado.DescontoUnitario > produtoDoItem.Preco)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O desconto unitário não dever ser maior do que o valor do produto");
                }
                AbaterQuantidadeEmEstoque(estoqueDoProdutoDoItem,itemEnviado.Quantidade);
                ItemVenda itemCriado = new ItemVenda(vendaCriada, produtoDoItem, itemEnviado.Quantidade, descontoPorUnidade, produtoDoItem.Preco);
                listaDeItensDaVendaCriada.Add(itemCriado);
                _itemVendaLogService.CriarLogsDeCriacao(itemCriado, vendaCriada, _usuarioLogado);
                _contexto.ItensVendas.Add(itemCriado);
                //coloco o save changes a cada interação no for each ou no final?, no final pois os itens precisão ser salvos no mesmo tempo da venda
                
            }

            foreach(ServicoVendaCreateDto servicoEnviado in dto.Venda.ServicosVenda)
            {
                Servico? servicoDaVenda = _contexto.Servicos.FirstOrDefault(s=>s.Id==servicoEnviado.IdServico && s.Ativo);
                if(servicoDaVenda == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado!!!");
                }
                decimal descontoDoServicoNaVenda = servicoEnviado.DescontoServico ?? 0.0m;
                if (descontoDoServicoNaVenda > servicoDaVenda.Preco)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O desconto do serviço não dever ser maior do que o valor do serviço");
                }
                ServicoVenda servicoDaVendaCriado = new ServicoVenda(vendaCriada, servicoDaVenda, descontoDoServicoNaVenda, servicoDaVenda.Preco);
                listaDeServicosDaVendaCriada.Add(servicoDaVendaCriado);
                _servicoVendaLogService.CriarLogsDeCriacao(servicoDaVendaCriado, vendaCriada, _usuarioLogado);
                _contexto.ServicosVendas.Add(servicoDaVendaCriado);
            }

            if(dto.Transacao.TipoPagamento== TipoPagamento.AVista && dto.Transacao.QuantidadeDeParcelas != 1)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Uma venda à vista deve ter apenas uma parcela");
            }

            Transacao transacaoCriada = new Transacao(vendaCriada, dto.Transacao.TipoPagamento, dto.Transacao.MeioPagamento);
            _contexto.Transacoes.Add(transacaoCriada);
            //validar se valores são maior que 0, já fiz essa validação???
            //decimal valorDeCadaParcela = vendaCriada.ValorTotalComDesconto / dto.Transacao.QuantidadeDeParcelas;

            List<Decimal> valoresDasParcelas = CalcularValorDasParcelas(vendaCriada.ValorTotalComDesconto, dto.Transacao.QuantidadeDeParcelas);

            if (!(dto.Transacao.DataDeVencinmentoPrimeiraParcela >= DateOnly.FromDateTime(DateTime.Today)))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A data de vencimento da primeira parcela deve ser maior ou igaual a data atual");
            }

            DateOnly dataDeVencimentoDaPrimeiraParcela = dto.Transacao.DataDeVencinmentoPrimeiraParcela;

            for (int i=0;i< valoresDasParcelas.Count(); i++)
            {
                Parcela parcelaCriada = new Parcela(transacaoCriada, (i + 1), valoresDasParcelas[i],dataDeVencimentoDaPrimeiraParcela.AddMonths(i));
                _contexto.Parcelas.Add(parcelaCriada);
            }
            _contexto.SaveChanges();

            return EntityToDto(vendaCriada);

            //fazer um endpoint para registrar pagamento de venda mesmo a vista, separar responsabilidade, de registrar pagamento

        }
        public decimal CalcularTotalVendaSemDescontoTotalAplicadoParaVendaCriada(List<ItemVendaCreateDto> itensVenda, List<ServicoVendaCreateDto> servicosVenda)
        {
            decimal totalVenda=0.0m;
            foreach (ItemVendaCreateDto itemIterado in itensVenda)
            {
                Produto? produtoVindoDoBanco = _contexto.Produtos.FirstOrDefault(p => p.Id == itemIterado.IdProduto && p.Ativo);
                if (produtoVindoDoBanco == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404,$"Produto com id {itemIterado.IdProduto} não encontrado");
                }
                decimal precoDoProdutoComDescontoAplicado =  Math.Round(produtoVindoDoBanco.Preco - (itemIterado.DescontoUnitario ?? 0.0m),2, MidpointRounding.AwayFromZero); 
                decimal totalItem =Math.Round ((precoDoProdutoComDescontoAplicado * itemIterado.Quantidade),2,MidpointRounding.AwayFromZero);
                totalVenda += totalItem;
            }
            foreach (ServicoVendaCreateDto servicoIterado in servicosVenda)
            {
                Servico? servicoVindoDoBanco = _contexto.Servicos.FirstOrDefault(s=>s.Id==servicoIterado.IdServico && s.Ativo);
                if (servicoVindoDoBanco == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, $"Serviço com id {servicoIterado.IdServico} não encontrado");
                }
                decimal precoDoServicoComDescontoAplicado =Math.Round(servicoVindoDoBanco.Preco - (servicoIterado.DescontoServico ?? 0.0m),2, MidpointRounding.AwayFromZero);
                decimal totalServico = Math.Round(precoDoServicoComDescontoAplicado,2,MidpointRounding.AwayFromZero);
                totalVenda += totalServico;
            }
            return totalVenda;
        }

        public decimal CalcularTotalVendaSemDescontoTotalAplicadoParaVendaAtualizada(List<ItemVenda> itensVenda, List<ServicoVenda> servicosVenda)
        {
            decimal totalVenda = 0.0m;
            foreach (ItemVenda itemIterado in itensVenda)
            {
                decimal precoDoProdutoComDescontoAplicado = Math.Round(itemIterado.PrecoUnitarioDoProdutoNaVendaSemDesconto - (itemIterado.DescontoUnitario), 2, MidpointRounding.AwayFromZero);
                decimal totalItem = Math.Round((precoDoProdutoComDescontoAplicado * itemIterado.Quantidade), 2, MidpointRounding.AwayFromZero);
                totalVenda += totalItem;
            }
            foreach (ServicoVenda servicoIterado in servicosVenda)
            {
                decimal precoDoServicoComDescontoAplicado = Math.Round(servicoIterado.PrecoServicoNaVendaSemDesconto - (servicoIterado.DescontoServico), 2, MidpointRounding.AwayFromZero);
                decimal totalServico = Math.Round(precoDoServicoComDescontoAplicado, 2, MidpointRounding.AwayFromZero);
                totalVenda += totalServico;
            }
            return totalVenda;
        }

        public List<decimal> CalcularValorDasParcelas(decimal totalDaVendaComDesconto, int quantidadeDeParcelas)
        {
            decimal valorBase = Math.Round(totalDaVendaComDesconto / quantidadeDeParcelas,2,MidpointRounding.AwayFromZero);

            var valorDeCadaParcela = Enumerable.Repeat(valorBase, quantidadeDeParcelas).ToList();

            decimal somaDasParcelas = valorDeCadaParcela.Sum();

            decimal diferencaValorTotalParaValorTotalDasParcelas = totalDaVendaComDesconto - somaDasParcelas;

            valorDeCadaParcela[quantidadeDeParcelas - 1] += diferencaValorTotalParaValorTotalDasParcelas;

            return valorDeCadaParcela;
        }

        public void AbaterQuantidadeEmEstoque(Estoque estoque, int quantidade)
        {
            //mudar para chamar o servive de estoque
            _estoqueService.AbaterQuantidadeEmEstoque(estoque.Id, quantidade);
        }

        public void AdicionarQuantidaEmEstoque(Estoque estoque, int quantidade)
        {
            _estoqueService.AdicionarQuantidadeEmEstoque(estoque.Id, quantidade);
        }
        //revisar lógica do service pois fiz com sono 
        public VendaTransacaoOutputDto AtualizarVenda(Guid idVendaEnviado, VendaTransacaoUpdateDto dto)
        {
            Venda VendaParaAtualizar = _contexto.Vendas.Where(v => v.Id == idVendaEnviado && v.Ativo).FirstOrDefault()
            ?? throw new ExcecaoDeRegraDeNegocio(404, "Venda não encontrada!!");

            Transacao transacaoDaVendaASerAtualizada = _contexto.Transacoes.Where(t=>t.IdVenda== VendaParaAtualizar.Id && t.Ativo).FirstOrDefault()
            ?? throw new ExcecaoDeRegraDeNegocio(404, "Transação não encontrada!!");


            if (transacaoDaVendaASerAtualizada.TransacaoEmCurso)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Não é possível atualizar essa venda pois ela já esta com o pagamento em andamento");
            }

            Cliente clienteAtualizadoDaVenda = _contexto.Clientes.Where(c => c.Id == dto.Venda.IdCliente && c.Ativo).Include(c => c.Endereco).FirstOrDefault()
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Cliente não encontrado!!!");

            Vendedor vendedorAtualizado = _vendedorRepositorio.BuscarPorId(dto.Venda.VendedorId)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "VEndedor não encontrado");

            List<ItemVendaCreateDto> itensNovosDto = dto.Venda.ItensVendaNovos;
            List<ServicoVendaCreateDto> servicosVendaNovosDto = dto.Venda.ServicosVendaNovos;

            List<ItemVendaUpdateDto> itensAtualizadosDto = dto.Venda.ItensVendaAtualizados;
            List<ServicoVendaUpdateDto> servicosVendaAtualizadosDto = dto.Venda.ServicosVendaAtualizados;


            if (dto.Transacao.QuantidadeDeParcelas > 60)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A Quantidade De Parcelas não deve ser maior que 60");
            }

            List<ItemVenda> itensVenda = _contexto.ItensVendas.Where(i => i.IdVenda == VendaParaAtualizar.Id && i.Ativo).ToList();

            foreach(ItemVenda itemIterado in itensVenda.ToList())
            {
                if (!itensAtualizadosDto.Any(i => i.IdItem == itemIterado.Id))
                {
                    Produto produtoDoItemIterado = itemIterado.Produto;
                    Estoque estoqueDoProduto = _contexto.Estoques.First(e => e.Produto.Id == produtoDoItemIterado.Id);
                    AdicionarQuantidaEmEstoque(estoqueDoProduto, itemIterado.Quantidade);
                    itemIterado.Ativo = false;
                    itensVenda.Remove(itemIterado);
                    _contexto.ItensVendas.Update(itemIterado);
                }
            }


            List<ServicoVenda> servicosVenda = _contexto.ServicosVendas.Where(i => i.IdVenda == VendaParaAtualizar.Id && i.Ativo).ToList();

            foreach (ServicoVenda servicoVendaIterado in servicosVenda.ToList())
            {
                if (!servicosVendaAtualizadosDto.Any(s => s.IdServicoVenda == servicoVendaIterado.Id))
                {
                    servicoVendaIterado.Ativo = false;
                    servicosVenda.Remove(servicoVendaIterado);
                    _contexto.ServicosVendas.Update(servicoVendaIterado);
                }
            }

            foreach(ItemVendaUpdateDto itemIteradoDto in itensAtualizadosDto)
            {
                ItemVenda itemVenda = itensVenda.FirstOrDefault(iv => iv.Id == itemIteradoDto.IdItem)
                    ?? throw new ExcecaoDeRegraDeNegocio(400, $"Não foi possível encontrar nenhum item venda para atualizar com esse id: {itemIteradoDto.IdItem}");

                
                Produto produtoDoItem = itemVenda.Produto;
                Estoque estoqueDoProduto=_contexto.Estoques.First(e=>e.ProdutoId == produtoDoItem.Id);

                if (itemIteradoDto.DescontoUnitario > produtoDoItem.Preco)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O valor do desconto unitario não pode ser maior o preço do produto, erro no item com id: " + itemVenda.Id);
                }

                estoqueDoProduto.AdicionarQuantidadeEmEstoque(itemVenda.Quantidade);
                if (itemIteradoDto.Quantidade > estoqueDoProduto.QuantidadeEmEstoque)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Quantidade insufisciente no estoque de produto com id: " + produtoDoItem.Id);
                }
                estoqueDoProduto.AbaterQuantidadeEmEstoque(itemIteradoDto.Quantidade);
                itemVenda.Quantidade = itemIteradoDto.Quantidade;
                itemVenda.DescontoUnitario = itemIteradoDto.DescontoUnitario ?? 0;
            }
            _contexto.ItensVendas.UpdateRange(itensVenda);


            foreach (ServicoVendaUpdateDto servicoVendaIteradoDto in servicosVendaAtualizadosDto)
            {
                ServicoVenda servicoVenda = servicosVenda.FirstOrDefault(sv => sv.Id == servicoVendaIteradoDto.IdServicoVenda)
                ?? throw new ExcecaoDeRegraDeNegocio(400, $"Não foi possível encontrar nenhum serviço venda para atualizar com esse id: {servicoVendaIteradoDto.IdServicoVenda}");

                Servico servicoRelacionadoAVenda = servicoVenda.Servico;

                if (servicoVendaIteradoDto.DescontoServico > servicoRelacionadoAVenda.Preco)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O valor do desconto do serviço não pode ser maior o preço do serviço, erro no serviço da venda com id: " 
                        + servicoVenda.Id);
                }
                servicoVenda.DescontoServico = servicoVendaIteradoDto.DescontoServico ?? 0;
            }
            _contexto.ServicosVendas.UpdateRange(servicosVenda);


            foreach(ItemVendaCreateDto itemIteradoDto in itensNovosDto)
            {
                Produto produto=_contexto.Produtos.FirstOrDefault(p=>p.Id==itemIteradoDto.IdProduto)
                    ?? throw new ExcecaoDeRegraDeNegocio(404,"produto não encontrado para o id: "+ itemIteradoDto.IdProduto);

                Estoque estoqueDoProduto = _contexto.Estoques.First(e => e.ProdutoId == produto.Id);
                if (itemIteradoDto.Quantidade > estoqueDoProduto.QuantidadeEmEstoque)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Quantidade insufisciente no estoque de produto com id: " + produto.Id);
                }
                if (itemIteradoDto.DescontoUnitario > produto.Preco)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O valor do desconto unitario não pode ser maior o preço do produto");
                }
                estoqueDoProduto.AbaterQuantidadeEmEstoque(itemIteradoDto.Quantidade);
                ItemVenda itemVenda = new(VendaParaAtualizar, produto, itemIteradoDto.Quantidade, itemIteradoDto.DescontoUnitario ?? 0, produto.Preco);
                itensVenda.Add(itemVenda);
                _contexto.ItensVendas.Add(itemVenda);
            }

            foreach (ServicoVendaCreateDto servicoVendaIteradoDto in servicosVendaNovosDto)
            {
                Servico servico = _contexto.Servicos.FirstOrDefault(s => s.Id == servicoVendaIteradoDto.IdServico)
                    ?? throw new ExcecaoDeRegraDeNegocio(404, "serviço não encontrado para o id: " + servicoVendaIteradoDto.IdServico);

                if (servicoVendaIteradoDto.DescontoServico > servico.Preco)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O valor do desconto do serviço não pode ser maior o preço do serviço");
                }
                ServicoVenda servicoVenda = new(VendaParaAtualizar, servico, servicoVendaIteradoDto.DescontoServico ?? 0, servico.Preco);
                servicosVenda.Add(servicoVenda);
                _contexto.ServicosVendas.Add(servicoVenda);
            }


            decimal valorTotalDaVendaSemDescontoTotalAplicado = CalcularTotalVendaSemDescontoTotalAplicadoParaVendaAtualizada(itensVenda, servicosVenda);
            decimal descontoVenda = dto.Venda.DescontoSobreTotalVenda ?? 0.0m;
            if (descontoVenda > valorTotalDaVendaSemDescontoTotalAplicado)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O desconto não pode ser maior que o total da venda");
            }

            decimal valorTotalDaVendaComDescontoAplicado = Math.Round((valorTotalDaVendaSemDescontoTotalAplicado - descontoVenda), 2, MidpointRounding.AwayFromZero);


            if (dto.Transacao.TipoPagamento == TipoPagamento.AVista && dto.Transacao.QuantidadeDeParcelas != 1)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Uma venda à vista deve ter apenas uma parcela");
            }


            List<decimal> valoresDasParcelas = CalcularValorDasParcelas(valorTotalDaVendaComDescontoAplicado, dto.Transacao.QuantidadeDeParcelas);
            if (!(dto.Transacao.DataDeVencinmentoPrimeiraParcela >= DateOnly.FromDateTime(DateTime.Today)))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A data de vencimento da primeira parcela deve ser maior ou igaual a data atual");
            }

            DateOnly dataDeVencimentoDaPrimeiraParcela = dto.Transacao.DataDeVencinmentoPrimeiraParcela;


            int quantidadeDeParcelasExistentes = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVendaASerAtualizada.Id && p.Ativo).Count();

            List<Parcela> parcelasDaVenda=_contexto.Parcelas.Where(p=>p.IdTransacao==transacaoDaVendaASerAtualizada.Id&& p.Ativo).OrderBy(p => p.NumeroDaParecelaDaVenda).ToList();

            if (quantidadeDeParcelasExistentes > dto.Transacao.QuantidadeDeParcelas)
            {
                int quantidaDeParcelasASeremDeletadas = quantidadeDeParcelasExistentes - dto.Transacao.QuantidadeDeParcelas;
                List<Parcela> parcelasDeletadas=parcelasDaVenda.TakeLast(quantidaDeParcelasASeremDeletadas).ToList();
                foreach(Parcela parcelaIterada in parcelasDeletadas)
                {
                    parcelaIterada.Ativo = false;
                    parcelasDaVenda.RemoveAll(p => p.Id == parcelaIterada.Id);
                    _contexto.Parcelas.Update(parcelaIterada);
                }

                for(int i = 0; i < parcelasDaVenda.Count; i++)
                {
                    parcelasDaVenda[i].NumeroDaParecelaDaVenda = i + 1;
                    parcelasDaVenda[i].DataVencimento=dataDeVencimentoDaPrimeiraParcela.AddMonths(i);
                    parcelasDaVenda[i].ValorParcela = valoresDasParcelas[i];
                }
                _contexto.Parcelas.UpdateRange(parcelasDaVenda);
                
            }
            else if (dto.Transacao.QuantidadeDeParcelas > quantidadeDeParcelasExistentes)
            {
                int quantidadeDeParcelasASerCriada = dto.Transacao.QuantidadeDeParcelas - quantidadeDeParcelasExistentes;
                DateOnly dataDeVencimentoProximaParcela=dataDeVencimentoDaPrimeiraParcela;
                int indiceDaParcela = 0;
                for (int i = 0; i < parcelasDaVenda.Count; i++)
                {
                    parcelasDaVenda[i].NumeroDaParecelaDaVenda = (indiceDaParcela+1);
                    parcelasDaVenda[i].DataVencimento = dataDeVencimentoProximaParcela;
                    parcelasDaVenda[i].ValorParcela = valoresDasParcelas[indiceDaParcela];
                    indiceDaParcela++;
                    dataDeVencimentoProximaParcela = dataDeVencimentoProximaParcela.AddMonths(1);
                }
                _contexto.Parcelas.UpdateRange(parcelasDaVenda);

                for (int i = 0; i < quantidadeDeParcelasASerCriada; i++)
                {
                    Parcela parcela = new(transacaoDaVendaASerAtualizada, indiceDaParcela + 1, valoresDasParcelas[indiceDaParcela],dataDeVencimentoProximaParcela);
                    indiceDaParcela++;
                    dataDeVencimentoProximaParcela = dataDeVencimentoProximaParcela.AddMonths(1);
                    parcelasDaVenda.Add(parcela);
                    _contexto.Parcelas.Add(parcela);
                }
               
            }
            else
            {
                for (int i = 0; i < parcelasDaVenda.Count; i++)
                {
                    parcelasDaVenda[i].NumeroDaParecelaDaVenda = i + 1;
                    parcelasDaVenda[i].DataVencimento = dataDeVencimentoDaPrimeiraParcela.AddMonths(i);
                    parcelasDaVenda[i].ValorParcela = valoresDasParcelas[i];
                }
                _contexto.Parcelas.UpdateRange(parcelasDaVenda);
            }


            VendaParaAtualizar.Cliente = clienteAtualizadoDaVenda;
            VendaParaAtualizar.IdCliente = clienteAtualizadoDaVenda.Id;
            VendaParaAtualizar.Vendedor = vendedorAtualizado;
            VendaParaAtualizar.IdVendedor=vendedorAtualizado.Id;
            VendaParaAtualizar.DescontoTotal = descontoVenda;
            VendaParaAtualizar.ValorTotalSemDesconto = valorTotalDaVendaSemDescontoTotalAplicado;
            VendaParaAtualizar.ValorTotalComDesconto = valorTotalDaVendaComDescontoAplicado;

            transacaoDaVendaASerAtualizada.TipoPagamento = dto.Transacao.TipoPagamento;
            transacaoDaVendaASerAtualizada.MeioPagamento = dto.Transacao.MeioPagamento;

            _contexto.Vendas.Update(VendaParaAtualizar);
            _contexto.Transacoes.Update(transacaoDaVendaASerAtualizada);
            _contexto.SaveChanges();

            return EntityToDto(VendaParaAtualizar);

        }

        public void DeletarVendaPorId(Guid idVenda)
        {
            Venda? vendaASerDeletada = _contexto.Vendas.FirstOrDefault(v=>v.Id==idVenda && v.Ativo);

            if(vendaASerDeletada== null)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Venda não encontrada");
            }
            Transacao? transacaoDaVendaASerDeletada = _contexto.Transacoes.FirstOrDefault(t=>t.IdVenda==vendaASerDeletada.Id && t.Ativo);

            if (transacaoDaVendaASerDeletada == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Transação não encontrada");
            }

            if (transacaoDaVendaASerDeletada.TransacaoEmCurso)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"A Venda não pode ser deletada pois o pagamaneto já está em andamento");
            }

            List<ItemVenda> itensDaVendaASeremDeletados=_contexto.ItensVendas.Include(i=>i.Produto).Where(i=>i.IdVenda==vendaASerDeletada.Id && i.Ativo).ToList();
            List<ServicoVenda> servicosDaVendaASeremDeletados = _contexto.ServicosVendas.Where(s => s.IdVenda == vendaASerDeletada.Id && s.Ativo).ToList();

            foreach(ItemVenda itemIterado in itensDaVendaASeremDeletados)
            {
                Produto produtoDoItem = itemIterado.Produto;
                Estoque estoqueDoProduto = _contexto.Estoques.First(e => e.Produto.Id == produtoDoItem.Id);
                AdicionarQuantidaEmEstoque(estoqueDoProduto, itemIterado.Quantidade);
                itemIterado.Ativo = false;
                _contexto.Update(itemIterado);
            }

            foreach(ServicoVenda servicoVendaIterado in servicosDaVendaASeremDeletados)
            {
                servicoVendaIterado.Ativo = false;
                _contexto.ServicosVendas.Update(servicoVendaIterado);
            }
            List<Parcela> parcelasDaTransacaoASeremDeletadas = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVendaASerDeletada.Id && p.Ativo).ToList();
            foreach(Parcela parcelaIterada in parcelasDaTransacaoASeremDeletadas)
            {
                parcelaIterada.Ativo = false;
                _contexto.Parcelas.Update(parcelaIterada);
            }

            transacaoDaVendaASerDeletada.Ativo = false;
            _contexto.Transacoes.Update(transacaoDaVendaASerDeletada);
            vendaASerDeletada.Ativo = false;
            _contexto.Vendas.Update(vendaASerDeletada);
            _contexto.SaveChanges();
        }
 

        //pode vir tanto a primeira vez para colocar parcelas como pagas, como ser a segunda...
        //quantidade de parcelas a pagar
        //ver esse método novamente, para fazer um segundo code review
        public TransacaoOutputDto AtualizarQuantidadeDeParcelasPagasEmUmaTransacao(Guid idTransacaoEnviado, int quantidadeDeParcelasASerAtualizadaParaPaga)
        {
            Transacao? transacaoReferente = _contexto.Transacoes.FirstOrDefault(t => t.Id == idTransacaoEnviado && t.Ativo);

            if(transacaoReferente == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404,"Transação não encontrada");
            }
            if (transacaoReferente.Pago)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Essa transação já esta paga, não há mais parcelas para pagar");
            }
            if(quantidadeDeParcelasASerAtualizadaParaPaga <= 0)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"O numero de parcelas pagas não pode ser menor ou igual a 0");
            }
            List<Parcela> parcelasDessaTransacao = _contexto.Parcelas.Where(p => p.IdTransacao == idTransacaoEnviado && p.Ativo).OrderBy(p=>p.NumeroDaParecelaDaVenda).ToList();
            List<Parcela> parcelasNaoPagasDessaTransacao = parcelasDessaTransacao.Where(p => p.Pago == false).ToList();
            int quantidadeDeParcelasNaoPagasNessaTransacao = parcelasNaoPagasDessaTransacao.Count();

            if(quantidadeDeParcelasASerAtualizadaParaPaga > quantidadeDeParcelasNaoPagasNessaTransacao)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A quantidade de parcelas para ser paga é maior do que a quantidade de parcelas não pagas dessa transação!!");
            }
            if(!transacaoReferente.TransacaoEmCurso)
            {
                transacaoReferente.TransacaoEmCurso = true;
                _contexto.Transacoes.Update(transacaoReferente);
            }

            foreach(Parcela parcelaIterada in parcelasNaoPagasDessaTransacao.Take(quantidadeDeParcelasASerAtualizadaParaPaga))
            {
                parcelaIterada.Pago = true;
                _contexto.Parcelas.Update(parcelaIterada);
            }
            //método antigo
            //for(int i = 0; i < quantidadeDeParcelasASerAtualizadaParaPaga; i++)
            //{
            //    Parcela parcelaIterada = parcelasNaoPagasDessaTransacao[i];
            //    parcelaIterada.Pago = true;
            //    _contexto.Parcelas.Update(parcelaIterada);
            //}
            quantidadeDeParcelasNaoPagasNessaTransacao = parcelasDessaTransacao.Where(p => p.Pago==false).Count();
            int quantidadeDeParcelasPagasDessaTransacao=parcelasDessaTransacao.Where(p=>p.Pago).Count();
            decimal valorPago =Math.Round((parcelasDessaTransacao.Where(p => p.Pago).Sum(p => p.ValorParcela)),2,MidpointRounding.AwayFromZero);
            if (quantidadeDeParcelasPagasDessaTransacao == parcelasDessaTransacao.Count)
            {
                transacaoReferente.Pago = true;
                _contexto.Transacoes.Update(transacaoReferente);
            }
            _contexto.SaveChanges();
            return new TransacaoOutputDto(transacaoReferente.Id, transacaoReferente.DataCriacao, transacaoReferente.TipoPagamento,
                transacaoReferente.MeioPagamento, transacaoReferente.TransacaoEmCurso, transacaoReferente.Pago, quantidadeDeParcelasNaoPagasNessaTransacao,
                quantidadeDeParcelasPagasDessaTransacao,valorPago,parcelasDessaTransacao);
        }


        public byte[] GerarRelatorioDeVendasPorPeriodo(DatasParaGeracaoDeRelatorioDto dto)
        {
            DateTime dataDeInicioDoPeriodoConvertidaDateTime;

            DateTime dataDeFimDoPeriodoConvertidaDateTime;

            bool dataInicioDoPeriodoNoFormatoCorreto = DateOnly.TryParseExact(dto.DataDeInicioDoPeriodo,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly dataDeInicioDoPeriodoFormatoDateOnly);

            if (!dataInicioDoPeriodoNoFormatoCorreto)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O formato da data deve estar no padrão ISO");
            }

            bool dataFimDoPeriodoNoFormatoCorreto = DateOnly.TryParseExact(dto.DataFinalDoPeriodo,
               "yyyy-MM-dd",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out DateOnly dataDeFimDoPeriodoDateOnly);

            if (!dataFimDoPeriodoNoFormatoCorreto)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O formato da data deve estar no padrão ISO");
            }

            if(dataDeInicioDoPeriodoFormatoDateOnly > dataDeFimDoPeriodoDateOnly)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"A data de fim de periodo nao pode ser maior do que ha de inicio do periodo");
            }
            if (dataDeFimDoPeriodoDateOnly > dataDeInicioDoPeriodoFormatoDateOnly.AddDays(366))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O Período não deve ser maior que 366 dias");
            }

            dataDeInicioDoPeriodoConvertidaDateTime = dataDeInicioDoPeriodoFormatoDateOnly.ToDateTime(TimeOnly.MinValue);
            dataDeFimDoPeriodoConvertidaDateTime = dataDeFimDoPeriodoDateOnly.ToDateTime(TimeOnly.MaxValue);

            //int numeroDeRegistroASerBuscados = _numeroMaximoDePaginas * _numeroDeLinhasPorPagina;

            List<Venda> vendasNoPeriodo = _contexto.Vendas.
                Where(v => v.DataCriacao >= dataDeInicioDoPeriodoConvertidaDateTime && v.DataCriacao <= dataDeFimDoPeriodoConvertidaDateTime && v.Ativo)
                .OrderBy(v => v.DataCriacao)
                .ToList();

            List<VendaNoFormatoASerExibidoRelatorioDto> listaDeVendasNoFormatoASerExibidoNoRelatorio=new List<VendaNoFormatoASerExibidoRelatorioDto>();

            decimal valorTotalPagoDasVendasNessePeriodo=0;

            decimal valorTotalDasVendasNessePeriodo=0;

            
            foreach(Venda vendaIterada in vendasNoPeriodo)
            {
                string nomeCliente="";
                string cpfOuCnpj = "";
                Cliente? cliente = _contexto.Clientes.Where(c => c.Id == vendaIterada.IdCliente && c.Ativo).FirstOrDefault();

                if(cliente == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(500, "Cliente nunca deveria ser nulo para uma venda já realizada");
                }
                if (cliente.TipoCliente == TipoCliente.PessoaFisica)
                {
                    nomeCliente = ((ClienteFisico)cliente).Nome;
                    cpfOuCnpj = ((ClienteFisico)cliente).Cpf;
                }
                if (cliente.TipoCliente == TipoCliente.PessoaJuridica)
                {
                    nomeCliente = ((ClienteJuridico)cliente).RazaoSocial;
                    cpfOuCnpj = ((ClienteJuridico)cliente).Cnpj;
                }
                if(cliente.TipoCliente != TipoCliente.PessoaFisica && cliente.TipoCliente != TipoCliente.PessoaJuridica)
                {
                    throw new ExcecaoDeRegraDeNegocio(500, "O tipo do cliente deve ser físico ou jurídico");
                }
                Transacao? transacaoDaVenda = _contexto.Transacoes.Where(t => t.IdVenda == vendaIterada.Id && t.Ativo).FirstOrDefault();
                if(transacaoDaVenda == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(500, "Transação nunca deveria ser nulo para uma venda já realizada");
                }
                string codigoVenda = vendaIterada.CodigoVenda;
                string tipoDePagamento = transacaoDaVenda.TipoPagamento.ToString();
                string meioDePagamento = transacaoDaVenda.MeioPagamento.ToString();
                string dataDaVenda = DateOnly.FromDateTime(vendaIterada.DataCriacao).ToString();
                decimal valorTotalPago =Math.Round ((_contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Pago && p.Ativo).Sum(p => p.ValorParcela)),2,MidpointRounding.AwayFromZero);
                valorTotalPagoDasVendasNessePeriodo += valorTotalPago;
                decimal valorTotalVenda = vendaIterada.ValorTotalComDesconto;
                valorTotalDasVendasNessePeriodo += valorTotalVenda;
                string pago="";
                if (transacaoDaVenda.Pago)
                {
                    pago = "Sim";
                }
                if (!transacaoDaVenda.Pago)
                {
                    pago = "Não";
                }

                VendaNoFormatoASerExibidoRelatorioDto vendaNoFormatoDto = new VendaNoFormatoASerExibidoRelatorioDto(codigoVenda,nomeCliente,cpfOuCnpj, tipoDePagamento,
                    dataDaVenda, valorTotalPago, valorTotalVenda,pago);

                listaDeVendasNoFormatoASerExibidoNoRelatorio.Add(vendaNoFormatoDto);
            }

            QuestPDF.Settings.License = LicenseType.Community;

            var documento = new RelatorioDeVendasPorPeriodo(listaDeVendasNoFormatoASerExibidoNoRelatorio,
                dataDeInicioDoPeriodoFormatoDateOnly, dataDeFimDoPeriodoDateOnly,valorTotalPagoDasVendasNessePeriodo,valorTotalDasVendasNessePeriodo);

            byte[] pdf = documento.GeneratePdf();

            return pdf;
        }

        public List<VendaTransacaoOutputDto> BuscarVendasPorCpfOuCnpj(DocumentoClienteInputDto dto)
        {
            List<VendaTransacaoOutputDto> vendasDoClienteNoFormatoDto=new List<VendaTransacaoOutputDto>();
            List<Venda> vendasDoCliente=new List<Venda>();
            Cliente? clienteDasVendasASeremBuscadas=null;
            if(dto.TipoDocumento== EnumTipoDocumentoASerBuscado.Cpf)
            {
                string cpfSomenteNumericos = DocumentoUtil.RemoverNaoNumericos(dto.NumeroDocumento);
                if (!DocumentoUtil.ValidarCpf(cpfSomenteNumericos))
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
                }
                clienteDasVendasASeremBuscadas = _contexto.ClientesFisicos.FirstOrDefault(c => c.Cpf == cpfSomenteNumericos && c.Ativo);
                if(clienteDasVendasASeremBuscadas == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Cliente não encontrado!!");
                }     
            }
            if (dto.TipoDocumento == EnumTipoDocumentoASerBuscado.Cnpj)
            {
                string cnpjSomenteNumericos = DocumentoUtil.RemoverNaoNumericos(dto.NumeroDocumento);
                if (!DocumentoUtil.ValidarCnpj(cnpjSomenteNumericos))
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Cnpj inválido");
                }

                clienteDasVendasASeremBuscadas = _contexto.ClientesJuridicos.FirstOrDefault(cj => cj.Cnpj == cnpjSomenteNumericos && cj.Ativo);
                if (clienteDasVendasASeremBuscadas == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Cliente não encontrado!!");
                }
            }
            if(clienteDasVendasASeremBuscadas == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cliente não encontrado");
            }
            vendasDoCliente = _contexto.Vendas.Include(v=>v.Cliente).Where(v => v.IdCliente == clienteDasVendasASeremBuscadas.Id && v.Ativo).ToList();
            if (vendasDoCliente.Count == 0)
            {
                return new List<VendaTransacaoOutputDto>();
            }
            foreach(Venda vendaIterada in vendasDoCliente)
            {
                VendaTransacaoOutputDto vendaTransacaoOutput = EntityToDto(vendaIterada);

                vendasDoClienteNoFormatoDto.Add(vendaTransacaoOutput);
            }
            return vendasDoClienteNoFormatoDto.OrderBy(vt => !vt.Transacao.TransacaoEmCurso ? 1 : vt.Transacao.TransacaoEmCurso && !vt.Transacao.Pago ? 2 : 3).ThenBy(vt => vt.Venda.DataCriacao).ToList(); ;
        }
        public VendaTransacaoOutputDto BuscarVendaPorCodigoVenda(string codigoVenda)
        {
            Venda? vendaVindaDoBanco = _contexto.Vendas.Include(v=>v.Cliente).ThenInclude(c=>c.Endereco).Where(v => v.CodigoVenda == codigoVenda).FirstOrDefault();
            if (vendaVindaDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Venda não encontrada");
            }
            Transacao? transacaoDaVenda = _contexto.Transacoes.FirstOrDefault(t=>t.IdVenda==vendaVindaDoBanco.Id);
            if (transacaoDaVenda == null)
            {
                throw new ExcecaoDeRegraDeNegocio(500, "Uma transação nunca deveria ser nula para uma venda já realizada");
            }

            VendaTransacaoOutputDto vendaTransacaoFormatoOutput = EntityToDto(vendaVindaDoBanco);

            return vendaTransacaoFormatoOutput;
        }

        public VendaTransacaoOutputDto EntityToDto(Venda venda)
        {

            List<ItemVenda> itensDaVenda = _contexto.ItensVendas.Include(i => i.Produto).Where(i => i.IdVenda == venda.Id && i.Ativo).ToList();
            List<ServicoVenda> servicosDaVenda = _contexto.ServicosVendas.Include(s => s.Servico).Where(s => s.IdVenda == venda.Id && s.Ativo).ToList();
            Transacao transacaoDaVenda = _contexto.Transacoes.Where(t => t.IdVenda == venda.Id && t.Ativo).FirstOrDefault();
            List<Parcela> parcelasDaTranscao = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Ativo).ToList();

            int QuantidadeDeParcelasNaoPagasDaVenda = parcelasDaTranscao.Where(p => p.Pago == false).Count();
            int QuantidadeDeParcelasPagasVenda = parcelasDaTranscao.Where(p => p.Pago).Count();
            decimal valorPago = Math.Round((parcelasDaTranscao.Where(p => p.Pago).Sum(p => p.ValorParcela)), 2, MidpointRounding.AwayFromZero);

            List<ItemVendaOutputDto> itensVendaFormatoDtoOutput = new List<ItemVendaOutputDto>();

            List<ServicoVendaOutputDto> servicosVendaFormatoDtoOutput = new List<ServicoVendaOutputDto>();

            foreach (ItemVenda item in itensDaVenda)
            {
                decimal precoDoProdutoNaVendaComDescontoAplicado = item.PrecoUnitarioDoProdutoNaVendaSemDesconto - item.DescontoUnitario;
                decimal totalItem = precoDoProdutoNaVendaComDescontoAplicado * item.Quantidade;
                ItemVendaOutputDto itemVendaOutputDto = new ItemVendaOutputDto(item.Id, item.Produto, item.DataCriacao, item.Quantidade, item.DescontoUnitario, item.PrecoUnitarioDoProdutoNaVendaSemDesconto, precoDoProdutoNaVendaComDescontoAplicado, totalItem);
                itensVendaFormatoDtoOutput.Add(itemVendaOutputDto);
            }
            foreach (ServicoVenda servicoVenda in servicosDaVenda)
            {
                decimal precoServicoNaVendaComDesconto = servicoVenda.PrecoServicoNaVendaSemDesconto - servicoVenda.DescontoServico;
                ServicoVendaOutputDto servicoVendaOutputDto = new ServicoVendaOutputDto(servicoVenda.Id, servicoVenda.Servico, servicoVenda.DataCriacao, servicoVenda.DescontoServico, servicoVenda.PrecoServicoNaVendaSemDesconto, precoServicoNaVendaComDesconto);
                servicosVendaFormatoDtoOutput.Add(servicoVendaOutputDto);
            }


            VendaOutputDto vendaOutputDto = new VendaOutputDto(venda.Id, venda.CodigoVenda, venda.DataCriacao, venda.DescontoTotal, venda.ValorTotalSemDesconto, venda.ValorTotalComDesconto, venda.Cliente,
                itensVendaFormatoDtoOutput, servicosVendaFormatoDtoOutput, venda.Vendedor);
            TransacaoOutputDto transacaoOutputDto = new TransacaoOutputDto(transacaoDaVenda.Id, transacaoDaVenda.DataCriacao, transacaoDaVenda.TipoPagamento,
                transacaoDaVenda.MeioPagamento, transacaoDaVenda.TransacaoEmCurso, transacaoDaVenda.Pago, QuantidadeDeParcelasNaoPagasDaVenda, QuantidadeDeParcelasPagasVenda,
                valorPago,parcelasDaTranscao);

            return new VendaTransacaoOutputDto(vendaOutputDto, transacaoOutputDto);

        }
    }
}
