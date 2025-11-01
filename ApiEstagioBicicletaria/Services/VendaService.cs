using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
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

            List<VendaTransacaoOutputDto> vendasTransacoes=new List<VendaTransacaoOutputDto>();

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
                vendasTransacoes.Add(vendaTransacaoOutputDto);
            }

            return vendasTransacoes;

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
                throw new ExcecaoDeRegraDeNegocio(404, "Cliente não encontrada!!!");
            }
            List<ItemVenda> item 
            Venda vendaASerCriada= new Venda(clienteDaVenda,clienteDaVenda.Id,dto.Venda.Desconto,dto.Venda.//colocar o valor da soma de todas as parcelas)
        }
    }
}
