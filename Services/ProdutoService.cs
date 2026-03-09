using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ApiEstagioBicicletaria.Services
{
    public class ProdutoService : IProdutoService
    {
        //implementar mexer em estoque no lugar de estoque em produto
        //redis
        //private readonly int _numeroMaximoDePaginas = 5;
        //private readonly int _numeroDeLinhasPorPagina = 42;
        private ContextoDb _contextoDb;

        public ProdutoService(ContextoDb contextoDb)
        {
            this._contextoDb = contextoDb;
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
                ProdutoDtoOutPut produtoFormatoDto = new ProdutoDtoOutPut(produtoIterado.Id, produtoIterado.CodigoDeBarra,
                    produtoIterado.DataCriacao, produtoIterado.NomeProduto, produtoIterado.Descricao,estoque.QuantidadeEmEstoque, 
                    produtoIterado.Ativo, podeExcluir,estoqueDto);
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
                produtoVindoDoBanco.NomeProduto, produtoVindoDoBanco.Descricao, produtoVindoDoBanco.PrecoUnitario,
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
                produtoVindoDoBanco.NomeProduto, produtoVindoDoBanco.Descricao, produtoVindoDoBanco.PrecoUnitario, 
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
            _contextoDb.Add(produtoAInserirNoBanco);
            _contextoDb.Add(estoque);
            _contextoDb.SaveChanges();
            return produtoAInserirNoBanco;

        }

        public Produto AtualizarProduto(Guid id, ProdutoInputDto dto)
        {
            if (!(string.IsNullOrWhiteSpace(dto.CodigoDeBarra)))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código de barra deve vir vazio, não é possível atualizar um código de barra");
            }
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault();

            if(produtoVindoDoBanco== null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
            }
            produtoVindoDoBanco.NomeProduto = dto.NomeProduto;
            produtoVindoDoBanco.Descricao = dto.Descricao;
            produtoVindoDoBanco.PrecoUnitario = dto.PrecoUnitario;
            _contextoDb.Update(produtoVindoDoBanco);
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
            Estoque estoque = _contextoDb.Estoques.FirstOrDefault(e => e.Produto.Id == produtoVindoDoBanco.Id)
                  ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
            produtoVindoDoBanco.Ativo = false;
            estoque.Ativo=false;
            //rever essa regra, para garantir que possa inativar um produto que ainda tenha quantidade em estoque
            _contextoDb.Update(produtoVindoDoBanco);
            _contextoDb.SaveChanges();
        }

        public Produto AdicionarQuantidadeEmEstoqueDeProdutoPorId(Guid idProdutoEnviado, int quantidadeAAdicionarEmEstoque)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.FirstOrDefault(p=>p.Id==idProdutoEnviado && p.Ativo);
            if(produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Produto não encontrado");
            }
            if (quantidadeAAdicionarEmEstoque < 0)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Não é possível adicionar uma quantidade negativa");
            }
            produtoVindoDoBanco.QuantidadeEmEstoque = produtoVindoDoBanco.QuantidadeEmEstoque + quantidadeAAdicionarEmEstoque;
            _contextoDb.Produtos.Update(produtoVindoDoBanco);
            _contextoDb.SaveChanges();
            return produtoVindoDoBanco;
        }
        //revisar
        public Produto AbaterQuantidadeEmEstoqueDeProdutoPorId(Guid idProdutoEnviado, int quantidadeAAbaterEmEstoque)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.FirstOrDefault(p => p.Id == idProdutoEnviado && p.Ativo);
            if (produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Produto não encontrado");
            }
            if (quantidadeAAbaterEmEstoque < 0)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Não é possível abater uma quantidade negativa");
            }
            if(quantidadeAAbaterEmEstoque> produtoVindoDoBanco.QuantidadeEmEstoque)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Não existe quantidade de produto sufisciente para remover, pois o estoque não pode ser negativo!!");
            }
            produtoVindoDoBanco.QuantidadeEmEstoque = produtoVindoDoBanco.QuantidadeEmEstoque - quantidadeAAbaterEmEstoque;
            _contextoDb.Produtos.Update(produtoVindoDoBanco);
            _contextoDb.SaveChanges();
            return produtoVindoDoBanco;
        }

        //public void DefinirQuantidadeEmEstoqueDeProduto(Guid id, int quantidade)
        //{
        //    Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault();

        //    if (produtoVindoDoBanco == null)
        //    {
        //        throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
        //    }
        //    if (quantidade < 0 || quantidade > 3000)
        //    {
        //        throw new ExcecaoDeRegraDeNegocio(400, "O valor de QuantidadeEmEstoque deve estar no intervalo de 0 até 3000");
        //    }
        //    produtoVindoDoBanco.QuantidadeEmEstoque = quantidade;
        //    _contextoDb.Update(produtoVindoDoBanco);
        //    _contextoDb.SaveChanges();
        //} 
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
                    produtoIterado.PrecoUnitario, produtoIterado.Ativo, podeExcluir, estoqueDto);
                produtosFomartoDto.Add(produtoFormatoDto);
            }
            return produtosFomartoDto;
        }

        public byte[] GerarRelatorioDeProdutosComMaiorFaturamentoPorPeriodo(DatasParaGeracaoDeRelatorioDto dto)
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

            var documento = new RelatorioProdutosComMaiorFaturamentoPorPeriodo(produtosMaisVendidos, dataDeInicioDoPeriodoFormatoDateOnly,dataDeFimDoPeriodoDateOnly);

            byte[] pdf = documento.GeneratePdf();

            return pdf;
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
            List<Produto> produtosEmFalta=new List<Produto>();
            foreach (Produto produto in _contextoDb.Produtos.Where(p => p.Ativo))
            {
                Estoque estoque =_contextoDb.Estoques.FirstOrDefault(e=>e.Produto.Id == produto.Id)
                      ?? throw new ExcecaoDeRegraDeNegocio(500, "Erro Interno em Estoque");
                if(estoque.QuantidadeEmEstoque<= quantidadeParaBuscarDosProdutosEmFalta)
                {
                    produtosEmFalta.Add(produto);
                }
            }
     
             = .Where(p=>p.Ativo  && p.QuantidadeEmEstoque<= )
                .OrderBy(p=>p.QuantidadeEmEstoque)
                .ToList();
            //colocar quando a quantidade for menor que tal, fazer a busca??
            QuestPDF.Settings.License = LicenseType.Community;

            var documento = new RelatorioDeProdutosEmFalta(produtosEmFalta,quantidadeParaBuscarDosProdutosEmFalta);

            byte[] pdf = documento.GeneratePdf();

            return pdf;
        }
    }    
}
