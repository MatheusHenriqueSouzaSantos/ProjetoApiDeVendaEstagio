using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.EstoqueDtos;
using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using ApiEstagioBicicletaria.Services.ServicesLogs;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ApiEstagioBicicletaria.Services
{
    public class ProdutoService : IProdutoService
    {
        //implementar mexer em estoque no lugar de estoque em produto
        //private readonly int _numeroMaximoDePaginas = 5;
        //private readonly int _numeroDeLinhasPorPagina = 42;
        private readonly ContextoDb _contextoDb;
        private readonly ProdutoLogService _produtoLogService;
        private readonly EstoqueLogService _estoqueLogService;
        private readonly Usuario _usuarioLogado;

        public ProdutoService(ContextoDb contextoDb, ProdutoLogService produtoLogService, EstoqueLogService estoqueLogService, UsuarioLogadoService userLogadoService)
        {
            _contextoDb = contextoDb;
            _produtoLogService = produtoLogService;
            _estoqueLogService = estoqueLogService;
            _usuarioLogado = userLogadoService.ObterUsuario();
        }

        public List<ProdutoDtoOutPut> BuscarProdutosAtivos()
        {
            List<Produto> produtosVindoDoBanco= _contextoDb.Produtos.Where(p=>p.Ativo).ToList();

            List<ProdutoDtoOutPut> produtosFomartoDto = new List<ProdutoDtoOutPut>();
            
            foreach(Produto produtoIterado in produtosVindoDoBanco)
            {
                Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoIterado.Id) 
                    ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
                EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id,estoque.QuantidadeEmEstoque);
                ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoIterado.Id,produtoIterado.CodigoDeBarra,produtoIterado.DataCriacao,produtoIterado.NomeProduto,produtoIterado.Descricao,
                    produtoIterado.Preco,produtoIterado.Ativo,estoqueDto);
                produtosFomartoDto.Add(produtoFormatoDto);
            }
            return produtosFomartoDto;
        }

        public List<ProdutoInativoOutputDto> BuscarProdutosInativos()
        {
            List<Produto> produtosVindoDoBanco = _contextoDb.Produtos.Where(p => !p.Ativo).ToList();

            List<ProdutoInativoOutputDto> produtosFomartoDto = new ();

            foreach (Produto produtoIterado in produtosVindoDoBanco)
            {
                Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoIterado.Id)
                    ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
                EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id, estoque.QuantidadeEmEstoque);
                ProdutoInativoOutputDto produtoFormatoDto = new (produtoIterado.Id, produtoIterado.CodigoDeBarra, produtoIterado.DataCriacao, 
                    produtoIterado.NomeProduto, produtoIterado.Descricao,produtoIterado.Preco, produtoIterado.Ativo, estoqueDto);
                produtosFomartoDto.Add(produtoFormatoDto);
            }
            return produtosFomartoDto;
        }
        public ProdutoDtoOutPut BuscarProdutoAtivoPorId(Guid id)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault();

            if (produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
            }
            Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoVindoDoBanco.Id)
                   ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id, estoque.QuantidadeEmEstoque);
            ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoVindoDoBanco.Id, produtoVindoDoBanco.CodigoDeBarra, produtoVindoDoBanco.DataCriacao,
                produtoVindoDoBanco.NomeProduto, produtoVindoDoBanco.Descricao, produtoVindoDoBanco.Preco,
                produtoVindoDoBanco.Ativo,estoqueDto);
            return produtoFormatoDto;
        }

        public ProdutoDtoOutPut BuscarProdutoAtivoPorCodigoDeBarra(string codigoDeBarra)
        {

            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.CodigoDeBarra == codigoDeBarra && p.Ativo).FirstOrDefault();

            if (produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Produto não encontrado");
            }
            Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoVindoDoBanco.Id)
                  ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id, estoque.QuantidadeEmEstoque);
            ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoVindoDoBanco.Id, produtoVindoDoBanco.CodigoDeBarra, produtoVindoDoBanco.DataCriacao,
                produtoVindoDoBanco.NomeProduto, produtoVindoDoBanco.Descricao, produtoVindoDoBanco.Preco, 
                produtoVindoDoBanco.Ativo, estoqueDto);
            return produtoFormatoDto;
        }

        public ProdutoDtoOutPut CadastrarProduto(ProdutoInputDto dto)
        {
            //depende do formato código de barra, por enaqunto livre
            if (string.IsNullOrWhiteSpace(dto.CodigoDeBarra))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código de barra não pode ser vazio");
            }
            Produto? produtoVindoDoBancoComMesmoCodigoDeBarra = _contextoDb
                .Produtos.Where(p => p.CodigoDeBarra == dto.CodigoDeBarra).FirstOrDefault();

            if(produtoVindoDoBancoComMesmoCodigoDeBarra != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um produto com esse código de barra!");
            }
            Produto produtoAInserirNoBanco = new Produto(dto.CodigoDeBarra,
                dto.NomeProduto, dto.Descricao, dto.PrecoUnitario);
            Estoque estoque = new(produtoAInserirNoBanco);
            estoque.Produto = produtoAInserirNoBanco;
            _contextoDb.Add(produtoAInserirNoBanco);
            _contextoDb.Add(estoque);
            _produtoLogService.CriarLogsDeCriacao(produtoAInserirNoBanco,_usuarioLogado);
            _estoqueLogService.CriarLogsDeCriacao(estoque,_usuarioLogado);
            _contextoDb.SaveChanges();
            return new ProdutoDtoOutPut(produtoAInserirNoBanco.Id,produtoAInserirNoBanco.CodigoDeBarra,produtoAInserirNoBanco.DataCriacao,produtoAInserirNoBanco.NomeProduto,
                produtoAInserirNoBanco.Descricao,produtoAInserirNoBanco.Preco,produtoAInserirNoBanco.Ativo, 
                new EstoqueSimplificadoOutputDto(estoque.Id,estoque.QuantidadeEmEstoque));

        }

        public ProdutoDtoOutPut AtualizarProduto(Guid id, ProdutoInputDto dto)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.FirstOrDefault(p => p.Id == id) 
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");

            if (produtoVindoDoBanco.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O produto está inativo, reative-o antes para atualiza-lo");
            }

            if (string.IsNullOrWhiteSpace(dto.CodigoDeBarra))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código de barra não pode ser vazio");
            }

            Produto? produtoVindoDoBancoComMesmoCodigoDeBarras = _contextoDb.Produtos.Where(p => p.CodigoDeBarra == dto.CodigoDeBarra && p.Id != id)
                .FirstOrDefault();


            if (produtoVindoDoBancoComMesmoCodigoDeBarras != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um produto com esse código de barras");
            }
            Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.ProdutoId == produtoVindoDoBanco.Id)
                  ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            Produto produtoSemAtualizar = produtoVindoDoBanco.Copia();

            produtoVindoDoBanco.CodigoDeBarra = dto.CodigoDeBarra;
            produtoVindoDoBanco.NomeProduto = dto.NomeProduto;
            produtoVindoDoBanco.Descricao = dto.Descricao;
            produtoVindoDoBanco.Preco = dto.PrecoUnitario;
            _contextoDb.Update(produtoVindoDoBanco);
            _produtoLogService.CriarLogsDeAtualizacao(produtoSemAtualizar,produtoVindoDoBanco,_usuarioLogado);
            _contextoDb.SaveChanges();
            return new ProdutoDtoOutPut(produtoVindoDoBanco.Id,produtoVindoDoBanco.CodigoDeBarra,produtoVindoDoBanco.DataCriacao,produtoVindoDoBanco.NomeProduto,
                produtoVindoDoBanco.Descricao,produtoVindoDoBanco.Preco,produtoVindoDoBanco.Ativo,new EstoqueSimplificadoOutputDto(estoque.Id,estoque.QuantidadeEmEstoque));
        }

        public void InativarProdutoPorId(Guid id)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id).FirstOrDefault()
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");

            if(produtoVindoDoBanco.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O produto já está inativo");
            }
            Estoque estoque = _contextoDb.Estoques.Include(e=>e.Produto).FirstOrDefault(e => e.Produto.Id == produtoVindoDoBanco.Id)
                  ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            produtoVindoDoBanco.Ativo = false;
            estoque.Ativo=false;
            _contextoDb.Produtos.Update(produtoVindoDoBanco);
            _contextoDb.Estoques.Update(estoque);
            _produtoLogService.CriarLogsDeInativacao(produtoVindoDoBanco,_usuarioLogado);
            _estoqueLogService.CriarLogsDeInativacao(estoque, _usuarioLogado);
            _contextoDb.SaveChanges();
        }

        public void ReativarProdutoPorId(Guid id)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id).FirstOrDefault()
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
            if (produtoVindoDoBanco.Ativo == true)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O produto já está Ativo");
            }
            Estoque estoque = _contextoDb.Estoques.Include(e=>e.Produto).FirstOrDefault(e => e.Produto.Id == produtoVindoDoBanco.Id)
                  ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            produtoVindoDoBanco.Ativo = true;
            estoque.Ativo=true;
            _contextoDb.Produtos.Update(produtoVindoDoBanco);
            _contextoDb.Estoques.Update(estoque);
            _produtoLogService.CriarLogsDeReativacao(produtoVindoDoBanco,_usuarioLogado);
            _estoqueLogService.CriarLogsDeReativacao(estoque, _usuarioLogado);
            _contextoDb.SaveChanges();
        }

        public List<ProdutoDtoOutPut> BuscarProdutosPorNome(string nome)
        {
            List<Produto> produtosVindoDoBanco = _contextoDb.Produtos.Where(p => p.NomeProduto.Contains(nome) && p.Ativo).Take(10).ToList();

            List<ProdutoDtoOutPut> produtosFomartoDto = new List<ProdutoDtoOutPut>();

            foreach (Produto produtoIterado in produtosVindoDoBanco)
            {
                Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoIterado.Id)
                   ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
                EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id, estoque.QuantidadeEmEstoque);
                ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoIterado.Id, produtoIterado.CodigoDeBarra,
                    produtoIterado.DataCriacao, produtoIterado.NomeProduto, produtoIterado.Descricao, 
                    produtoIterado.Preco, produtoIterado.Ativo, estoqueDto);
                produtosFomartoDto.Add(produtoFormatoDto);
            }
            return produtosFomartoDto;
        }

        public byte[] GerarRelatorioDeProdutosMaisVendidosPorPeriodo(DatasParaGeracaoDeRelatorioDto dto)
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

            if (dataDeInicioDoPeriodoFormatoDateOnly > dataDeFimDoPeriodoDateOnly)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A data de fim de periodo nao pode ser maior do que ha de inicio do periodo");
            }
            if(dataDeFimDoPeriodoDateOnly>dataDeInicioDoPeriodoFormatoDateOnly.AddDays(366))
            {
                throw new ExcecaoDeRegraDeNegocio(400,"O Período não deve ser maior que 366 dias");
            }

            dataDeInicioDoPeriodoConvertidaDateTime = dataDeInicioDoPeriodoFormatoDateOnly.ToDateTime(TimeOnly.MinValue);
            dataDeFimDoPeriodoConvertidaDateTime = dataDeFimDoPeriodoDateOnly.ToDateTime(TimeOnly.MaxValue);

            //int numeroDeRegistroASerBuscados = _numeroMaximoDePaginas * _numeroDeLinhasPorPagina;
            List<ProdutoMaisVendidoDto> produtosMaisVendidos = _contextoDb.
                ItensVendas.
                Where(iv => iv.Ativo && iv.Produto.Ativo && iv.DataCriacao>=dataDeInicioDoPeriodoConvertidaDateTime && iv.DataCriacao<=dataDeFimDoPeriodoConvertidaDateTime)
                .GroupBy(iv => iv.Produto.Id)
                .Select(g => new ProdutoMaisVendidoDto
                {
                    Produto= g.First().Produto,
                    QuantidadeVendida= g.Sum(x=>x.Quantidade),
                    Faturamento=g.Sum(x=>(x.PrecoUnitarioDoProdutoNaVendaSemDesconto-x.DescontoUnitario)*x.Quantidade)

                })
                .OrderByDescending(x=>x.QuantidadeVendida)
                .ThenByDescending(x=>x.Faturamento)
                .ToList();

            QuestPDF.Settings.License = LicenseType.Community;

            var modeloDocumento = new RelatorioProdutosMaisVendidosPorPeriodo(produtosMaisVendidos, dataDeInicioDoPeriodoFormatoDateOnly,dataDeFimDoPeriodoDateOnly);

            return modeloDocumento.GeneratePdf();

            
        }

        public byte[] GerarRelatorioDeProdutosComEstoqueAbaixoOuIgualUmaQuantidade(int quantidadeParaBuscarDosProdutosEmFalta)
        {
            //int numeroDeRegistroASerBuscados = _numeroMaximoDePaginas * _numeroDeLinhasPorPagina;
            if (quantidadeParaBuscarDosProdutosEmFalta < 0)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Não é possível buscar produtos com a quantidade em estoque menor do que 0");
            }
            if (quantidadeParaBuscarDosProdutosEmFalta > 150)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A Quantidade para se enquadrar em produtos em falta não deve ser maior que 150");
            }

            var produtosEmFaltaDto = _contextoDb.Produtos
                .Where(p => p.Ativo)
                .Join(_contextoDb.Estoques,
                    produto => produto.Id,
                    estoque => estoque.ProdutoId,
                    (produto, estoque) => new
                    {
                        produto.Id,
                        produto.CodigoDeBarra,
                        produto.NomeProduto,
                        produto.Preco,
                        estoque.QuantidadeEmEstoque
                    })
                .Where(x => x.QuantidadeEmEstoque <= quantidadeParaBuscarDosProdutosEmFalta)
                .OrderBy(x => x.QuantidadeEmEstoque)
                .Select(x => new ProdutoEmFaltaDto(
                    x.Id,
                    x.CodigoDeBarra,
                    x.NomeProduto,
                    x.Preco,
                    x.QuantidadeEmEstoque
                ))
                .ToList();

            var documento = new RelatorioDeProdutosEmFalta(produtosEmFaltaDto, quantidadeParaBuscarDosProdutosEmFalta);
            QuestPDF.Settings.License = LicenseType.Community;
            byte[] pdf = documento.GeneratePdf();

            return pdf;
        }

        public List<Object> BuscarLogsPorIdProduto(Guid idProdutoEnviado)
        {
            Produto produto = _contextoDb.Produtos.FirstOrDefault(p => p.Id == idProdutoEnviado)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");

            List<ProdutoLog> produtoLogs = _contextoDb.ProdutoLogs
                .Where(l => l.IdProduto == produto.Id).ToList();

            List<ProdutoLogOutputDto> produtoLogsDto =
                produtoLogs.Select(l => new ProdutoLogOutputDto
                (l.IdProduto,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            List<EstoqueLog> estoqueLogs = _contextoDb.EstoqueLogs
               .Where(l => l.IdProduto == produto.Id).Include(e=>e.Produto).ToList();

            List<EstoqueLogOutPutDto> estoqueLogsDto =
                estoqueLogs.Select(l => new EstoqueLogOutPutDto
                (l.IdEstoque,
                l.IdProduto,
                l.Produto.NomeProduto,
                l.AcaoQueAlterouEstoque,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            List<BaseLogOutputDto> dtoLogs = new List<BaseLogOutputDto>();

            dtoLogs.AddRange(produtoLogsDto);
            dtoLogs.AddRange(estoqueLogsDto);

            return dtoLogs.
                OrderByDescending(l => l.DataCriacao)
                .Cast<Object>()
                .ToList();

        }

        public List<Object> BuscarLogsPorCodigoDeBarra(string codigoDeBarra)
        {

            Produto produto = _contextoDb.Produtos.FirstOrDefault(p => p.CodigoDeBarra == codigoDeBarra)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");

            List<ProdutoLog> produtoLogs = _contextoDb.ProdutoLogs
                .Where(l => l.IdProduto == produto.Id).ToList();

            List<ProdutoLogOutputDto> produtoLogsDto =
                produtoLogs.Select(l => new ProdutoLogOutputDto
                (l.IdProduto,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            List<EstoqueLog> estoqueLogs = _contextoDb.EstoqueLogs
               .Where(l => l.IdProduto == produto.Id).Include(e => e.Produto).ToList();

            List<EstoqueLogOutPutDto> estoqueLogsDto =
                estoqueLogs.Select(l => new EstoqueLogOutPutDto
                (l.IdEstoque,
                l.IdProduto,
                l.Produto.NomeProduto,
                l.AcaoQueAlterouEstoque,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            List<BaseLogOutputDto> dtoLogs = new List<BaseLogOutputDto>();

            dtoLogs.AddRange(produtoLogsDto);
            dtoLogs.AddRange(estoqueLogsDto);

            return dtoLogs.
                OrderByDescending(l => l.DataCriacao)
                .Cast<Object>()
                .ToList();

        }

        //public List<BaseDtoLog> BuscarLogsGeraisPorPaginacao(int paginaASerBuscada, int quantidadeDeRegistrosPorPagina)
        //{
        //    List<ProdutoLog> produtoLogs = _contextoDb.ProdutoLogs.OrderByDescending(l=>l.DataCriacao).ToList();

        //    List<ProdutoLogDto> produtoLogsDto =
        //        produtoLogs.Select(l => new ProdutoLogDto
        //        (l.IdProduto,
        //        l.Acao,
        //        l.CampoAlterado,
        //        l.ValorAntigo,
        //        l.ValorNovo,
        //        l.IdUsuarioResponsavel,
        //        l.DataCriacao)).ToList();

        //    List<EstoqueLog> estoqueLogs = _contextoDb.EstoqueLogs.OrderByDescending(l=>l.DataCriacao).ToList();

        //    List<EstoqueLogDto> estoqueLogsDto =
        //        estoqueLogs.Select(l => new EstoqueLogDto
        //        (l.IdEstoque,
        //        l.Acao,
        //        l.CampoAlterado,
        //        l.ValorAntigo,
        //        l.ValorNovo,
        //        l.IdUsuarioResponsavel,
        //        l.DataCriacao)).ToList();

        //    List<BaseDtoLog> dtoLogs = new List<BaseDtoLog>();

        //    dtoLogs.AddRange(produtoLogsDto);
        //    dtoLogs.AddRange(estoqueLogsDto);

        //    return dtoLogs.OrderByDescending(l => l.DataCriacao).ToList();
        //}
    }    
}
