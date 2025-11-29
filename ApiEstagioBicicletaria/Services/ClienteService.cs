using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Validacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Services
{
    //Service não retorna respostas de requisição
    public class ClienteService : IClienteService
    {
        private ContextoDb _contextoDb;

        public ClienteService(ContextoDb contextoDb)
        {
            this._contextoDb = contextoDb;
        }

        public List<Cliente> BuscarClientes()
        {
            List<ClienteFisico> clientesFisicos = _contextoDb.Clientes
                .OfType<ClienteFisico>()
                .Include(c => c.Endereco)
                .Where(c => c.Ativo)
                .ToList();

            List<ClienteJuridico> clientesJuridicos = _contextoDb.Clientes
                .OfType<ClienteJuridico>()
                .Include(c => c.Endereco)
                .Where(c => c.Ativo)
                .ToList();

            List<Cliente> todosClientes = new List<Cliente>();
            todosClientes.AddRange(clientesFisicos);
            todosClientes.AddRange(clientesJuridicos);

            return todosClientes;

        }

        public Cliente BuscarClientePorId(Guid id)
        {
            Cliente? cliente = _contextoDb.Clientes.Include(c => c.Endereco).FirstOrDefault(c => c.Id == id && c.Ativo);

            if (cliente== null)
            {
                throw new ExcecaoDeRegraDeNegocio(404,"Cliente não encontrado");
            }
            return cliente;
        }

        public ClienteFisico CadastrarClienteFisico(ClienteFisicoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Cpf))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf não pode ser sem valor");
            }
            string cpfSemPontoETracos= ClienteValidacao.RemoverNaoNumericos(dto.Cpf);
            if (!ClienteValidacao.ValidarCpf(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            ClienteFisico? clienteExistenteRetornadoComEsseCpf= _contextoDb.ClientesFisicos.Where(c => c.Cpf == cpfSemPontoETracos && c.Ativo).FirstOrDefault();
            if (clienteExistenteRetornadoComEsseCpf != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um cliente cadastrado com esse cpf");
            }
            Cliente? clienteExistenteComEsseEmail = _contextoDb.Clientes.Where(c => c.Email == dto.Email && c.Ativo).FirstOrDefault();
            if (clienteExistenteComEsseEmail != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Já existe um cliente cadastrado com esse E-mail");
            }
            Endereco endereco = new Endereco(dto.Endereco.Logradouro, dto.Endereco.Numero, dto.Endereco.Cidade, dto.Endereco.SiglaUf);
            ClienteFisico clienteFisico = new ClienteFisico(endereco, dto.Telefone, dto.Email, dto.Nome, cpfSemPontoETracos);
            _contextoDb.Enderecos.Add(endereco);
            _contextoDb.ClientesFisicos.Add(clienteFisico);
            _contextoDb.SaveChanges();
            return clienteFisico;
        }

        public ClienteJuridico CadastrarClienteJuridico(ClienteJuridicoDto dto)
        {

            //if (!ClienteValidacao.validarInscricaoEstadual(dto.InscricaoEstadual))
            //{
            //    throw new ExcecaoDeRegraDeNegocio(400, "Inscrição estadual inválida");
            //}
            string cnpjSemPontoETracos = ClienteValidacao.RemoverNaoNumericos(dto.Cnpj);
            if (string.IsNullOrWhiteSpace(cnpjSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cnpj não pode estar sem valor");
            }
            if (!ClienteValidacao.ValidarCnpj(cnpjSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cnpj inválido");
            }
            ClienteJuridico? empresaExistenteRetornadoComEsseCnpj = _contextoDb.ClientesJuridicos.Where(c => c.Cnpj == cnpjSemPontoETracos && c.Ativo).FirstOrDefault();
            if (empresaExistenteRetornadoComEsseCnpj != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe uma empresa cadastrada com esse cnpj");
            }
            Cliente? clienteExistenteComEsseEmail = _contextoDb.Clientes.Where(c => c.Email == dto.Email && c.Ativo).FirstOrDefault();
            if (clienteExistenteComEsseEmail != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um cliente cadastrado com esse E-mail");
            }
            Endereco endereco = new Endereco(dto.Endereco.Logradouro, dto.Endereco.Numero, dto.Endereco.Cidade, dto.Endereco.SiglaUf);
            ClienteJuridico clienteJuridico = new ClienteJuridico(endereco, dto.Telefone, dto.Email, dto.RazaoSocial, 
                dto.NomeFantasia,dto.InscricaoEstadual,cnpjSemPontoETracos);
            _contextoDb.Enderecos.Add(endereco);
            _contextoDb.ClientesJuridicos.Add(clienteJuridico);
            _contextoDb.SaveChanges();
            return clienteJuridico;
        }

        public ClienteFisico AtualizarClienteFisico(Guid id, ClienteFisicoDto dto)
        {
            //revisar
            if (!(string.IsNullOrWhiteSpace(dto.Cpf)))
            {
                throw new ExcecaoDeRegraDeNegocio(400,"O Cpf deve vir vazio ou nulo, não é possivel atualizar um cpf");
            }
            Cliente? clienteExistenteComEsseEmail = _contextoDb.Clientes.FirstOrDefault(c=>c.Email==dto.Email && c.Ativo && c.Id!=id);
            if (clienteExistenteComEsseEmail != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um cliente cadastrado com esse E-mail");
            }
            ClienteFisico? clienteFisicoVindoDoBanco= _contextoDb.ClientesFisicos.Include(c => c.Endereco).Where(c=>c.Id==id && c.Ativo).FirstOrDefault();
            if(clienteFisicoVindoDoBanco== null)
            {
                throw new ExcecaoDeRegraDeNegocio(404,"Cliente não encontrado");
            }
            clienteFisicoVindoDoBanco.Endereco.Logradouro = dto.Endereco.Logradouro;
            clienteFisicoVindoDoBanco.Endereco.Numero = dto.Endereco.Numero;
            clienteFisicoVindoDoBanco.Endereco.Cidade=dto.Endereco.Cidade;
            clienteFisicoVindoDoBanco.Endereco.SiglaUf=dto.Endereco.SiglaUf;
            clienteFisicoVindoDoBanco.Telefone = dto.Telefone;
            clienteFisicoVindoDoBanco.Email = dto.Email;
            clienteFisicoVindoDoBanco.Nome= dto.Nome;

            _contextoDb.Update(clienteFisicoVindoDoBanco);
            _contextoDb.SaveChanges();
            return clienteFisicoVindoDoBanco;
            
        }

        public ClienteJuridico AtualizarClienteJuridico(Guid id, ClienteJuridicoDto dto)
        {

            //if (!ClienteValidacao.validarInscricaoEstadual(dto.InscricaoEstadual))
            //{
            //    throw new ExcecaoDeRegraDeNegocio(400, "Inscrição estadual inválida");
            //}

            if (!(string.IsNullOrWhiteSpace(dto.Cnpj)))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O Cnpj deve vir vazio ou nulo, não é possivel atualizar um cpf");
            }
            ClienteJuridico? clienteJuridicoVindoDoBanco = _contextoDb.ClientesJuridicos.Include(c=>c.Endereco).Where(c => c.Id == id && c.Ativo).FirstOrDefault();
            if (clienteJuridicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Empresa não encontrado");
            }
            Cliente? clienteExistenteComEsseEmail = _contextoDb.Clientes.FirstOrDefault(c => c.Email == dto.Email && c.Ativo && c.Id != id);
            if (clienteExistenteComEsseEmail != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um cliente cadastrado com esse E-mail");
            }
            clienteJuridicoVindoDoBanco.Endereco.Logradouro = dto.Endereco.Logradouro;
            clienteJuridicoVindoDoBanco.Endereco.Numero = dto.Endereco.Numero;
            clienteJuridicoVindoDoBanco.Endereco.Cidade = dto.Endereco.Cidade;
            clienteJuridicoVindoDoBanco.Endereco.SiglaUf = dto.Endereco.SiglaUf;
            clienteJuridicoVindoDoBanco.Telefone = dto.Telefone;
            clienteJuridicoVindoDoBanco.Email = dto.Email;
            clienteJuridicoVindoDoBanco.RazaoSocial=dto.RazaoSocial;
            clienteJuridicoVindoDoBanco.NomeFantasia = dto.NomeFantasia;
            clienteJuridicoVindoDoBanco.InscricaoEstadual = dto.InscricaoEstadual;
            
            _contextoDb.Update(clienteJuridicoVindoDoBanco);
            _contextoDb.SaveChanges();
            return clienteJuridicoVindoDoBanco;
        }

        public void DeletarCLientePorId(Guid id)
        {
            //is ativo
            Cliente? clienteExistente = _contextoDb.Clientes.Where(c => c.Id == id && c.Ativo).FirstOrDefault();

            if(clienteExistente == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Cliente não encontrado");
            }
            bool clienteEstaEmAlgumaVenda = _contextoDb.Vendas.Where(v => v.IdCliente == clienteExistente.Id && v.Ativo).Any();
            if (clienteEstaEmAlgumaVenda)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Esse cliente já realizou uma venda, exclua a venda antes de exclui-lo");
            }
            clienteExistente.Ativo = false;
            _contextoDb.Update(clienteExistente);
            _contextoDb.SaveChanges();
            
        }

        public List<Cliente> BuscarClientesPorNome(string nome)
        {
            List<ClienteFisico> clientesFisicos = _contextoDb.Clientes
                .OfType<ClienteFisico>()
                .Include(c => c.Endereco)
                .Where(c => c.Ativo && c.Nome.Contains(nome))
                .ToList();

            List<ClienteJuridico> clientesJuridicos = _contextoDb.Clientes
                .OfType<ClienteJuridico>()
                .Include(c => c.Endereco)
                .Where(c => c.Ativo && c.RazaoSocial.Contains(nome))
                .ToList();

            List<Cliente> todosClientes = new List<Cliente>();
            todosClientes.AddRange(clientesFisicos);
            todosClientes.AddRange(clientesJuridicos);

            return todosClientes;
        }
        public Cliente BuscarClientePorDocumentoIndentificador(DocumentoClienteInputDto dto)
        {
            switch (dto.TipoDocumento) 
            {
                case (EnumTipoDocumentoASerBuscado.Cpf):
                    {
                        return BuscarClientePorCpf(dto.NumeroDocumento);
                        break;
                    }
                case (EnumTipoDocumentoASerBuscado.Cnpj):
                    {
                        return BuscarClientePorCnpj(dto.NumeroDocumento);
                        break;
                    }
                default:
                    {
                        throw new ExcecaoDeRegraDeNegocio(400,"Tipo de cliente inválido enviado");
                        break;
                    }
            }
        }
        public ClienteFisico BuscarClientePorCpf(string cpfEnviado)
        {
            string cpfSomentNumeros=ClienteValidacao.RemoverNaoNumericos(cpfEnviado);
            if (!ClienteValidacao.ValidarCpf(cpfSomentNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Cpf Ínválido");
            }
            ClienteFisico? clienteVindoDoBanco = _contextoDb.ClientesFisicos.Include(c=>c.Endereco).FirstOrDefault(c => c.Cpf == cpfSomentNumeros && c.Ativo);
            if(clienteVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Cliente não encontrado!!");
            }
            return clienteVindoDoBanco;
        }

        public ClienteJuridico BuscarClientePorCnpj(string cnpjEnviado)
        {
            string cnpjSomenteNumeros = ClienteValidacao.RemoverNaoNumericos(cnpjEnviado);

            if (!ClienteValidacao.ValidarCnpj(cnpjSomenteNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cnpj Inválido");
            }
            ClienteJuridico? clienteVindoDoBanco = _contextoDb.ClientesJuridicos.Include(c=>c.Endereco).FirstOrDefault(c => c.Cnpj == cnpjSomenteNumeros && c.Ativo);
            if(clienteVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cliente não encontrado!!");
            }
            return clienteVindoDoBanco;
        }
    }
}
