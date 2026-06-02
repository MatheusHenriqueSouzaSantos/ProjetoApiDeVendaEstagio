using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class ClienteLogService
    {
        private readonly LogRepositorio<ClienteLog> _repositorio;

        public ClienteLogService(LogRepositorio<ClienteLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacaoClienteFisico(ClienteFisico cliente, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ClienteFisico).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(cliente);

                ClienteLog log = new(
                    cliente,
                    TipoCliente.PessoaFisica,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeCriacaoClienteJuridico(ClienteJuridico cliente, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ClienteJuridico).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(cliente);

                ClienteLog log = new(
                    cliente,
                    TipoCliente.PessoaJuridica,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            
            }
        }

        public void CriarLogsDeAtualizacaoClienteFisico(ClienteFisico clienteAntigo, ClienteFisico clienteAtualizado,
            Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ClienteFisico).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AnotacaoDeAtributoASerIgnoradoLog)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(clienteAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(clienteAtualizado);

                if (valorAntigoPropriedade != valorAtualizadoPropriedade)
                {
                    ClienteLog log = 
                    new(clienteAtualizado,
                    TipoCliente.PessoaFisica,
                    LogAcao.Atualizacao,
                    propriedade.Name,
                    valorAntigoPropriedade?.ToString(),
                    valorAtualizadoPropriedade?.ToString(),
                    usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeAtualizacaoClienteJuridico(ClienteJuridico clienteAntigo, ClienteJuridico clienteAtualizado,
            Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ClienteJuridico).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AnotacaoDeAtributoASerIgnoradoLog)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(clienteAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(clienteAtualizado);

                if (valorAntigoPropriedade != valorAtualizadoPropriedade)
                {
                    ClienteLog log =
                    new(clienteAtualizado,
                    TipoCliente.PessoaJuridica,
                    LogAcao.Atualizacao,
                    propriedade.Name,
                    valorAntigoPropriedade?.ToString(),
                    valorAtualizadoPropriedade?.ToString(),
                    usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(Cliente cliente, Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new ClienteLog(cliente,cliente.TipoCliente,
                LogAcao.Exclusao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel));
        }
    }
}
