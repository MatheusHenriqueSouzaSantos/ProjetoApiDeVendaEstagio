using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
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
using ApiEstagioBicicletaria.Services.LogServices.InterfacesLog;
using ApiEstagioBicicletaria.Validacao;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace ApiEstagioBicicletaria.Services
{
    public class VendedorService : IVendedorService
    {

        private ContextoDb _contexto;
        private IVendedorLogService _logService;
        private readonly Usuario _usuarioLogado;

        public VendedorService(ContextoDb contexto, IVendedorLogService logService, IUsuarioLogadoService usuarioLogadoService)
        {
            _contexto = contexto;
            _logService = logService;
            _usuarioLogado = usuarioLogadoService.ObterUsuario();
        }

        public List<Vendedor> BuscarTodosOsVendedoresAtivos()
        {
            return _contexto.Vendedores.Where(v=>v.Ativo).ToList();
        }

        public List<Vendedor> BuscarTodosOsVendedoresInativos()
        {
            return _contexto.Vendedores.Where(v => !v.Ativo).ToList();
        }

        public Vendedor BuscarVendedorAtivoPorId(Guid id)
        {
            return _contexto.Vendedores.FirstOrDefault(v=>v.Id == id && v.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404,"Vendedor não encontrado");
        }

        public Vendedor BuscarVendedorPorCpf(string cpf)
        {
            string cpfSemPontoETracos = DocumentoUtil.RemoverPontosTracosEBarras(cpf);
            if (!DocumentoUtil.VerificarSeAStringContemSomenteNumeros(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O cpf deve conter apenas números");
            }
            if (!DocumentoUtil.ValidarCpf(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            return _contexto.Vendedores.FirstOrDefault(v => v.Cpf == cpfSemPontoETracos && v.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Vendedor não encontrado");
        }

        public List<Vendedor> BuscarVendedoresPorNome(string nome)
        {
            return _contexto.Vendedores.Where(v => v.NomeCompleto.Contains(nome) && v.Ativo).Take(10).ToList();
        }


        public Vendedor CadastrarVendedor(VendedorCreateDto dto)
        {
            string cpfSemPontoETracos = DocumentoUtil.RemoverPontosTracosEBarras(dto.Cpf);
            if (!DocumentoUtil.VerificarSeAStringContemSomenteNumeros(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O cpf deve conter apenas números");
            }
            if (!DocumentoUtil.ValidarCpf(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            bool vendedorExistenteComEsseCpf = _contexto.Vendedores.Any(v => v.Cpf == cpfSemPontoETracos);
            if (vendedorExistenteComEsseCpf)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um vendedor cadastrado com esse cpf");
            }
            bool vendedorCadastradoComEsseEmail = _contexto.Vendedores.Any(v => v.Email == dto.Email);

            if (vendedorCadastradoComEsseEmail)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um vendedor cadastrado com esse email");
            }

            Vendedor vendedor = new Vendedor(dto.Telefone,dto.Email,dto.NomeCompleto, cpfSemPontoETracos);

            _contexto.Vendedores.Add(vendedor);
            _logService.CriarLogsDeCriacao(vendedor, _usuarioLogado);
            _contexto.SaveChanges();

            return vendedor;
        }
        public Vendedor AtualizarVendedor(Guid id,VendedorUpdatedDto dto)
        {
            Vendedor vendedor = _contexto.Vendedores.FirstOrDefault(v => v.Id == id)
                ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");

            if (vendedor.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Vendedor Inativo, reative-o antes para poder atualiza-lo");
            }
            
            bool vendedorCadastradoComEsseEmail = _contexto.Vendedores.Any(v => v.Email == dto.Email && v.Id!=id);

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

        public void InativarVendedor(Guid id)
        {
            Vendedor vendedor = _contexto.Vendedores.FirstOrDefault(v => v.Id == id)
               ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");

            if (vendedor.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Vendedor já está inativo");
            }
            vendedor.Ativo = false;
            _contexto.Vendedores.Update(vendedor);
            _logService.CriarLogsDeInativacao(vendedor, _usuarioLogado);
            _contexto.SaveChanges();
        }

        public void ReativarVendedor(Guid id)
        {
            Vendedor vendedor = _contexto.Vendedores.FirstOrDefault(v => v.Id == id)
               ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");

            if (vendedor.Ativo == true)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Vendedor já está ativo");
            }
            vendedor.Ativo = true;
            _contexto.Vendedores.Update(vendedor);
            _logService.CriarLogsDeReativacao(vendedor, _usuarioLogado);
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
                            v => v.IdVendedor,
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

        
        public List<VendedorLogOutputDto> BuscarLogsPorIdVendedor(Guid id)
        {
            Vendedor vendedor=_contexto.Vendedores.FirstOrDefault(v=>v.Id==id)
                ?? throw new ExcecaoDeRegraDeNegocio(404,"Vendedor não encontrado");

            List<VendedorLog> logs = _contexto.VendedorLogs
                .Where(l => l.IdVendedor == vendedor.Id).ToList();

            List<VendedorLogOutputDto> logsDto =
                logs.Select(l => new VendedorLogOutputDto
                (l.IdVendedor,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            return logsDto.OrderByDescending(l=>l.DataCriacao).ToList();
        }
        public List<VendedorLogOutputDto> BuscarLogsPorCpf(string cpf)
        {
            string cpfSemPontoETracos = DocumentoUtil.RemoverPontosTracosEBarras(cpf);
            if (!DocumentoUtil.VerificarSeAStringContemSomenteNumeros(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O cpf deve conter apenas números");
            }
            if (!DocumentoUtil.ValidarCpf(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            Vendedor vendedor=_contexto.Vendedores.FirstOrDefault(v=>v.Cpf == cpfSemPontoETracos)
            ?? throw new ExcecaoDeRegraDeNegocio(404, "Vendedor não encontrado");
            List<VendedorLog> logs = _contexto.VendedorLogs
                .Where(l => l.IdVendedor==vendedor.Id).ToList();

            List<VendedorLogOutputDto> logsDto =
                logs.Select(l => new VendedorLogOutputDto
                (l.IdVendedor,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            return logsDto.OrderByDescending(l=>l.DataCriacao).ToList();
        }
    }
}
