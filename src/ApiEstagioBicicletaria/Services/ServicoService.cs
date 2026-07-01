using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.ServicoDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using System.Text.RegularExpressions;

namespace ApiEstagioBicicletaria.Services
{
    public class ServicoService : IServicoService
    {
        private ContextoDb _contextoDb;
        private Usuario _usuarioLogado;
        private ServicoLogService _logService;

        public ServicoService(ContextoDb contextoDb, IUsuarioLogadoService usuarioLogadoService, ServicoLogService logService)
        {
            _contextoDb = contextoDb;
            _usuarioLogado = usuarioLogadoService.ObterUsuario();
            _logService = logService;
        }

        public List<ServicoDtoOutPut> BuscarServicosAtivos()
        {
            List<Servico> servicosVindoDoBanco= _contextoDb.Servicos.Where(s => s.Ativo).ToList();
            List<ServicoDtoOutPut> servicosFormatoOutPut = new List<ServicoDtoOutPut>();

            foreach(Servico servicoIterado in servicosVindoDoBanco)
            {
                bool podeExcluir = !_contextoDb.ServicosVendas.Any(sv => sv.IdServico == servicoIterado.Id && sv.Ativo);
                ServicoDtoOutPut servicoFormatoDto = new ServicoDtoOutPut(servicoIterado.Id, servicoIterado.CodigoDoServico, servicoIterado.DataCriacao, servicoIterado.NomeServico,
                    servicoIterado.Descricao, servicoIterado.Preco, servicoIterado.Ativo, podeExcluir);
                servicosFormatoOutPut.Add(servicoFormatoDto);
            }
            return servicosFormatoOutPut;
        }

        public List<ServicoInativoOutputDto> BuscarServicosInativos()
        {
            List<Servico> servicosVindoDoBanco = _contextoDb.Servicos.Where(s => !s.Ativo).ToList();
            List<ServicoInativoOutputDto> servicosFormatoOutPut = new();

            foreach (Servico servicoIterado in servicosVindoDoBanco)
            {
                ServicoInativoOutputDto servicoFormatoDto = new(servicoIterado.Id, servicoIterado.CodigoDoServico, servicoIterado.DataCriacao, 
                    servicoIterado.NomeServico,servicoIterado.Descricao, servicoIterado.Preco, servicoIterado.Ativo);
                servicosFormatoOutPut.Add(servicoFormatoDto);
            }
            return servicosFormatoOutPut;
        }
        public ServicoDtoOutPut BuscarServicoAtivoPorId(Guid id)
        {
            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.Id == id && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            bool podeExcluir = !_contextoDb.ServicosVendas.Any(sv => sv.IdServico == servicoVindoDoBanco.Id && sv.Ativo);
            ServicoDtoOutPut servicoFormatoDto=new ServicoDtoOutPut(servicoVindoDoBanco.Id,servicoVindoDoBanco.CodigoDoServico,servicoVindoDoBanco.DataCriacao,servicoVindoDoBanco.NomeServico,
                servicoVindoDoBanco.Descricao,servicoVindoDoBanco.Preco,servicoVindoDoBanco.Ativo,podeExcluir);

            return servicoFormatoDto;
        }

        public ServicoDtoOutPut BuscarServicoAtivoPorCodigoDoServico(string codigoDoServico)
        {
            Servico? servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.CodigoDoServico == codigoDoServico && s.Ativo).FirstOrDefault();

            if (servicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");
            }
            bool podeExcluir = !_contextoDb.ServicosVendas.Any(sv => sv.IdServico == servicoVindoDoBanco.Id && sv.Ativo);
            ServicoDtoOutPut servicoFormatoDto = new ServicoDtoOutPut(servicoVindoDoBanco.Id, servicoVindoDoBanco.CodigoDoServico, servicoVindoDoBanco.DataCriacao, servicoVindoDoBanco.NomeServico,
                servicoVindoDoBanco.Descricao, servicoVindoDoBanco.Preco, servicoVindoDoBanco.Ativo, podeExcluir);

            return servicoFormatoDto;
        }

        public Servico CadastrarServico(ServicoInputDto dto)
        {
            //validar formato de codigo de barra? mais qual o formato vai utilizar?

            if (string.IsNullOrWhiteSpace(dto.CodigoDoServico))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O código do Serviço não pode ser vazio");
            }

            Servico? servicoVindoDoBancoComMesmoCodigoDoServico = _contextoDb
                .Servicos.Where(s => s.CodigoDoServico == dto.CodigoDoServico).FirstOrDefault();

            if (servicoVindoDoBancoComMesmoCodigoDoServico != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um Serviço com esse Código de Serviço!");
            }
            Servico servicoAInserirNoBanco = new Servico(dto.CodigoDoServico,
                dto.NomeServico, dto.Descricao, dto.PrecoServico);
            _contextoDb.Add(servicoAInserirNoBanco);
            _logService.CriarLogsDeCriacao(servicoAInserirNoBanco,_usuarioLogado);
            _contextoDb.SaveChanges();
            return servicoAInserirNoBanco;

        }

        public Servico AtualizarServico(Guid id, ServicoInputDto dto)
        {
            Servico servicoVindoDoBanco = _contextoDb.Servicos.Where(s => s.Id == id).FirstOrDefault()
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");

            if (servicoVindoDoBanco.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Serviço está inativo, reative-o para poder atualiza-lo");
            }

            Servico? servicoVindoDoBancoComMesmoCodigoDeBarras = _contextoDb.Servicos.Where(s => s.CodigoDoServico == dto.CodigoDoServico && s.Id != id)
                .FirstOrDefault();


            if (servicoVindoDoBancoComMesmoCodigoDeBarras != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um servico com esse código de serviço");
            }

            Servico servicoDesatualizado = servicoVindoDoBanco.Copia();
            servicoVindoDoBanco.CodigoDoServico = dto.CodigoDoServico;
            servicoVindoDoBanco.NomeServico = dto.NomeServico;
            servicoVindoDoBanco.Descricao = dto.Descricao;
            servicoVindoDoBanco.Preco = dto.PrecoServico;
            _contextoDb.Update(servicoVindoDoBanco);
            _logService.CriarLogsDeAtualizacao(servicoDesatualizado,servicoVindoDoBanco,_usuarioLogado);
            _contextoDb.SaveChanges();
            return servicoVindoDoBanco;
        }

        public void InativarServicoPorId(Guid id)
        {
            Servico servicoVindoDoBanco = _contextoDb.Servicos.FirstOrDefault(s => s.Id == id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");

            if (servicoVindoDoBanco.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Serviço já está inativo");
            }
            servicoVindoDoBanco.Ativo = false;
            _contextoDb.Servicos.Update(servicoVindoDoBanco);
            _logService.CriarLogsDeInativacao(servicoVindoDoBanco, _usuarioLogado);
            _contextoDb.SaveChanges();
        }

        public void ReativarServicoPorId(Guid id)
        {
            Servico servicoVindoDoBanco = _contextoDb.Servicos.FirstOrDefault(s => s.Id == id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");

            if (servicoVindoDoBanco.Ativo == true)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Serviço já está Ativo");
            }
            servicoVindoDoBanco.Ativo = true;
            _contextoDb.Servicos.Update(servicoVindoDoBanco);
            _logService.CriarLogsDeReativacao(servicoVindoDoBanco, _usuarioLogado);
            _contextoDb.SaveChanges();
        }

        public List<ServicoDtoOutPut> BuscarServicosPorNome(string nome)
        {
            List<Servico> servicosVindoDoBanco= _contextoDb.Servicos.Where(s => s.NomeServico.Contains(nome)).Take(10).ToList();
            List<ServicoDtoOutPut> servicosFormatoOutPut = new List<ServicoDtoOutPut>();

            foreach (Servico servicoIterado in servicosVindoDoBanco)
            {
                bool podeExcluir = !_contextoDb.ServicosVendas.Any(sv => sv.IdServico == servicoIterado.Id && sv.Ativo);
                ServicoDtoOutPut servicoFormatoDto = new ServicoDtoOutPut(servicoIterado.Id, servicoIterado.CodigoDoServico, servicoIterado.DataCriacao, servicoIterado.NomeServico,
                    servicoIterado.Descricao, servicoIterado.Preco, servicoIterado.Ativo, podeExcluir);
                servicosFormatoOutPut.Add(servicoFormatoDto);
            }
            return servicosFormatoOutPut;
        }

        public List<ServicoLogOutputDto> BuscarLogsPorIdServico(Guid id)
        {
            Servico servico = _contextoDb.Servicos.FirstOrDefault(s => s.Id == id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");

            List<ServicoLog> logs = _contextoDb.ServicoLogs
                .Where(l => l.IdServico == servico.Id).OrderByDescending(l => l.DataCriacao).ToList();

            List<ServicoLogOutputDto> logsDto =
                logs.Select(l => new ServicoLogOutputDto
                (l.IdServico,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();
            return logsDto;
        }

        public List<ServicoLogOutputDto> BuscarLogsPorCodigoDoServico(string codigoDoServico)
        {
            Servico servico = _contextoDb.Servicos.FirstOrDefault(s => s.CodigoDoServico == codigoDoServico)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Serviço não encontrado");

            List<ServicoLog> logs = _contextoDb.ServicoLogs
                .Where(l => l.IdServico == servico.Id).OrderByDescending(l => l.DataCriacao).ToList();

            List<ServicoLogOutputDto> logsDto =
                logs.Select(l => new ServicoLogOutputDto
                (l.IdServico,
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
