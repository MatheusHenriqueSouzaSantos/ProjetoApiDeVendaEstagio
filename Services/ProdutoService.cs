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

        public List<ProdutoDtoOutPut> BuscarProdutos()
        {
            List<Produto> produtosVindoDoBanco= _contextoDb.Produtos.Where(p=>p.Ativo).ToList();

            List<ProdutoDtoOutPut> produtosFomartoDto = new List<ProdutoDtoOutPut>();
            
            foreach(Produto produtoIterado in produtosVindoDoBanco)
            {
                bool podeExcluir = !_contextoDb.ItensVendas.Any(iv => iv.IdProduto == produtoIterado.Id && iv.Ativo);
                Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoIterado.Id) 
                    ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
                EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id,estoque.QuantidadeEmEstoque);
                ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoIterado.Id,produtoIterado.CodigoDeBarra,produtoIterado.DataCriacao,produtoIterado.NomeProduto,produtoIterado.Descricao,
                    produtoIterado.Preco,produtoIterado.Ativo,podeExcluir,estoqueDto);
                produtosFomartoDto.Add(produtoFormatoDto);
            }
            return produtosFomartoDto;
        }
        public ProdutoDtoOutPut BuscarProdutoPorId(Guid id)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault();

            if (produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
            }
            bool podeExcluir = !_contextoDb.ItensVendas.Any(iv => iv.IdProduto == produtoVindoDoBanco.Id && iv.Ativo);
            Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoVindoDoBanco.Id)
                   ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id, estoque.QuantidadeEmEstoque);
            ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoVindoDoBanco.Id, produtoVindoDoBanco.CodigoDeBarra, produtoVindoDoBanco.DataCriacao,
                produtoVindoDoBanco.NomeProduto, produtoVindoDoBanco.Descricao, produtoVindoDoBanco.Preco,
                produtoVindoDoBanco.Ativo, podeExcluir,estoqueDto);
            return produtoFormatoDto;
        }

        public ProdutoDtoOutPut BuscarProdutoPorCodigoDeBarra(string codigoDeBarra)
        {

            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.CodigoDeBarra == codigoDeBarra && p.Ativo).FirstOrDefault();

            if (produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Produto não encontrado");
            }
            bool podeExcluir = !_contextoDb.ItensVendas.Any(iv => iv.IdProduto == produtoVindoDoBanco.Id && iv.Ativo);
            Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoVindoDoBanco.Id)
                  ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id, estoque.QuantidadeEmEstoque);
            ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoVindoDoBanco.Id, produtoVindoDoBanco.CodigoDeBarra, produtoVindoDoBanco.DataCriacao,
                produtoVindoDoBanco.NomeProduto, produtoVindoDoBanco.Descricao, produtoVindoDoBanco.Preco, 
                produtoVindoDoBanco.Ativo, podeExcluir, estoqueDto);
            return produtoFormatoDto;
        }

        public Produto CadastrarProduto(ProdutoInputDto dto)
        {
            //validar formato de codigo de barra? mais qual o formato vai utilizar?

            string codigoDeBarraSomenteNumerosELetras = Regex.Replace(dto.CodigoDeBarra, @"[^a-zA-Z0-9]", "");

            if (string.IsNullOrWhiteSpace(dto.CodigoDeBarra))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código de barra não pode ser null ou vazio");
            }
            Produto? produtoVindoDoBancoComMesmoCodigoDeBarra = _contextoDb
                .Produtos.Where(p => p.CodigoDeBarra == dto.CodigoDeBarra && p.Ativo).FirstOrDefault();

            if(produtoVindoDoBancoComMesmoCodigoDeBarra != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um produto com esse código de barra!");
            }
            Produto produtoAInserirNoBanco = new Produto(codigoDeBarraSomenteNumerosELetras,
                dto.NomeProduto, dto.Descricao, dto.PrecoUnitario);
            Estoque estoque = new(produtoAInserirNoBanco);
            estoque.Produto = produtoAInserirNoBanco;
            _contextoDb.Add(produtoAInserirNoBanco);
            _contextoDb.Add(estoque);
            _produtoLogService.CriarLogsDeCriacao(produtoAInserirNoBanco,_usuarioLogado);
            _estoqueLogService.CriarLogsDeCriacao(estoque,_usuarioLogado);
            _contextoDb.SaveChanges();
            return produtoAInserirNoBanco;

        }

        public Produto AtualizarProduto(Guid id, ProdutoInputDto dto)
        {
            Produto? produtoVindoDoBanco = 
                _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault() ?? throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");

            Produto? produtoVindoDoBancoComMesmoCodigoDeBarras = _contextoDb.Produtos.Where(p => p.CodigoDeBarra == dto.CodigoDeBarra && p.Id != id && p.Ativo)
                .FirstOrDefault();


            if (produtoVindoDoBancoComMesmoCodigoDeBarras != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um produto com esse código de barras");
            }

            Produto produtoSemAtualizar = produtoVindoDoBanco.Copia();

            produtoVindoDoBanco.CodigoDeBarra = dto.CodigoDeBarra;
            produtoVindoDoBanco.NomeProduto = dto.NomeProduto;
            produtoVindoDoBanco.Descricao = dto.Descricao;
            produtoVindoDoBanco.Preco = dto.PrecoUnitario;
            _contextoDb.Update(produtoVindoDoBanco);
            _produtoLogService.CriarLogsDeAtualizacao(produtoSemAtualizar,produtoVindoDoBanco,_usuarioLogado);
            _contextoDb.SaveChanges();
            return produtoVindoDoBanco;
        }

        public void DeletarProdutoPorId(Guid id)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault();

            if(produtoVindoDoBanco== null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
            }
            bool produtoEstaEmAlgumaVenda = _contextoDb.ItensVendas.Where(i => i.IdProduto == produtoVindoDoBanco.Id && i.Ativo).Any();
            if (produtoEstaEmAlgumaVenda)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Esse produto esta em uma venda, exclua a venda antes de exclui-lo");
            }
            Estoque estoque = _contextoDb.Estoques.Include(e=>e.Produto).FirstOrDefault(e => e.Produto.Id == produtoVindoDoBanco.Id)
                  ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            produtoVindoDoBanco.Ativo = false;
            estoque.Ativo=false;
            _contextoDb.Update(produtoVindoDoBanco);
            _produtoLogService.CriarLogsDeExclusao(produtoVindoDoBanco,_usuarioLogado);
            _estoqueLogService.CriarLogsDeExclusao(estoque, _usuarioLogado);
            _contextoDb.SaveChanges();
        }

        public List<ProdutoDtoOutPut> BuscarProdutosPorNome(string nome)
        {
            List<Produto> produtosVindoDoBanco = _contextoDb.Produtos.Where(p => p.NomeProduto.Contains(nome) && p.Ativo).Take(10).ToList();

            List<ProdutoDtoOutPut> produtosFomartoDto = new List<ProdutoDtoOutPut>();

            foreach (Produto produtoIterado in produtosVindoDoBanco)
            {
                bool podeExcluir = !_contextoDb.ItensVendas.Any(iv => iv.IdProduto == produtoIterado.Id && iv.Ativo);
                Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoIterado.Id)
                   ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
                EstoqueSimplificadoOutputDto estoqueDto = new(estoque.Id, estoque.QuantidadeEmEstoque);
                ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoIterado.Id, produtoIterado.CodigoDeBarra,
                    produtoIterado.DataCriacao, produtoIterado.NomeProduto, produtoIterado.Descricao, 
                    produtoIterado.Preco, produtoIterado.Ativo, podeExcluir, estoqueDto);
                produtosFomartoDto.Add(produtoFormatoDto);
            }
            return produtosFomartoDto;
        }

        public byte[] GerarRelatorioDeProdutosComMaiorQuantidadeVendidaPorPeriodo(DatasParaGeracaoDeRelatorioDto dto)
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

            var modeloDocumento = new RelatorioProdutosComMaiorFaturamentoPorPeriodo(produtosMaisVendidos, dataDeInicioDoPeriodoFormatoDateOnly,dataDeFimDoPeriodoDateOnly);

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

        public List<BaseDtoLog> BuscarLogsPorIdProduto(Guid idProdutoEnviado)
        {
            List<ProdutoLog> produtoLogs = _contextoDb.ProdutoLogs
                .Where(l => l.IdProduto == idProdutoEnviado).ToList();

            List<ProdutoLogDto> produtoLogsDto =
                produtoLogs.Select(l => new ProdutoLogDto
                (l.IdProduto,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            List<EstoqueLog> estoqueLogs = _contextoDb.EstoqueLogs
               .Where(l => l.IdProduto == idProdutoEnviado).ToList();

            List<EstoqueLogDto> estoqueLogsDto =
                estoqueLogs.Select(l => new EstoqueLogDto
                (l.IdEstoque,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            List<BaseDtoLog> dtoLogs = new List<BaseDtoLog>();

            dtoLogs.AddRange(produtoLogsDto);
            dtoLogs.AddRange(estoqueLogsDto);

            dtoLogs.OrderByDescending(l => l.DataCriacao).ToList();

            return dtoLogs;
        }

    }    
}
