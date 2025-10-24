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

        public Servico BuscarServicoPorCodigoDoServico(string codigoServico)
        {

            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.CodigoServico == codigoServico && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            return servicoVindoDoBanco;
        }

        public Servico CadastraServico(ServicoDto dto)
        {
            //validar formato de codigo de barra? mais qual o formato vai utilizar?

            string codigoServicoSomenteNumerosELetras = Regex.Replace(dto.CodigoServico, @"[^a-zA-Z0-9]", "");

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
            Servico servicoAInserirNoBanco = new Servico(codigoServicoSomenteNumerosELetras,
                dto.NomeServico, dto.Descricao, dto.PrecoServico);
            _contextoDb.Add(servicoAInserirNoBanco);
            _contextoDb.SaveChanges();
            return servicoAInserirNoBanco;

        }

        public Servico AtualizarServico(Guid id, ServicoDto dto)
        {
            if (!(string.IsNullOrWhiteSpace(dto.CodigoServico)))
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

        public void DeletarservicoPorId(Guid id)
        {
            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.Id == id && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            servicoVindoDoBanco.Ativo = false;
            _contextoDb.Update(servicoVindoDoBanco);
            _contextoDb.SaveChanges();
        }

    }
}
