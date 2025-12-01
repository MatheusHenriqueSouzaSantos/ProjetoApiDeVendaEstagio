using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
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
        private readonly int _numeroMaximoDePaginas = 5;
        private readonly int _numeroDeLinhasPorPagina = 42;
        private ContextoDb _contexto;
        private readonly GeradorCodigoVenda _geradorCodigoVenda;

        public VendaService(ContextoDb contexto)
        {
            _contexto = contexto;
            _geradorCodigoVenda = new GeradorCodigoVenda(_contexto);
        }

        public List<VendaTransacaoOutputDto> BuscarTodasVendas()
        {
            List<Venda> vendas = _contexto.Vendas.Include(v => v.Cliente).ThenInclude(c=>c.Endereco).Where(v=>v.Ativo).ToList();
            //List<ItemVenda> itensVenda = _contexto.ItensVendas.Include(i => i.Produto).Where(v => v.Ativo).ToList();
            //List<ServicoVenda> servicosVenda = _contexto.ServicosVendas.Include(i => i.Servico).Where(v => v.Ativo).ToList();
            //List<Transacao> transacoes = _contexto.Transacoes.Where(t => t.Ativo).ToList();
            //List<Parcela> parcelas = _contexto.Parcelas.Where(p => p.Ativo).ToList();

            /*"SELECT * FROM VENDA V, TRANSACAO T
             * WHERE V.ID = T.VENDA_ID "*/;

            List<VendaTransacaoOutputDto> ListaComTodasVendasTransacoes=new List<VendaTransacaoOutputDto>();

            foreach(Venda venda in vendas)
            {
                List<ItemVenda> itensDaVenda = _contexto.ItensVendas.Include(i => i.Produto).Where(i => i.IdVenda == venda.Id && i.Ativo).ToList();
                List<ServicoVenda> servicosDaVenda = _contexto.ServicosVendas.Include(s => s.Servico).Where(s => s.IdVenda == venda.Id && s.Ativo).ToList();
                Transacao transacaoDaVenda = _contexto.Transacoes.Where(t => t.IdVenda == venda.Id && t.Ativo).FirstOrDefault();
                List<Parcela> parcelasDaTranscao = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Ativo).ToList();

                int QuantidadeDeParcelasNaoPagasDaVenda=parcelasDaTranscao.Where(p=>p.Pago==false).Count();
                int QuantidadeDeParcelasPagasVenda=parcelasDaTranscao.Where(p=>p.Pago).Count();
                decimal valorPago = Math.Round(parcelasDaTranscao.Where(p => p.Pago).Sum(p => p.ValorParcela),2,MidpointRounding.AwayFromZero);

                List<ItemVendaOutputDto> itensVendaFormatoDtoOutput=new List<ItemVendaOutputDto>();

                List<ServicoVendaOutputDto> servicosVendaFormatoDtoOutput=new List<ServicoVendaOutputDto>();

                foreach(ItemVenda item in itensDaVenda)
                {
                    decimal precoDoProdutoNaVendaComDescontoAplicado = item.PrecoUnitarioDoProdutoNaVendaSemDesconto - item.DescontoUnitario;
                    decimal totalItem = precoDoProdutoNaVendaComDescontoAplicado * item.Quantidade;
                    ItemVendaOutputDto itemVendaOutputDto = new ItemVendaOutputDto(item.Id,item.Produto,item.DataCriacao,item.Quantidade,item.DescontoUnitario,item.PrecoUnitarioDoProdutoNaVendaSemDesconto,precoDoProdutoNaVendaComDescontoAplicado,totalItem);
                    itensVendaFormatoDtoOutput.Add(itemVendaOutputDto);
                }
                foreach (ServicoVenda servicoVenda in servicosDaVenda)
                {
                    decimal precoServicoNaVendaComDesconto = servicoVenda.PrecoServicoNaVendaSemDesconto - servicoVenda.DescontoServico;
                    ServicoVendaOutputDto servicoVendaOutputDto = new ServicoVendaOutputDto(servicoVenda.Id,servicoVenda.Servico,servicoVenda.DataCriacao,servicoVenda.DescontoServico,servicoVenda.PrecoServicoNaVendaSemDesconto, precoServicoNaVendaComDesconto);
                    servicosVendaFormatoDtoOutput.Add(servicoVendaOutputDto);
                }


                VendaOutputDto vendaOutputDto = new VendaOutputDto(venda.Id,venda.CodigoVenda, venda.DataCriacao, venda.DescontoTotal,venda.ValorTotalSemDesconto, venda.ValorTotalComDesconto, venda.Cliente,
                    itensVendaFormatoDtoOutput, servicosVendaFormatoDtoOutput);
                TransacaoOutputDto transacaoOutputDto = new TransacaoOutputDto(transacaoDaVenda.Id, transacaoDaVenda.DataCriacao, transacaoDaVenda.TipoPagamento,
                    transacaoDaVenda.MeioPagamento,transacaoDaVenda.TransacaoEmCurso, transacaoDaVenda.Pago, QuantidadeDeParcelasNaoPagasDaVenda,QuantidadeDeParcelasPagasVenda,valorPago);

                VendaTransacaoOutputDto vendaTransacaoOutputDto = new VendaTransacaoOutputDto(vendaOutputDto, transacaoOutputDto);
                ListaComTodasVendasTransacoes.Add(vendaTransacaoOutputDto);
            }

            return ListaComTodasVendasTransacoes;

        }
        
        public VendaTransacaoOutputDto BuscarVendaPorId(Guid id)
        {
            Venda? venda = _contexto.Vendas.Include(v => v.Cliente).ThenInclude(c => c.Endereco).FirstOrDefault(v=>v.Id==id && v.Ativo);
            if (venda == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404,"Venda não encontrada!!!");
            }

            VendaTransacaoOutputDto vendaTransacaoOutputDto;

            List<ItemVenda> itensDaVenda = _contexto.ItensVendas.Include(i => i.Produto).Where(i => i.IdVenda == venda.Id && i.Ativo).ToList();
            List<ServicoVenda> servicosDaVenda = _contexto.ServicosVendas.Include(s => s.Servico).Where(s => s.IdVenda == venda.Id && s.Ativo).ToList();
            Transacao transacaoDaVenda = _contexto.Transacoes.Where(t => t.IdVenda == venda.Id && t.Ativo).FirstOrDefault();
            List<Parcela> parcelasDaTranscao = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Ativo).ToList();

            int QuantidadeDeParcelasNaoPagasDaVenda = parcelasDaTranscao.Where(p=>p.Pago==false).Count();
            int QuantidadeDeParcelasPagasVenda = parcelasDaTranscao.Where(p => p.Pago).Count();
            decimal valorPago = Math.Round((parcelasDaTranscao.Where(p => p.Pago).Sum(p => p.ValorParcela)),2,MidpointRounding.AwayFromZero);

            List<ItemVendaOutputDto> itensVendaFormatoDtoOutput = new List<ItemVendaOutputDto>();

            List<ServicoVendaOutputDto> servicosVendaFormatoDtoOutput = new List<ServicoVendaOutputDto>();

            foreach (ItemVenda item in itensDaVenda)
            {
                decimal precoDoProdutoNaVendaComDescontoAplicado = item.PrecoUnitarioDoProdutoNaVendaSemDesconto - item.DescontoUnitario;
                decimal totalItem = precoDoProdutoNaVendaComDescontoAplicado * item.Quantidade;
                ItemVendaOutputDto itemVendaOutputDto = new ItemVendaOutputDto(item.Id, item.Produto, item.DataCriacao, item.Quantidade, item.DescontoUnitario, item.PrecoUnitarioDoProdutoNaVendaSemDesconto, precoDoProdutoNaVendaComDescontoAplicado,totalItem);
                itensVendaFormatoDtoOutput.Add(itemVendaOutputDto);
            }
            foreach (ServicoVenda servicoVenda in servicosDaVenda)
            {
                decimal precoServicoNaVendaComDesconto = servicoVenda.PrecoServicoNaVendaSemDesconto - servicoVenda.DescontoServico;
                ServicoVendaOutputDto servicoVendaOutputDto = new ServicoVendaOutputDto(servicoVenda.Id, servicoVenda.Servico, servicoVenda.DataCriacao, servicoVenda.DescontoServico, servicoVenda.PrecoServicoNaVendaSemDesconto, precoServicoNaVendaComDesconto);
                servicosVendaFormatoDtoOutput.Add(servicoVendaOutputDto);
            }


            VendaOutputDto vendaOutputDto = new VendaOutputDto(venda.Id,venda.CodigoVenda, venda.DataCriacao, venda.DescontoTotal,venda.ValorTotalSemDesconto, venda.ValorTotalComDesconto, venda.Cliente,
                itensVendaFormatoDtoOutput, servicosVendaFormatoDtoOutput);
            TransacaoOutputDto transacaoOutputDto = new TransacaoOutputDto(transacaoDaVenda.Id, transacaoDaVenda.DataCriacao, transacaoDaVenda.TipoPagamento,
                transacaoDaVenda.MeioPagamento,transacaoDaVenda.TransacaoEmCurso, transacaoDaVenda.Pago, QuantidadeDeParcelasNaoPagasDaVenda, QuantidadeDeParcelasPagasVenda, valorPago);

            vendaTransacaoOutputDto = new VendaTransacaoOutputDto(vendaOutputDto, transacaoOutputDto);

            return vendaTransacaoOutputDto;
        }
        //refatorar alguns métodos para separar a lógica em métodos menores
        public VendaTransacaoOutputDto CadastrarVenda(VendaTransacaoInputDto dto)
        {
            Cliente? clienteDaVenda = _contexto.Clientes.Where(c => c.Id == dto.Venda.IdCliente && c.Ativo).Include(c => c.Endereco).FirstOrDefault();
            if (clienteDaVenda == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Cliente não encontrado!!!");
            }
            List<ItemVendaInputDto> itensVenda = dto.Venda.ItensVenda;
            List<ServicoVendaInputDto> servicosVenda = dto.Venda.ServicosVenda;
            decimal valorTotalDaVendaSemDescontoTotalAplicado=CalcularTotalVendaSemDescontoTotalAplicado(itensVenda,servicosVenda);
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

            decimal valorTotalDaVendaComDescontoAplicado=Math.Round( (valorTotalDaVendaSemDescontoTotalAplicado-descontoVenda),2,MidpointRounding.AwayFromZero);

            string codigoVenda = _geradorCodigoVenda.GerarCodigoVenda();

            Venda vendaCriada = new Venda(clienteDaVenda,codigoVenda, clienteDaVenda.Id, descontoVenda,valorTotalDaVendaSemDescontoTotalAplicado, valorTotalDaVendaComDescontoAplicado);

            List<ItemVenda> listaDeItensDaVendaCriada = new List<ItemVenda>();

            List<ServicoVenda> listaDeServicosDaVendaCriada =new List<ServicoVenda>();

            foreach(ItemVendaInputDto itemEnviado in dto.Venda.ItensVenda.ToList())
            {
                Produto? produtoDoItem = _contexto.Produtos.FirstOrDefault(p => p.Id == itemEnviado.IdProduto && p.Ativo);

                if(produtoDoItem == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado!!!");
                }
                if (itemEnviado.Quantidade == 0)
                {
                    throw new ExcecaoDeRegraDeNegocio(400,"Não é possível adicionar um produto com 0 unidades");
                }
                if(produtoDoItem.QuantidadeEmEstoque< itemEnviado.Quantidade)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Estoque do produto: " + produtoDoItem.NomeProduto + " insuficiente, pois tem apenas: " +
                        produtoDoItem.QuantidadeEmEstoque + " unidades em estoque");
                }
                decimal descontoPorUnidade = itemEnviado.DescontoUnitario ?? 0.0m;
                if (itemEnviado.DescontoUnitario > produtoDoItem.PrecoUnitario)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O desconto unitário não dever ser maior do que o valor do produto");
                }
                AbaterQuantidadeEmEstoque(produtoDoItem,itemEnviado.Quantidade);
                ItemVenda itemCriado = new ItemVenda(vendaCriada, produtoDoItem, itemEnviado.Quantidade, descontoPorUnidade, produtoDoItem.PrecoUnitario);
                listaDeItensDaVendaCriada.Add(itemCriado);
                _contexto.ItensVendas.Add(itemCriado);
                //coloco o save changes a cada interação no for each ou no final?
                
            }

            foreach(ServicoVendaInputDto servicoEnviado in dto.Venda.ServicosVenda)
            {
                Servico? servicoDaVenda = _contexto.Servicos.FirstOrDefault(s=>s.Id==servicoEnviado.IdServico && s.Ativo);
                if(servicoDaVenda == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado!!!");
                }
                decimal descontoDoServicoNaVenda = servicoEnviado.DescontoServico ?? 0.0m;
                if (descontoDoServicoNaVenda > servicoDaVenda.PrecoServico)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O desconto do serviço não dever ser maior do que o valor do serviço");
                }
                ServicoVenda servicoDaVendaCriado = new ServicoVenda(vendaCriada, servicoDaVenda, descontoDoServicoNaVenda, servicoDaVenda.PrecoServico);
                listaDeServicosDaVendaCriada.Add(servicoDaVendaCriado);
                _contexto.ServicosVendas.Add(servicoDaVendaCriado);
            }

            if(dto.Transacao.TipoPagamento== TipoPagamento.AVista && dto.Transacao.QuantidadeDeParcelas != 1)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Uma venda à vista deve ter apenas uma parcela");
            }

            Transacao transacaoCriada = new Transacao(vendaCriada, dto.Transacao.TipoPagamento, dto.Transacao.MeioPagamento);
            //validar se valores são maior que 0, já fiz essa validação???
            //decimal valorDeCadaParcela = vendaCriada.ValorTotalComDesconto / dto.Transacao.QuantidadeDeParcelas;

            List<Decimal> valoresDasParcelas = calcularValorDasParcelas(vendaCriada.ValorTotalComDesconto, dto.Transacao.QuantidadeDeParcelas);

            for (int i=0;i< valoresDasParcelas.Count(); i++)
            {
                Parcela parcelaCriada = new Parcela(transacaoCriada, (i + 1), valoresDasParcelas[i]);
                _contexto.Parcelas.Add(parcelaCriada);
            }
            _contexto.SaveChanges();

            List<ItemVendaOutputDto> listaDeItensASeremRetornados= new List<ItemVendaOutputDto>();
            List<ServicoVendaOutputDto> listaDeServicosASeremRetornados= new List<ServicoVendaOutputDto>();

            foreach(ItemVenda itemNoFormatoModel in listaDeItensDaVendaCriada)
            {
                decimal precoDoProdutoNaVendaComDescontoAplicado = itemNoFormatoModel.PrecoUnitarioDoProdutoNaVendaSemDesconto - itemNoFormatoModel.DescontoUnitario;
                decimal totalItem = precoDoProdutoNaVendaComDescontoAplicado * itemNoFormatoModel.Quantidade;
                ItemVendaOutputDto itemNoFormatoDeOutput = new ItemVendaOutputDto(itemNoFormatoModel.Id, itemNoFormatoModel.Produto, itemNoFormatoModel.DataCriacao,
                    itemNoFormatoModel.Quantidade, itemNoFormatoModel.DescontoUnitario, itemNoFormatoModel.PrecoUnitarioDoProdutoNaVendaSemDesconto, precoDoProdutoNaVendaComDescontoAplicado,totalItem);

                listaDeItensASeremRetornados.Add(itemNoFormatoDeOutput);
            }

            foreach(ServicoVenda servicoVendaNoFormatoModel in listaDeServicosDaVendaCriada)
            {
                decimal precoDoServicoNaVendaComDescontoAplicado = servicoVendaNoFormatoModel.PrecoServicoNaVendaSemDesconto - servicoVendaNoFormatoModel.DescontoServico;
                ServicoVendaOutputDto servicoVendaFormatoDeOutput = new ServicoVendaOutputDto(servicoVendaNoFormatoModel.Id,servicoVendaNoFormatoModel.Servico,servicoVendaNoFormatoModel.DataCriacao,
                    servicoVendaNoFormatoModel.DescontoServico,servicoVendaNoFormatoModel.PrecoServicoNaVendaSemDesconto, precoDoServicoNaVendaComDescontoAplicado);

                listaDeServicosASeremRetornados.Add(servicoVendaFormatoDeOutput);
            }//regra no meio de pagamento, tipo pagamento a prazo não pode ser dinheiro??
            VendaOutputDto vendaASerRetornadaNoFormatoOutput = new VendaOutputDto(vendaCriada.Id,vendaCriada.CodigoVenda, vendaCriada.DataCriacao, vendaCriada.DescontoTotal,vendaCriada.ValorTotalSemDesconto, vendaCriada.ValorTotalComDesconto,
                vendaCriada.Cliente,listaDeItensASeremRetornados,listaDeServicosASeremRetornados);
            int quantidadeDeParcelasNaoPagasDessaTransacao = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoCriada.Id && p.Ativo && p.Pago==false).Count();
            int numeroDeParcelasPagas = 0;
            decimal valorPago = 0.0m;
            TransacaoOutputDto transacaoASerRetornadaNoFormatoOutput = new TransacaoOutputDto(transacaoCriada.Id, transacaoCriada.DataCriacao, transacaoCriada.TipoPagamento,
                transacaoCriada.MeioPagamento,transacaoCriada.TransacaoEmCurso,transacaoCriada.Pago,quantidadeDeParcelasNaoPagasDessaTransacao,numeroDeParcelasPagas,valorPago);

            VendaTransacaoOutputDto vendaTransacaoASerRetornadaFormatoOutput = new(vendaASerRetornadaNoFormatoOutput,transacaoASerRetornadaNoFormatoOutput);

            return vendaTransacaoASerRetornadaFormatoOutput;

            //fazer um endpoint para registrar pagamento de venda mesmo a vista, separar responsabilidade, de registrar pagamento

        }
        public decimal CalcularTotalVendaSemDescontoTotalAplicado(List<ItemVendaInputDto> itensVenda, List<ServicoVendaInputDto> servicosVenda)
        {
            decimal totalVenda=0.0m;
            foreach (ItemVendaInputDto itemIterado in itensVenda)
            {
                Produto? produtoVindoDoBanco = _contexto.Produtos.FirstOrDefault(p => p.Id == itemIterado.IdProduto && p.Ativo);
                if (produtoVindoDoBanco == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404,$"Produto com id {itemIterado.IdProduto} não encontrado");
                }
                decimal precoDoProdutoComDescontoAplicado =  Math.Round(produtoVindoDoBanco.PrecoUnitario - (itemIterado.DescontoUnitario ?? 0.0m),2, MidpointRounding.AwayFromZero); 
                decimal totalItem =Math.Round ((precoDoProdutoComDescontoAplicado * itemIterado.Quantidade),2,MidpointRounding.AwayFromZero);
                totalVenda += totalItem;
            }
            foreach (ServicoVendaInputDto servicoIterado in servicosVenda)
            {
                Servico? servicoVindoDoBanco = _contexto.Servicos.FirstOrDefault(s=>s.Id==servicoIterado.IdServico && s.Ativo);
                if (servicoVindoDoBanco == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, $"Serviço com id {servicoIterado.IdServico} não encontrado");
                }
                decimal precoDoServicoComDescontoAplicado =Math.Round(servicoVindoDoBanco.PrecoServico - (servicoIterado.DescontoServico ?? 0.0m),2, MidpointRounding.AwayFromZero);
                decimal totalServico = Math.Round(precoDoServicoComDescontoAplicado,2,MidpointRounding.AwayFromZero);
                totalVenda += totalServico;
            }
            return totalVenda;
        }

        public List<decimal> calcularValorDasParcelas(decimal totalDaVendaComDesconto, int quantidadeDeParcelas)
        {
            decimal valorBase = Math.Round(totalDaVendaComDesconto / quantidadeDeParcelas,2,MidpointRounding.AwayFromZero);

            var valorDeCadaParcela = Enumerable.Repeat(valorBase, quantidadeDeParcelas).ToList();

            decimal somaDasParcelas = valorDeCadaParcela.Sum();

            decimal diferencaValorTotalParaValorTotalDasParcelas = totalDaVendaComDesconto - somaDasParcelas;

            valorDeCadaParcela[quantidadeDeParcelas - 1] += diferencaValorTotalParaValorTotalDasParcelas;

            return valorDeCadaParcela;
        }

        public void AbaterQuantidadeEmEstoque(Produto produto, int quantidadeASerAbatida)
        {
            int novaQuantidadeEmEstoque = produto.QuantidadeEmEstoque - quantidadeASerAbatida;
            produto.QuantidadeEmEstoque=novaQuantidadeEmEstoque;
            _contexto.Produtos.Update(produto);
        }

        public void AdicionarQuantidaEmEstoque(Produto produto, int quantidadeASerAdicionada)
        {
            int novaQuantidadeEmEstoque = produto.QuantidadeEmEstoque +quantidadeASerAdicionada;
            produto.QuantidadeEmEstoque= novaQuantidadeEmEstoque;
            _contexto.Produtos.Update(produto);
        }
        //revisar lógica do service pois fiz com sono 
        public VendaTransacaoOutputDto AtualizarVenda(Guid idVendaEnviado, VendaTransacaoInputDto dto)
        {
            Venda? VendaParaAtualizar=_contexto.Vendas.Where(v=>v.Id==idVendaEnviado && v.Ativo).FirstOrDefault();

            if (VendaParaAtualizar == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Venda não encontrada!!");
            }
            Transacao? transacaoDaVendaASerAtualizada = _contexto.Transacoes.Where(t=>t.IdVenda== VendaParaAtualizar.Id && t.Ativo).FirstOrDefault();

            if(transacaoDaVendaASerAtualizada == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404,"Transação não encontrada!!");
            }

            if (transacaoDaVendaASerAtualizada.TransacaoEmCurso)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Não é possível atualizar essa venda pois ela já esta com o pagamento em andamento");
            }

            Cliente? clienteAtualizadoDaVenda = _contexto.Clientes.Where(c => c.Id == dto.Venda.IdCliente && c.Ativo).Include(c => c.Endereco).FirstOrDefault();
            if (clienteAtualizadoDaVenda == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Cliente não encontrado!!!");
            }
            List<ItemVendaInputDto> itensEnviadosFormatoDto = dto.Venda.ItensVenda;
            List<ServicoVendaInputDto> servicosEnviadosFormatoDto = dto.Venda.ServicosVenda;
            decimal valorTotalDaVendaSemDescontoTotalAplicado =CalcularTotalVendaSemDescontoTotalAplicado(itensEnviadosFormatoDto, servicosEnviadosFormatoDto);
            decimal descontoVenda = dto.Venda.DescontoSobreTotalVenda ?? 0.0m;
            if (descontoVenda < 0)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O desconto não pode ser negativo");
            }

            if (descontoVenda > valorTotalDaVendaSemDescontoTotalAplicado)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O desconto não pode ser maior que o total da venda");
            }

            decimal valorTotalDaVendaComDescontoAplicado = Math.Round((valorTotalDaVendaSemDescontoTotalAplicado - descontoVenda),2,MidpointRounding.AwayFromZero);


            if (itensEnviadosFormatoDto.Count < 1 && servicosEnviadosFormatoDto.Count < 1)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A venda deve conter pelo menos um item ou serviço");
            }

            List<ItemVenda> itensAntigoASeremExcluidos = _contexto.ItensVendas.Include(i => i.Produto).Where(i => i.IdVenda == VendaParaAtualizar.Id && i.Ativo).ToList();

            foreach (ItemVenda itemIterado in itensAntigoASeremExcluidos)
            {
                Produto produtoDoItemIterado = itemIterado.Produto;
                AdicionarQuantidaEmEstoque(produtoDoItemIterado, itemIterado.Quantidade);
                _contexto.ItensVendas.Remove(itemIterado);
            }

            List<ServicoVenda> servicosAntigosASeremExcluidos = _contexto.ServicosVendas.Where(s => s.IdVenda == VendaParaAtualizar.Id && s.Ativo).ToList();

            foreach (ServicoVenda servicoDaVendaIterado in servicosAntigosASeremExcluidos)
            {
                _contexto.ServicosVendas.Remove(servicoDaVendaIterado);
            }

            if (dto.Transacao.TipoPagamento == TipoPagamento.AVista && dto.Transacao.QuantidadeDeParcelas != 1)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Uma venda à vista deve ter apenas uma parcela");
            }

            List<Parcela> parcelasAntigasASeremExcluidas = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVendaASerAtualizada.Id && p.Ativo).ToList();

            foreach (Parcela parcelaIterada in parcelasAntigasASeremExcluidas)
            {
                _contexto.Parcelas.Remove(parcelaIterada);
            }

            List<ItemVenda> listaDeItensAtualizadosDaVenda = new List<ItemVenda>();

            List<ServicoVenda> listaDeServicosAtualizadosDaVenda = new List<ServicoVenda>();

            foreach (ItemVendaInputDto itemEnviado in dto.Venda.ItensVenda.ToList())
            {
                Produto? produtoDoItem = _contexto.Produtos.FirstOrDefault(p => p.Id == itemEnviado.IdProduto && p.Ativo);

                if (produtoDoItem == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado!!!");
                }
                if (itemEnviado.Quantidade == 0)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Não é possível adicionar um produto com 0 unidades");
                }
                if (produtoDoItem.QuantidadeEmEstoque < itemEnviado.Quantidade)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Estoque do produto: " + produtoDoItem.NomeProduto + " insuficiente, pois tem apenas: " +
                        produtoDoItem.QuantidadeEmEstoque + " unidades em estoque");
                }
                decimal descontoPorUnidade = itemEnviado.DescontoUnitario ?? 0.0m;
                if (itemEnviado.DescontoUnitario > produtoDoItem.PrecoUnitario)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O desconto unitário não dever ser maior do que o valor do produto");
                }
                AbaterQuantidadeEmEstoque(produtoDoItem, itemEnviado.Quantidade);
                ItemVenda itemCriado = new ItemVenda(VendaParaAtualizar, produtoDoItem, itemEnviado.Quantidade, descontoPorUnidade, produtoDoItem.PrecoUnitario);
                listaDeItensAtualizadosDaVenda.Add(itemCriado);
                _contexto.ItensVendas.Add(itemCriado);

            }

            foreach (ServicoVendaInputDto servicoEnviado in dto.Venda.ServicosVenda)
            {
                Servico? servicoDaVenda = _contexto.Servicos.FirstOrDefault(s => s.Id == servicoEnviado.IdServico && s.Ativo);
                if (servicoDaVenda == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado!!!");
                }
                decimal descontoDoServicoNaVenda = servicoEnviado.DescontoServico ?? 0.0m;
                if (descontoDoServicoNaVenda > servicoDaVenda.PrecoServico)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O desconto do serviço não dever ser maior do que o valor do serviço");
                }
                ServicoVenda servicoDaVendaCriado = new ServicoVenda(VendaParaAtualizar, servicoDaVenda, descontoDoServicoNaVenda, servicoDaVenda.PrecoServico);
                listaDeServicosAtualizadosDaVenda.Add(servicoDaVendaCriado);
                _contexto.ServicosVendas.Add(servicoDaVendaCriado);
            }

            //List<Parcela> parcelasAtualizadasDaVenda=new List<Parcela>();

            //decimal valorDeCadaParcela =Math.Round((valorTotalDaVendaComDescontoAplicado / dto.Transacao.QuantidadeDeParcelas),2,MidpointRounding.AwayFromZero);

            List<Decimal> valoresDasParcelas = calcularValorDasParcelas(valorTotalDaVendaComDescontoAplicado, dto.Transacao.QuantidadeDeParcelas);

            for (int i = 0; i < valoresDasParcelas.Count(); i++)
            {
                Parcela parcelaCriada = new Parcela(transacaoDaVendaASerAtualizada, (i + 1), valoresDasParcelas[i]);
                _contexto.Parcelas.Add(parcelaCriada);
            }
            VendaParaAtualizar.Cliente = clienteAtualizadoDaVenda;
            VendaParaAtualizar.IdCliente = clienteAtualizadoDaVenda.Id;
            VendaParaAtualizar.DescontoTotal = descontoVenda;
            VendaParaAtualizar.ValorTotalSemDesconto = valorTotalDaVendaSemDescontoTotalAplicado;
            VendaParaAtualizar.ValorTotalComDesconto = valorTotalDaVendaComDescontoAplicado;

            transacaoDaVendaASerAtualizada.TipoPagamento = dto.Transacao.TipoPagamento;
            transacaoDaVendaASerAtualizada.MeioPagamento = dto.Transacao.MeioPagamento;

            _contexto.Vendas.Update(VendaParaAtualizar);
            _contexto.Transacoes.Update(transacaoDaVendaASerAtualizada);
            _contexto.SaveChanges();

            List<ItemVendaOutputDto> listaDeItensASeremRetornadosFormatoOutput = new List<ItemVendaOutputDto>();
            List<ServicoVendaOutputDto> listaDeServicosASeremRetornadosFormatoOutput = new List<ServicoVendaOutputDto>();

            foreach (ItemVenda itemNoFormatoModel in listaDeItensAtualizadosDaVenda)
            {
                decimal PrecoProdutoNaVendaComDescontoAplicado = itemNoFormatoModel.PrecoUnitarioDoProdutoNaVendaSemDesconto - itemNoFormatoModel.DescontoUnitario;
                decimal totalItem = PrecoProdutoNaVendaComDescontoAplicado * itemNoFormatoModel.Quantidade;
                ItemVendaOutputDto itemNoFormatoDeOutput = new ItemVendaOutputDto(itemNoFormatoModel.Id, itemNoFormatoModel.Produto, itemNoFormatoModel.DataCriacao,
                    itemNoFormatoModel.Quantidade, itemNoFormatoModel.DescontoUnitario, itemNoFormatoModel.PrecoUnitarioDoProdutoNaVendaSemDesconto, PrecoProdutoNaVendaComDescontoAplicado,totalItem);

                listaDeItensASeremRetornadosFormatoOutput.Add(itemNoFormatoDeOutput);
            }

            foreach (ServicoVenda servicoVendaNoFormatoModel in listaDeServicosAtualizadosDaVenda)
            {
                decimal precoServicoNaVendaComDescontoAplicado = servicoVendaNoFormatoModel.PrecoServicoNaVendaSemDesconto - servicoVendaNoFormatoModel.DescontoServico;
                ServicoVendaOutputDto servicoVendaFormatoDeOutput = new ServicoVendaOutputDto(servicoVendaNoFormatoModel.Id, servicoVendaNoFormatoModel.Servico, servicoVendaNoFormatoModel.DataCriacao,
                    servicoVendaNoFormatoModel.DescontoServico, servicoVendaNoFormatoModel.PrecoServicoNaVendaSemDesconto, precoServicoNaVendaComDescontoAplicado);

                listaDeServicosASeremRetornadosFormatoOutput.Add(servicoVendaFormatoDeOutput);
            }
            //definição só para ficar mais claro pois a venda já esta atualizada agora
            Venda VendaAtualizada = VendaParaAtualizar;
            Transacao transacaoDaVendaAtualizada = transacaoDaVendaASerAtualizada;

            VendaOutputDto vendaASerRetornadaNoFormatoOutput = new VendaOutputDto(VendaAtualizada.Id,VendaAtualizada.CodigoVenda, VendaAtualizada.DataCriacao, VendaAtualizada.DescontoTotal,VendaAtualizada.ValorTotalSemDesconto, VendaAtualizada.ValorTotalComDesconto,
                VendaAtualizada.Cliente, listaDeItensASeremRetornadosFormatoOutput, listaDeServicosASeremRetornadosFormatoOutput);
            int quantidadeDeParcelasNaoPagasDessaTransacao = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVendaASerAtualizada.Id && p.Ativo && p.Pago==false).Count();
            int numeroDeParcelasPagas = 0;
            decimal valorPago = 0.0m;
            TransacaoOutputDto transacaoASerRetornadaNoFormatoOutput = new TransacaoOutputDto(transacaoDaVendaAtualizada.Id, transacaoDaVendaAtualizada.DataCriacao, transacaoDaVendaAtualizada.TipoPagamento,
                transacaoDaVendaAtualizada.MeioPagamento, transacaoDaVendaAtualizada.TransacaoEmCurso, transacaoDaVendaAtualizada.Pago, quantidadeDeParcelasNaoPagasDessaTransacao, numeroDeParcelasPagas, valorPago);

            VendaTransacaoOutputDto vendaTransacaoASerRetornadaFormatoOutput = new(vendaASerRetornadaNoFormatoOutput, transacaoASerRetornadaNoFormatoOutput);

            return vendaTransacaoASerRetornadaFormatoOutput;

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
                AdicionarQuantidaEmEstoque(itemIterado.Produto, itemIterado.Quantidade);
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
                quantidadeDeParcelasPagasDessaTransacao,valorPago);
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

            bool dataFimDoPeriodoNoFormatoCorreto = DateOnly.TryParseExact(dto.DataDeFimDoPeriodo,
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

            dataDeInicioDoPeriodoConvertidaDateTime = dataDeInicioDoPeriodoFormatoDateOnly.ToDateTime(TimeOnly.MinValue);
            dataDeFimDoPeriodoConvertidaDateTime = dataDeFimDoPeriodoDateOnly.ToDateTime(TimeOnly.MaxValue);


            int numeroDeRegistroASerBuscados = _numeroMaximoDePaginas * _numeroDeLinhasPorPagina;

            List<Venda> vendasNoPeriodo = _contexto.Vendas.
                Where(v => v.DataCriacao >= dataDeInicioDoPeriodoConvertidaDateTime && v.DataCriacao <= dataDeFimDoPeriodoConvertidaDateTime && v.Ativo)
                .OrderBy(v => v.DataCriacao)
                .Take(numeroDeRegistroASerBuscados)
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
                string cpfSomenteNumericos = ClienteUtil.RemoverNaoNumericos(dto.NumeroDocumento);
                if (!ClienteUtil.ValidarCpf(cpfSomenteNumericos))
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
                string cnpjSomenteNumericos = ClienteUtil.RemoverNaoNumericos(dto.NumeroDocumento);
                if (!ClienteUtil.ValidarCnpj(cnpjSomenteNumericos))
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "Cnpj inválido");
                }

                clienteDasVendasASeremBuscadas = _contexto.ClientesJuridicos.FirstOrDefault(cj => cj.Cnpj == dto.NumeroDocumento && cj.Ativo);
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
                List<ItemVenda> itensDaVenda = _contexto.ItensVendas.Include(i=>i.Produto).Where(i => i.IdVenda == vendaIterada.Id && i.Ativo).ToList();
                List<ItemVendaOutputDto> itensDaVendaNoFormatoOutput=new List<ItemVendaOutputDto>();

                foreach(ItemVenda itemIterado in itensDaVenda)
                {
                    decimal precoUnitarioDoProdutoDoItemComDescontoAplicado = itemIterado.PrecoUnitarioDoProdutoNaVendaSemDesconto - itemIterado.DescontoUnitario;
                    decimal valorTotalDoItem = precoUnitarioDoProdutoDoItemComDescontoAplicado * itemIterado.Quantidade;
                    ItemVendaOutputDto itemFormatoOutput = new ItemVendaOutputDto(itemIterado.IdProduto, itemIterado.Produto, itemIterado.DataCriacao,
                        itemIterado.Quantidade, itemIterado.DescontoUnitario, itemIterado.PrecoUnitarioDoProdutoNaVendaSemDesconto, precoUnitarioDoProdutoDoItemComDescontoAplicado, valorTotalDoItem);
                    itensDaVendaNoFormatoOutput.Add(itemFormatoOutput);
                }
                List<ServicoVenda> servicosVendas = _contexto.ServicosVendas.Include(sv => sv.Servico).Where(sv => sv.IdVenda == vendaIterada.Id && sv.Ativo).ToList();
                List<ServicoVendaOutputDto> servicosVendaFormatoOutput=new List<ServicoVendaOutputDto>();

                foreach (ServicoVenda servicoVendaIterado in servicosVendas)
                {
                    decimal precoDoServicoVendaComDescontoAplicado = servicoVendaIterado.PrecoServicoNaVendaSemDesconto - servicoVendaIterado.DescontoServico;
                    ServicoVendaOutputDto servicoVendaFormatoOutput = new ServicoVendaOutputDto(servicoVendaIterado.Id, servicoVendaIterado.Servico, servicoVendaIterado.DataCriacao,
                        servicoVendaIterado.DescontoServico, servicoVendaIterado.PrecoServicoNaVendaSemDesconto, precoDoServicoVendaComDescontoAplicado);

                    servicosVendaFormatoOutput.Add(servicoVendaFormatoOutput);
                }


                VendaOutputDto vendaOutput = new VendaOutputDto(vendaIterada.Id, vendaIterada.CodigoVenda, vendaIterada.DataCriacao, vendaIterada.DescontoTotal, vendaIterada.ValorTotalSemDesconto, vendaIterada.ValorTotalComDesconto,
                    vendaIterada.Cliente, itensDaVendaNoFormatoOutput, servicosVendaFormatoOutput);

                Transacao? transacaoDaVenda = _contexto.Transacoes.FirstOrDefault(t => t.IdVenda == vendaIterada.Id && vendaIterada.Ativo);

                if(transacaoDaVenda == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(500, "Transacao nunca deveria ser nula");
                }

                int quantidadeDeParcelasNaoPagas = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Pago == false && p.Ativo).Count();

                int quantidadeDeParcelasPagas = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Pago && p.Ativo).Count();

                decimal valorPagoDaTransacao = Math.Round((_contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Pago && p.Ativo).Sum(p => p.ValorParcela)),2,MidpointRounding.AwayFromZero);

                TransacaoOutputDto transacaoOutput = new TransacaoOutputDto(transacaoDaVenda.Id, transacaoDaVenda.DataCriacao, transacaoDaVenda.TipoPagamento, transacaoDaVenda.MeioPagamento,
                    transacaoDaVenda.TransacaoEmCurso, transacaoDaVenda.Pago, quantidadeDeParcelasNaoPagas, quantidadeDeParcelasPagas, valorPagoDaTransacao);

                VendaTransacaoOutputDto vendaTransacaoOutput = new VendaTransacaoOutputDto(vendaOutput, transacaoOutput);

                vendasDoClienteNoFormatoDto.Add(vendaTransacaoOutput);
            }
            return vendasDoClienteNoFormatoDto;
        }
        
    }
}
