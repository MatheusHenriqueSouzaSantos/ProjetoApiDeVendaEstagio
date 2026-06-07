using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using ApiEstagioBicicletaria.Validacao;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace ApiEstagioBicicletaria.Services
{
    public class VendedorService : IVendedorService
    {

        private ContextoDb _contexto;
        private VendedorLogService _logService;
        private readonly Usuario _usuarioLogado;

        public VendedorService(ContextoDb contexto, VendedorLogService logService, UsuarioLogadoService usuarioLogadoService)
        {
            _contexto = contexto;
            _logService = logService;
            _usuarioLogado = usuarioLogadoService.ObterUsuario();
        }

        public List<Vendedor> BuscarTodosOsVendedores()
        {
            return _contexto.Vendedores.Where(v=>v.Ativo).ToList();
        }

        public Vendedor BuscarVendedorPorId(Guid id)
        {
            return _contexto.Vendedores.FirstOrDefault(v=>v.Id == id && v.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(400,"Vendedor não encontrado");
        }

        public Vendedor BuscarVendedorPorCpf(string cpf)
        {
            string cpfSoNumeros=DocumentoUtil.RemoverNaoNumericos(cpf);
            bool cpfValido= DocumentoUtil.ValidarCpf(cpfSoNumeros);
            if (!cpfValido)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            return _contexto.Vendedores.FirstOrDefault(v => v.Cpf == cpfSoNumeros && v.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");
        }

        public List<Vendedor> BuscarVendedoresPorNome(string nome)
        {
            return _contexto.Vendedores.Where(v => v.NomeCompleto.Contains(nome) && v.Ativo).Take(10).ToList();
        }


        public Vendedor CadastrarVendedor(VendedorCreateDto dto)
        {
            string cpfSoNumeros = DocumentoUtil.RemoverNaoNumericos(dto.Cpf);
            if (!DocumentoUtil.ValidarCpf(cpfSoNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            bool vendedorExistenteComEsseCpf = _contexto.Vendedores.Any(v => v.Cpf == cpfSoNumeros && v.Ativo);
            if (vendedorExistenteComEsseCpf)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um vendedor cadastrado com esse cpf");
            }
            bool vendedorCadastradoComEsseEmail = _contexto.Vendedores.Any(v => v.Email == dto.Email && v.Ativo);

            if (vendedorCadastradoComEsseEmail)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um vendedor cadastrado com esse email");
            }

            Vendedor vendedor = new Vendedor(dto.Telefone,dto.Email,dto.NomeCompleto, cpfSoNumeros);

            _contexto.Vendedores.Add(vendedor);
            _logService.CriarLogsDeCriacao(vendedor, _usuarioLogado);
            _contexto.SaveChanges();

            return vendedor;
        }
        public Vendedor AtualizarVendedor(Guid id,VendedorUpdatedDto dto)
        {
            Vendedor vendedor = _contexto.Vendedores.FirstOrDefault(v => v.Id == id && v.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");
            
            bool vendedorCadastradoComEsseEmail = _contexto.Vendedores.Any(v => v.Email == dto.Email && v.Ativo && v.Id!=id);

            if (vendedorCadastradoComEsseEmail)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um vendedor cadastrado com esse email");
            }

            Vendedor vendedorAntigo = vendedor.Copia();

            vendedor.Telefone = dto.Telefone;
            vendedor.Email = dto.Email;
            vendedor.NomeCompleto = dto.NomeCompleto;

            _contexto.Vendedores.Update(vendedor);
            _logService.CriarLogsDeAtualizacao(vendedorAntigo,vendedor,_usuarioLogado);
            _contexto.SaveChanges();
            return vendedor;
        }

        public void DesativarVendedor(Guid id)
        {
            Vendedor vendedor = _contexto.Vendedores.FirstOrDefault(v => v.Id == id && v.Ativo)
               ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");

            bool vendedorPresenteEmAlgumaVenda = _contexto.Vendas.Any(v => v.Vendedor.Id == id && v.Ativo);

            if (vendedorPresenteEmAlgumaVenda)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O Vendedor Está presente em uma venda!, A Venda deve ser excluída antes de poder excluír o cliente");
            }
            vendedor.Ativo = false;
            _contexto.Vendedores.Update(vendedor);
            _logService.CriarLogsDeExclusao(vendedor, _usuarioLogado);
            _contexto.SaveChanges();
        }

        public byte[] GerarRelatorioDeVendedoresComMaiorFaturamentoPorPeriodo(DatasParaGeracaoDeRelatorioDto dto)
        {
            DateTime dataDeInicioDoPeriodoConvertidaDateTime;

            DateTime dataDeFinalDoPeriodoConvertidaDateTime;

            bool sucessoAoFazerConversaoDataInicio = DateTime.TryParseExact(
                    dto.DataDeInicioDoPeriodo,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dataDeInicioDoPeriodoConvertidaDateTime
            );
            if (!sucessoAoFazerConversaoDataInicio)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Data de início está no formato inválido");
            }

            bool sucessoAoFazerConversaoDataFinal = DateTime.TryParseExact(
                   dto.DataFinalDoPeriodo,
                   "yyyy-MM-dd",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out dataDeFinalDoPeriodoConvertidaDateTime
           );
            if (!sucessoAoFazerConversaoDataFinal)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Data final está no formato inválido");
            }

            if (dataDeInicioDoPeriodoConvertidaDateTime > dataDeFinalDoPeriodoConvertidaDateTime)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A data de inícion deve ser antes ou igual da data final");
            }

            dataDeFinalDoPeriodoConvertidaDateTime = dataDeFinalDoPeriodoConvertidaDateTime.AddDays(1).AddTicks(-1);

            var dados = _contexto.Vendas
                        .Where(v => v.Ativo &&
                            v.DataCriacao >= dataDeInicioDoPeriodoConvertidaDateTime &&
                            v.DataCriacao <= dataDeFinalDoPeriodoConvertidaDateTime)
                        .Join(_contexto.Vendedores,
                            v => v.VendedorId,
                            ve => ve.Id,
                            (v, ve) => new { v, ve })
                        .GroupBy(x => new
                        {
                            x.ve.Id,
                            x.ve.NomeCompleto,
                            x.ve.Cpf
                        })
                        .Select(g => new
                        {
                            VendedorId = g.Key.Id,
                            Nome = g.Key.NomeCompleto,
                            Cpf = g.Key.Cpf,
                            TotalVendas = g.Count(),
                            Faturamento = g.Sum(x => x.v.ValorTotalComDesconto)
                        })
                        .OrderByDescending(x => x.Faturamento)
                        .ToList();

            var resultado = dados
                            .Select(x => new VendedorComMaiorFaturamentoPorPeriodo(
                                x.VendedorId,
                                x.Nome,
                                x.Cpf,
                                x.TotalVendas,
                                x.Faturamento
                            ))
                            .ToList();

            QuestPDF.Settings.License=LicenseType.Community;

            var modeloDocumento = new RelatorioDeVendedoresComMaiorFaturamentoPorPeriodo(resultado,
                DateOnly.FromDateTime(dataDeInicioDoPeriodoConvertidaDateTime),
                DateOnly.FromDateTime(dataDeFinalDoPeriodoConvertidaDateTime));

            return modeloDocumento.GeneratePdf();

        }


        public List<VendedorLogDto> BuscarLogsPorIdVendedor(Guid id)
        {
            List<VendedorLog> logs = _contexto.VendedorLogs
                .Where(l => l.IdVendedor == id).OrderByDescending(l=>l.DataCriacao).ToList();

            List<VendedorLogDto> logsDto =
                logs.Select(l => new VendedorLogDto
                (l.IdVendedor,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            return logsDto;
        }
    }
}
