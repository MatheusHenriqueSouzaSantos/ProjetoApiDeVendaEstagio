using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services.Interfaces;
using System.Text.RegularExpressions;

namespace ApiEstagioBicicletaria.Services
{
    public class ServicoService : IServicoService
    {
        private ContextoDb _contextoDb;

        public ServicoService(ContextoDb contextoDb)
        {
            this._contextoDb = contextoDb;
        }

        public List<Servico> BuscarServicos()
        {
            return _contextoDb.Servicos.Where(s => s.Ativo).ToList();
        }
        public Servico BuscarServicoPorId(Guid id)
        {
            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.Id == id && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            return servicoVindoDoBanco;
        }

        public Servico BuscarServicoPorCodigoServico(string codigoServico)
        {

            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.CodigoServico == codigoServico && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            return servicoVindoDoBanco;
        }

        public Servico CadastrarProduto(ServicoDto dto)
        {
            //validar formato de codigo de barra? mais qual o formato vai utilizar?

            string codigoDeServicoSomenteNumerosELetras = Regex.Replace(dto.CodigoServico, @"[^a-zA-Z0-9]", "");

            if (string.IsNullOrWhiteSpace(dto.CodigoServico))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código do Serviço não pode ser null ou vazio");
            }
            Servico? servicoVindoDoBancoComMesmoCodigoServico = _contextoDb
                .Servicos.Where(s => s.CodigoServico == dto.CodigoServico && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBancoComMesmoCodigoServico != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um Serviço com esse código de barra!");
            }
            Servico servicoAInserirNoBanco = new Servico(codigoDeServicoSomenteNumerosELetras,
                dto.NomeServico, dto.Descricao, dto.PrecoServico);
            _contextoDb.Add(servicoAInserirNoBanco);
            _contextoDb.SaveChanges();
            return servicoAInserirNoBanco;

        }

        public Produto AtualizarProduto(Guid id, ProdutoDto dto)
        {
            if (!(string.IsNullOrWhiteSpace(dto.CodigoDeBarra)))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código de barra deve vir vazio, não é possível atualizar um código de barra");
            }
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault();

            if (produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
            }
            produtoVindoDoBanco.NomeProduto = dto.NomeProduto;
            produtoVindoDoBanco.Descricao = dto.Descricao;
            produtoVindoDoBanco.QuantidadeEmEstoque = dto.QuantidadeEmEstoque;
            produtoVindoDoBanco.PrecoUnitario = dto.PrecoUnitario;
            _contextoDb.Update(produtoVindoDoBanco);
            _contextoDb.SaveChanges();
            return produtoVindoDoBanco;
        }

        public void DeletarProdutoPorId(Guid id)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault();

            if (produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
            }
            produtoVindoDoBanco.Ativo = false;
            _contextoDb.Update(produtoVindoDoBanco);
            _contextoDb.SaveChanges();
        }

        public void DefinirQuantidadeEmEstoqueDeProduto(Guid id, int quantidade)
        {
            Produto? produtoVindoDoBanco = _contextoDb.Produtos.Where(p => p.Id == id && p.Ativo).FirstOrDefault();

            if (produtoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Produto não encontrado");
            }
            if (quantidade < 0 || quantidade > 3000)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O valor de QuantidadeEmEstoque deve estar no intervalo de 0 até 3000");
            }
            produtoVindoDoBanco.QuantidadeEmEstoque = quantidade;
            _contextoDb.Update(produtoVindoDoBanco);
            _contextoDb.SaveChanges();

        }
    }
