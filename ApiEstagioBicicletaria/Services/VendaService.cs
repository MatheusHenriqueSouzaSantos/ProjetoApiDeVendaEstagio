using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Services
{
    public class VendaService
    {
        private ContextoDb _contexto;

        public List<VendaTransacaoOutputDto> BuscarTodasVendas()
        {
            List<Venda> vendas = _contexto.Vendas.Include(v => v.Cliente).Where(v=>v.Ativo).ToList();
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
                List<ServicoVenda> servicoDaVenda = _contexto.ServicosVendas.Include(s => s.Servico).Where(s => s.IdVenda == venda.Id && s.Ativo).ToList();
                Transacao transacaoDaVenda = _contexto.Transacoes.Where(t => t.IdVenda == venda.Id && t.Ativo).FirstOrDefault();
                List<Parcela> parcelasDaTranscao = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Ativo).ToList();

                int QuantidadeDeParcelasVenda=parcelasDaTranscao.Count();
                int QuantidadeDeParcelasPagasVenda=parcelasDaTranscao.Where(p=>p.Pago).Count();
                decimal valorPago = parcelasDaTranscao.Where(p => p.Pago).Sum(p => p.ValorParcela);

                List<ItemVendaOutputDto> itensVendaFormatoDtoOutput=new List<ItemVendaOutputDto>();

                List<ServicoVendaOutputDto> servicosVendaFormatoDtoOutput=new List<ServicoVendaOutputDto>();

                foreach(ItemVenda item in itensDaVenda)
                {
                    ItemVendaOutputDto itemVendaOutputDto = new ItemVendaOutputDto(item.Id,item.Produto,item.DataCriacao,item.Quantidade,item.DescontoUnitario,item.PrecoUnitarioNaVenda);
                    itensVendaFormatoDtoOutput.Add(itemVendaOutputDto);
                }
                foreach (ServicoVenda servicoVenda in servicoDaVenda)
                {
                    ServicoVendaOutputDto servicoVendaOutputDto = new ServicoVendaOutputDto(servicoVenda.Id,servicoVenda.Servico,servicoVenda.DataCriacao,servicoVenda.DescontoServico,servicoVenda.PrecoServicoNaVenda);
                    servicosVendaFormatoDtoOutput.Add(servicoVendaOutputDto);
                }


                VendaOutputDto vendaOutputDto = new VendaOutputDto(venda.Id, venda.DataCriacao, venda.Desconto, venda.ValorTotal, venda.Cliente,
                    itensVendaFormatoDtoOutput, servicosVendaFormatoDtoOutput);
                TransacaoOutputDto transacaoOutputDto = new TransacaoOutputDto(transacaoDaVenda.Id, transacaoDaVenda.DataCriacao, transacaoDaVenda.TipoPagamento,
                    transacaoDaVenda.MeioPagamento, transacaoDaVenda.Pago, transacaoDaVenda.TransacaoEmCurso,QuantidadeDeParcelasVenda,QuantidadeDeParcelasPagasVenda,valorPago);

                VendaTransacaoOutputDto vendaTransacaoOutputDto = new VendaTransacaoOutputDto(vendaOutputDto, transacaoOutputDto);
                ListaComTodasVendasTransacoes.Add(vendaTransacaoOutputDto);
            }

            return ListaComTodasVendasTransacoes;

        }
        
        public VendaTransacaoOutputDto BuscarVendaPorId(Guid id)
        {
            Venda? venda = _contexto.Vendas.Include(v => v.Cliente).FirstOrDefault(v=>v.Id==id && v.Ativo);
            if (venda == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404,"Venda não encontrada!!!");
            }

            VendaTransacaoOutputDto vendaTransacaoOutputDto;

            List<ItemVenda> itensDaVenda = _contexto.ItensVendas.Include(i => i.Produto).Where(i => i.IdVenda == venda.Id && i.Ativo).ToList();
            List<ServicoVenda> servicoDaVenda = _contexto.ServicosVendas.Include(s => s.Servico).Where(s => s.IdVenda == venda.Id && s.Ativo).ToList();
            Transacao transacaoDaVenda = _contexto.Transacoes.Where(t => t.IdVenda == venda.Id && t.Ativo).FirstOrDefault();
            List<Parcela> parcelasDaTranscao = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoDaVenda.Id && p.Ativo).ToList();

            int QuantidadeDeParcelasVenda = parcelasDaTranscao.Count();
            int QuantidadeDeParcelasPagasVenda = parcelasDaTranscao.Where(p => p.Pago).Count();
            decimal valorPago = parcelasDaTranscao.Where(p => p.Pago).Sum(p => p.ValorParcela);

            List<ItemVendaOutputDto> itensVendaFormatoDtoOutput = new List<ItemVendaOutputDto>();

            List<ServicoVendaOutputDto> servicosVendaFormatoDtoOutput = new List<ServicoVendaOutputDto>();

            foreach (ItemVenda item in itensDaVenda)
            {
                ItemVendaOutputDto itemVendaOutputDto = new ItemVendaOutputDto(item.Id, item.Produto, item.DataCriacao, item.Quantidade, item.DescontoUnitario, item.PrecoUnitarioNaVenda);
                itensVendaFormatoDtoOutput.Add(itemVendaOutputDto);
            }
            foreach (ServicoVenda servicoVenda in servicoDaVenda)
            {
                ServicoVendaOutputDto servicoVendaOutputDto = new ServicoVendaOutputDto(servicoVenda.Id, servicoVenda.Servico, servicoVenda.DataCriacao, servicoVenda.DescontoServico, servicoVenda.PrecoServicoNaVenda);
                servicosVendaFormatoDtoOutput.Add(servicoVendaOutputDto);
            }


            VendaOutputDto vendaOutputDto = new VendaOutputDto(venda.Id, venda.DataCriacao, venda.Desconto, venda.ValorTotal, venda.Cliente,
                itensVendaFormatoDtoOutput, servicosVendaFormatoDtoOutput);
            TransacaoOutputDto transacaoOutputDto = new TransacaoOutputDto(transacaoDaVenda.Id, transacaoDaVenda.DataCriacao, transacaoDaVenda.TipoPagamento,
                transacaoDaVenda.MeioPagamento, transacaoDaVenda.Pago, transacaoDaVenda.TransacaoEmCurso, QuantidadeDeParcelasVenda, QuantidadeDeParcelasPagasVenda, valorPago);

            vendaTransacaoOutputDto = new VendaTransacaoOutputDto(vendaOutputDto, transacaoOutputDto);

            return vendaTransacaoOutputDto;
        }

        public VendaTransacaoOutputDto CadastrarVenda(VendaTransacaoInputDto dto)
        {
            Cliente? clienteDaVenda = _contexto.Clientes.Where(c => c.Id == dto.Venda.IdCliente && c.Ativo).FirstOrDefault();
            if (clienteDaVenda == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Cliente não encontrado!!!");
            }
            List<ItemVendaInputDto> itensVenda = dto.Venda.ItensVenda;
            List<ServicoVendaInputDto> servicosVenda = dto.Venda.ServicosVenda;
            decimal totalDaVenda=CalcularTotalVenda(itensVenda,servicosVenda);
            decimal descontoVenda = dto.Venda.Desconto ?? 0.0m;

            if(itensVenda.Count<1 && servicosVenda.Count < 1)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"A venda deve conter pelo menos um item ou serviço");
            }

            Venda vendaCriada = new Venda(clienteDaVenda, clienteDaVenda.Id, descontoVenda, totalDaVenda);

            List<ItemVenda> listaDeItensDaVendaCriada = new List<ItemVenda>();

            List<ServicoVenda> listaDeServicosDaVendaCriada =new List<ServicoVenda>();

            foreach(ItemVendaInputDto itemEnviado in dto.Venda.ItensVenda.ToList())
            {
                Produto? produtoDoItem = _contexto.Produtos.FirstOrDefault(p => p.Id == itemEnviado.IdProduto && p.Ativo);

                if(produtoDoItem == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado!!!");
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
                decimal descontoDoServico = servicoEnviado.DescontoServico ?? 0.0m;
                if (descontoDoServico > servicoDaVenda.PrecoServico)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "O desconto do serviço não dever ser maior do que o valor do serviço");
                }
                ServicoVenda servicoDaVendaCriado = new ServicoVenda(vendaCriada, servicoDaVenda, descontoDoServico, servicoDaVenda.PrecoServico);
                listaDeServicosDaVendaCriada.Add(servicoDaVendaCriado);
                _contexto.ServicosVendas.Add(servicoDaVendaCriado);
            }

            if(dto.Transacao.TipoPagamento== TipoPagamento.AVista && dto.Transacao.QuantidadeDeParcelas != 1)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Uma venda à vista deve ter apenas uma parcela");
            }

            Transacao transacaoCriada = new Transacao(vendaCriada, dto.Transacao.TipoPagamento, dto.Transacao.MeioPagamaneto);
            //validar se valores são maior que 0, já fiz essa validação???
            decimal valorDeCadaParcela = vendaCriada.ValorTotal / dto.Transacao.QuantidadeDeParcelas;

            for (int i=0;i<dto.Transacao.QuantidadeDeParcelas; i++)
            {
                Parcela parcelaCriada = new Parcela(transacaoCriada, (i + 1), valorDeCadaParcela);
                _contexto.Parcelas.Add(parcelaCriada);
            }
            _contexto.SaveChanges();

            List<ItemVendaOutputDto> listaDeItensASeremRetornados= new List<ItemVendaOutputDto>();
            List<ServicoVendaOutputDto> listaDeServicosASeremRetornados= new List<ServicoVendaOutputDto>();

            foreach(ItemVenda itemNoFormatoModel in listaDeItensDaVendaCriada)
            {
                ItemVendaOutputDto itemNoFormatoDeOutput = new ItemVendaOutputDto(itemNoFormatoModel.Id, itemNoFormatoModel.Produto, itemNoFormatoModel.DataCriacao,
                    itemNoFormatoModel.Quantidade, itemNoFormatoModel.DescontoUnitario, itemNoFormatoModel.PrecoUnitarioNaVenda);

                listaDeItensASeremRetornados.Add(itemNoFormatoDeOutput);
            }

            foreach(ServicoVenda servicoVendaNoFormatoModel in listaDeServicosDaVendaCriada)
            {
                ServicoVendaOutputDto servicoVendaFormatoDeOutput = new ServicoVendaOutputDto(servicoVendaNoFormatoModel.Id,servicoVendaNoFormatoModel.Servico,servicoVendaNoFormatoModel.DataCriacao,
                    servicoVendaNoFormatoModel.DescontoServico,servicoVendaNoFormatoModel.PrecoServicoNaVenda);

                listaDeServicosASeremRetornados.Add(servicoVendaFormatoDeOutput);
            }//regra no meio de pagamento, tipo pagamento a prazo não pode ser dinheiro??
            VendaOutputDto vendaASerRetornadaNoFormatoOutput = new VendaOutputDto(vendaCriada.Id, vendaCriada.DataCriacao, vendaCriada.Desconto, vendaCriada.ValorTotal,
                vendaCriada.Cliente,listaDeItensASeremRetornados,listaDeServicosASeremRetornados);
            int quantidadeDeParcelas = _contexto.Parcelas.Where(p => p.IdTransacao == transacaoCriada.Id && p.Ativo).Count();
            int numeroDeParcelasPagas = 0;
            decimal valorPago = 0.0m;
            TransacaoOutputDto transacaoASerRetornadaNoFormatoOutput = new TransacaoOutputDto(transacaoCriada.Id, transacaoCriada.DataCriacao, transacaoCriada.TipoPagamento,
                transacaoCriada.MeioPagamento,transacaoCriada.TransacaoEmCurso,transacaoCriada.Pago,quantidadeDeParcelas,numeroDeParcelasPagas,valorPago);

            VendaTransacaoOutputDto vendaTransacaoASerRetornadaFormatoOutput = new(vendaASerRetornadaNoFormatoOutput,transacaoASerRetornadaNoFormatoOutput);

            return vendaTransacaoASerRetornadaFormatoOutput;

            //fazer um endpoint para registrar pagamento de venda mesmo a vista, separar responsabilidade, de registrar pagamento

        }
        public decimal CalcularTotalVenda(List<ItemVendaInputDto> itensVenda, List<ServicoVendaInputDto> servicosVenda)
        {
            decimal totalVenda=0;
            foreach (ItemVendaInputDto itemIterado in itensVenda)
            {
                Produto? produtoVindoDoBanco = _contexto.Produtos.FirstOrDefault(p => p.Id == itemIterado.IdProduto && p.Ativo);
                if (produtoVindoDoBanco == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404,$"Produto com id {itemIterado.IdProduto} não encontrado");
                }
                decimal precoDoProdutoComDescontoAplicado = produtoVindoDoBanco.PrecoUnitario - (itemIterado.DescontoUnitario ?? 0.0m);
                decimal totalItem = precoDoProdutoComDescontoAplicado * itemIterado.Quantidade;
                totalVenda += totalItem;
            }
            foreach (ServicoVendaInputDto servicoIterado in servicosVenda)
            {
                Servico? servicoVindoDoBAnco = _contexto.Servicos.FirstOrDefault(s=>s.Id==servicoIterado.IdServico && s.Ativo);
                if (servicoVindoDoBAnco == null)
                {
                    throw new ExcecaoDeRegraDeNegocio(404, $"Serviço com id {servicoIterado.IdServico} não encontrado");
                }
                decimal precoDoServicoComDescontoAplicado = servicoVindoDoBAnco.PrecoServico - (servicoIterado.DescontoServico ?? 0.0m);
                decimal totalServico = precoDoServicoComDescontoAplicado;
                totalVenda += totalServico;
            }
            return totalVenda;
        }

        public void AbaterQuantidadeEmEstoque(Produto produto, int quantidadeASerAbatida)
        {
            int novaQuantidadeEmEstoque = produto.QuantidadeEmEstoque - quantidadeASerAbatida;
            produto.QuantidadeEmEstoque=novaQuantidadeEmEstoque;
            _contexto.Produtos.Update(produto);
        }

        public TransacaoOutputDto AtualizarQuantidadeDeParcelasPagasEmUmaTransacao(Guid idTransacao, int quantidadeDeParcelasPagas)
        {

        }
    }
}
