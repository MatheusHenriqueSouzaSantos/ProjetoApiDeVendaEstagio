using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
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

        public Servico BuscarServicoPorCodigoDoServico(string codigoDoServico)
        {

            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.CodigoDoServico == codigoDoServico && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            return servicoVindoDoBanco;
        }

        public Servico CadastrarServico(ServicoDto dto)
        {
            //validar formato de codigo de barra? mais qual o formato vai utilizar?

            if (string.IsNullOrWhiteSpace(dto.CodigoDoServico))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código do Serviço não pode ser null ou vazio");
            }
            string codigoDoServicoSomenteNumerosELetras = Regex.Replace(dto.CodigoDoServico, @"[^a-zA-Z0-9]", "");

            Servico? servicoVindoDoBancoComMesmoCodigoDoServico = _contextoDb
                .Servicos.Where(s => s.CodigoDoServico == dto.CodigoDoServico && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBancoComMesmoCodigoDoServico != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um Serviço com esse código de barra!");
            }
            Servico servicoAInserirNoBanco = new Servico(codigoDoServicoSomenteNumerosELetras,
                dto.NomeServico, dto.Descricao, dto.PrecoServico);
            _contextoDb.Add(servicoAInserirNoBanco);
            _contextoDb.SaveChanges();
            return servicoAInserirNoBanco;

        }

        public Servico AtualizarServico(Guid id, ServicoDto dto)
        {
            if (!(string.IsNullOrWhiteSpace(dto.CodigoDoServico)))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código do Serviço deve vir vazio, não é possível atualizar um código do Serviço");
            }
            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.Id == id && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            servicoVindoDoBanco.NomeServico = dto.NomeServico;
            servicoVindoDoBanco.Descricao = dto.Descricao;
            servicoVindoDoBanco.PrecoServico = dto.PrecoServico;
            _contextoDb.Update(servicoVindoDoBanco);
            _contextoDb.SaveChanges();
            return servicoVindoDoBanco;
        }

        public void DeletarServicoPorId(Guid id)
        {
            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.Id == id && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            bool servicoEstaEmAlgumaVenda = _contextoDb.ServicosVendas.Where(sv => sv.IdServico == servicoVindoDoBanco.Id && sv.Ativo).Any();
            if (servicoEstaEmAlgumaVenda)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Esse serviço esta em uma venda, exclua a venda antes de exclui-lo");
            }
            servicoVindoDoBanco.Ativo = false;
            _contextoDb.Update(servicoVindoDoBanco);
            _contextoDb.SaveChanges();
        }

        public List<Servico> BuscarServicosPorNome(string nome)
        {
            return _contextoDb.Servicos.Where(s => s.NomeServico.Contains(nome)).Take(10).ToList();
        }

    }
}
