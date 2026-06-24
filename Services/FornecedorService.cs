using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using ApiEstagioBicicletaria.Validacao;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace ApiEstagioBicicletaria.Services
{
    public class FornecedorService : IFornecedorService
    {

        private ContextoDb _contexto;

        private FornecedorRepositorio _fornecedorRepositorio;

        private FornecedorLogService _logService;

        private Usuario _usuarioLogado;

        public FornecedorService(ContextoDb contexto, FornecedorRepositorio fornecedorRepositorio, FornecedorLogService logService, 
            UsuarioLogadoService usuarioLogadoService)
        {
            _contexto = contexto;
            _fornecedorRepositorio = fornecedorRepositorio;
            _logService = logService;
            _usuarioLogado = usuarioLogadoService.ObterUsuario();
        }

        public List<Fornecedor> BuscarTodos()
        {
            return _contexto.Fornecedores.Where(f => f.Ativo).ToList();
        }

        public Fornecedor BuscarPorId(Guid id)
        {
            return _contexto.Fornecedores.FirstOrDefault(e=>e.Id==id && e.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404,"Fornecedor nao encontrado");
        }
       

        public Fornecedor BuscarPorCnpj(string cnpj)
        {
            string cnpjSomenteNumeros=DocumentoUtil.RemoverPontosTracosEBarras(cnpj);

            if (!DocumentoUtil.ValidarCnpj(cnpjSomenteNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O CNPJ deve estar em um formato valido");
            }

            return _contexto.Fornecedores.FirstOrDefault(e => e.Cnpj == cnpjSomenteNumeros && e.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Fornecedor nao encontrado");
        }

        public List<Fornecedor> BuscarPorNome(string nome)
        {
            return _contexto.Fornecedores.Where(v => v.RazaoSocial.Contains(nome) && v.Ativo).Take(10).ToList();
        }


        public Fornecedor Cadastrar(FornecedorCreateDto dto)
        {
            string cnpjSomenteNumeros = DocumentoUtil.RemoverPontosTracosEBarras(dto.Cnpj);

            if (!DocumentoUtil.ValidarCnpj(cnpjSomenteNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O CNPJ deve estar em um formato valido");
            }
            if(_contexto.Fornecedores.Any(f=>f.Cnpj==cnpjSomenteNumeros && f.Ativo))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um fornecedor cadastrado com esse cnpj");
            }
            if(_contexto.Fornecedores.Any(f=>f.Email == dto.Email && f.Ativo))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um fornecedor cadastrado com esse email");
            }
            Fornecedor fornecedor = new(dto.Telefone, dto.Email, dto.RazaoSocial, dto.NomeFantasia, cnpjSomenteNumeros, dto.InscricaoEstadual);
            _contexto.Add(fornecedor);
            _logService.CriarLogsDeCriacao(fornecedor,_usuarioLogado);
            _contexto.SaveChanges();
            return fornecedor;
        }

        public Fornecedor Atualizar(Guid id, FornecedorUpdateDto dto)
        {
            Fornecedor fornecedorVindoDoBanco = _contexto.Fornecedores.FirstOrDefault(f => f.Id == id && f.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Fornecedor não encontrado");
            if (_contexto.Fornecedores.Any(f => f.Email == dto.Email && f.Ativo && f.Id!=id))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um fornecedor cadastrado com esse email");
            }
            Fornecedor fornecedorAntigo = fornecedorVindoDoBanco.Copia();

            fornecedorVindoDoBanco.Telefone=dto.Telefone;
            fornecedorVindoDoBanco.Email=dto.Email;
            fornecedorVindoDoBanco.RazaoSocial=dto.RazaoSocial;
            fornecedorVindoDoBanco.NomeFantasia=dto.NomeFantasia;
            fornecedorVindoDoBanco.InscricaoEstadual=dto.InscricaoEstadual;
            _contexto.Fornecedores.Update(fornecedorVindoDoBanco);
            _logService.CriarLogsDeAtualizacao(fornecedorAntigo,fornecedorVindoDoBanco,_usuarioLogado);
            _contexto.SaveChanges();
            return fornecedorVindoDoBanco; 

        }

        public void Desativar(Guid id)
        {
            //não deixar ser desativado se fornecedor já tiver feito uma entrada de estoque
            Fornecedor fornecedorVindoDoBanco = _contexto.Fornecedores.FirstOrDefault(f => f.Id == id && f.Ativo)
               ?? throw new ExcecaoDeRegraDeNegocio(404, "Fornecedor não encontrado");
            fornecedorVindoDoBanco.Ativo = false;
            _contexto.Fornecedores.Update(fornecedorVindoDoBanco);
            _logService.CriarLogsDeExclusao(fornecedorVindoDoBanco, _usuarioLogado);
            _contexto.SaveChanges();

        }

        public byte[] GerarRelatorioFornecedoresComMaiorVolumeDeEntradaPorPeriodo(DatasParaGeracaoDeRelatorioDto dto)
        {
            DateTime dataInicialDoPeriodoConvertidaDateTime;

            DateTime dataFinalDoPeriodoConvertidaDateTime;

            bool sucessoAoFazerConversaoDataInicio = DateTime.TryParseExact(
                    dto.DataDeInicioDoPeriodo,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dataInicialDoPeriodoConvertidaDateTime
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
                   out dataFinalDoPeriodoConvertidaDateTime
           );
            if (!sucessoAoFazerConversaoDataFinal)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Data final está no formato inválido");
            }

            if (dataInicialDoPeriodoConvertidaDateTime > dataFinalDoPeriodoConvertidaDateTime)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A data de inícion deve ser antes ou igual da data final");
            }

            dataFinalDoPeriodoConvertidaDateTime = dataFinalDoPeriodoConvertidaDateTime.AddDays(1).AddTicks(-1);

            List<FornecedorComMaisEntradasDto> fornecedoresComDadosEntrada =
                _contexto.EntradasEstoque.Where(e => e.Ativo && e.DataCriacao >= dataInicialDoPeriodoConvertidaDateTime
                && e.DataCriacao <= dataFinalDoPeriodoConvertidaDateTime)
                .Include(e => e.Itens)
                .GroupBy(e => new
                {
                    e.IdFornecedor,
                    e.Fornecedor.Cnpj,
                    e.Fornecedor.RazaoSocial
                }).ToList()
                .Select(g=>new FornecedorComMaisEntradasDto(
                    g.Key.RazaoSocial,
                    g.Key.Cnpj,
                    g.Count(),
                    g.Sum(e=>e.Itens.Count()),
                    g.Sum(e=>e.Itens.Sum(it=>it.Quantidade))
                ))
                .OrderByDescending(fce=>fce.QuantidadeTotalDosItens)
                .ToList();

            QuestPDF.Settings.License = LicenseType.Community;

            var modeloDocumento = new RelatorioFornecedoresComMaiorVolumeDeEntradaPorPeriodo(fornecedoresComDadosEntrada,
                DateOnly.FromDateTime(dataInicialDoPeriodoConvertidaDateTime), DateOnly.FromDateTime(dataFinalDoPeriodoConvertidaDateTime));

            return modeloDocumento.GeneratePdf();

        }

        public List<FornecedorLogOutputDto> BuscarLogsPorIdFornecedor(Guid id)
        {
            List<FornecedorLog> logs = _contexto.FornecedorLogs
                .Where(l => l.IdFornecedor == id).OrderByDescending(l => l.DataCriacao).ToList();

            List<FornecedorLogOutputDto> logsDto =
                logs.Select(l => new FornecedorLogOutputDto
                (l.IdFornecedor,
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
