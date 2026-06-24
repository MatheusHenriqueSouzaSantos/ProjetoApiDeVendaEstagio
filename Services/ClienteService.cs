using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using ApiEstagioBicicletaria.Validacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiEstagioBicicletaria.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ContextoDb _contextoDb;

        private readonly ClienteLogService _logService;
        
        private readonly EnderecoLogService _enderecoLogService;

        private readonly Usuario _usuarioLogado;

        public ClienteService(ContextoDb contextoDb, ClienteLogService logService, EnderecoLogService enderecoLogService, UsuarioLogadoService usuarioLogadoService)
        {
            _contextoDb = contextoDb;
            _logService = logService;
            _enderecoLogService = enderecoLogService;
            _usuarioLogado = usuarioLogadoService.ObterUsuario();
        }

        public List<ClienteDtoOutPut> BuscarClientes()
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
            List<ClienteDtoOutPut> todosClientesFormatoDto = new List<ClienteDtoOutPut>();
            List<ClienteFisicoDtoOutPut> clientesFisicoFormatoDtoOutput = new List<ClienteFisicoDtoOutPut>();

            foreach(ClienteFisico clienteFisicoIterado in clientesFisicos)
            {
                bool podeExcluirEsseCliente = !_contextoDb.Vendas.Any(v => v.IdCliente == clienteFisicoIterado.Id && v.Ativo);
                ClienteFisicoDtoOutPut clienteFormatoDtoOutput = new ClienteFisicoDtoOutPut(clienteFisicoIterado.Id, clienteFisicoIterado.Endereco, clienteFisicoIterado.DataCriacao,
                    clienteFisicoIterado.Telefone, clienteFisicoIterado.Email, clienteFisicoIterado.TipoCliente, podeExcluirEsseCliente, clienteFisicoIterado.Ativo,clienteFisicoIterado.Nome,clienteFisicoIterado.Cpf);
                clientesFisicoFormatoDtoOutput.Add(clienteFormatoDtoOutput);
            }
            todosClientesFormatoDto.AddRange(clientesFisicoFormatoDtoOutput);
            List<ClienteJuridicoDtoOutPut> clientesJuridicosFormatoDtoOutput = new List<ClienteJuridicoDtoOutPut>();
            foreach (ClienteJuridico clienteJuridicoIterado in clientesJuridicos)
            {
                bool podeExcluirEsseCliente = !_contextoDb.Vendas.Any(v => v.IdCliente == clienteJuridicoIterado.Id && v.Ativo);
                ClienteJuridicoDtoOutPut clienteFormatoDtoOutput = new ClienteJuridicoDtoOutPut(clienteJuridicoIterado.Id, clienteJuridicoIterado.Endereco, clienteJuridicoIterado.DataCriacao,
                    clienteJuridicoIterado.Telefone, clienteJuridicoIterado.Email, clienteJuridicoIterado.TipoCliente, podeExcluirEsseCliente, clienteJuridicoIterado.Ativo,clienteJuridicoIterado.RazaoSocial,
                    clienteJuridicoIterado.NomeFantasia,clienteJuridicoIterado.InscricaoEstadual,clienteJuridicoIterado.Cnpj);
                clientesJuridicosFormatoDtoOutput.Add(clienteFormatoDtoOutput);
            }
            todosClientesFormatoDto.AddRange(clientesJuridicosFormatoDtoOutput);
            return todosClientesFormatoDto;

        }

        public ClienteDtoOutPut BuscarClientePorId(Guid id)
        {
            Cliente? cliente = _contextoDb.Clientes.Include(c => c.Endereco).FirstOrDefault(c => c.Id == id && c.Ativo);
            ClienteDtoOutPut clienteFormatoDto=null;

            if (cliente== null)
            {
                throw new ExcecaoDeRegraDeNegocio(404,"Cliente não encontrado");
            }
            if (cliente.TipoCliente == TipoCliente.PessoaFisica)
            {
                ClienteFisico clienteFormatoFisico= (ClienteFisico)cliente;
                bool podeExcluirEsseCliente = !_contextoDb.Vendas.Any(v => v.IdCliente == clienteFormatoFisico.Id && v.Ativo);
                clienteFormatoDto = new ClienteFisicoDtoOutPut(clienteFormatoFisico.Id, clienteFormatoFisico.Endereco, clienteFormatoFisico.DataCriacao, clienteFormatoFisico.Telefone, clienteFormatoFisico.Email,
                    clienteFormatoFisico.TipoCliente, podeExcluirEsseCliente,clienteFormatoFisico.Ativo, clienteFormatoFisico.Nome, clienteFormatoFisico.Cpf);
            }
            if (cliente.TipoCliente == TipoCliente.PessoaJuridica)
            {
                ClienteJuridico clienteFormatoJuridico = (ClienteJuridico)cliente;
                bool podeExcluirEsseCliente = !_contextoDb.Vendas.Any(v => v.IdCliente == clienteFormatoJuridico.Id && v.Ativo);
                clienteFormatoDto = new ClienteJuridicoDtoOutPut(clienteFormatoJuridico.Id, clienteFormatoJuridico.Endereco, clienteFormatoJuridico.DataCriacao, clienteFormatoJuridico.Telefone, clienteFormatoJuridico.Email,
                    clienteFormatoJuridico.TipoCliente, podeExcluirEsseCliente, clienteFormatoJuridico.Ativo, clienteFormatoJuridico.RazaoSocial,clienteFormatoJuridico.NomeFantasia,clienteFormatoJuridico.InscricaoEstadual,clienteFormatoJuridico.Cnpj);
            }
            return clienteFormatoDto;
        }

        public ClienteFisico CadastrarClienteFisico(ClienteFisicoCreateDto dto)
        {
            string cpfSemPontoETracos= DocumentoUtil.RemoverPontosTracosEBarras(dto.Cpf);
            if (!DocumentoUtil.VerificarSeAStringContemSomenteNumeros(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O cpf deve conter apenas números");
            }
            if (!DocumentoUtil.ValidarCpf(cpfSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            if (dto.Endereco.SiglaUf.Any(char.IsDigit))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A sigla UF não deve conter números");
            }
            if (dto.Endereco.Cidade.Any(char.IsDigit))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O nome da cidade não deve conter numeros");
            }
            //como o sistema lida para mostrar clientes não ativos em vendas, devo deixar adicionar com o mesmo cpf acho que não??
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
            _enderecoLogService.CriarLogsDeCriacao(endereco,clienteFisico, _usuarioLogado);
            _logService.CriarLogsDeCriacaoClienteFisico(clienteFisico, _usuarioLogado);
            _contextoDb.SaveChanges();
            return clienteFisico;
        }

        public ClienteJuridico CadastrarClienteJuridico(ClienteJuridicoCreateDto dto)
        {
            //sem validação pois pode ser que seja cadastrado uma inscrição estadual de outro estado
            //if (!string.IsNullOrEmpty(inscricaoEstadualSemPontosTracosEBarras) || !ClienteUtil.VerificarSeAStringContemSomenteNumeros(inscricaoEstadualSemPontosTracosEBarras))
            //{
            //    throw new ExcecaoDeRegraDeNegocio(400, "A Incrição Estadual deve Conter apenas números");
            //}
            string inscricaoEstadual=dto.InscricaoEstadual;
            if (dto.InscricaoEstadual != null)
            {
                inscricaoEstadual = DocumentoUtil.RemoverPontosTracosEBarras(dto.InscricaoEstadual);
            }
            
            string cnpjSemPontoETracos = DocumentoUtil.RemoverPontosTracosEBarras(dto.Cnpj);
            if (!DocumentoUtil.VerificarSeAStringContemSomenteNumeros(cnpjSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O Cnpj deve conter apenas números");
            }
            if (!DocumentoUtil.ValidarCnpj(cnpjSemPontoETracos))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cnpj inválido");
            }
            if (dto.Endereco.SiglaUf.Any(char.IsDigit))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A sigla UF não deve conter números");
            }
            if (dto.Endereco.Cidade.Any(char.IsDigit))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O nome da cidade não deve conter numeros");
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
                dto.NomeFantasia, inscricaoEstadual, cnpjSemPontoETracos);
            _contextoDb.Enderecos.Add(endereco);
            _contextoDb.ClientesJuridicos.Add(clienteJuridico);
            _enderecoLogService.CriarLogsDeCriacao(endereco, clienteJuridico, _usuarioLogado);
            _logService.CriarLogsDeCriacaoClienteJuridico(clienteJuridico, _usuarioLogado);
            _contextoDb.SaveChanges();
            return clienteJuridico;
        }

        public ClienteFisico AtualizarClienteFisico(Guid id, ClienteFisicoUpdateDto dto)
        {
            //revisar
            if (dto.Endereco.SiglaUf.Any(char.IsDigit))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A sigla UF não deve conter números");
            }
            if (dto.Endereco.Cidade.Any(char.IsDigit))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O nome da cidade não deve conter numeros");
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
            Endereco enderecoAntigo = clienteFisicoVindoDoBanco.Endereco.Copia();

            ClienteFisico clienteAntigo = clienteFisicoVindoDoBanco.Copia();

            clienteFisicoVindoDoBanco.Endereco.Logradouro = dto.Endereco.Logradouro;
            clienteFisicoVindoDoBanco.Endereco.Numero = dto.Endereco.Numero;
            clienteFisicoVindoDoBanco.Endereco.Cidade=dto.Endereco.Cidade;
            clienteFisicoVindoDoBanco.Endereco.SiglaUf=dto.Endereco.SiglaUf;
            clienteFisicoVindoDoBanco.Telefone = dto.Telefone;
            clienteFisicoVindoDoBanco.Email = dto.Email;
            clienteFisicoVindoDoBanco.Nome= dto.Nome;

            _contextoDb.Update(clienteFisicoVindoDoBanco);
            _enderecoLogService.CriarLogsDeAtualizacao(enderecoAntigo,clienteFisicoVindoDoBanco.Endereco, clienteFisicoVindoDoBanco, _usuarioLogado);
            _logService.CriarLogsDeAtualizacaoClienteFisico(clienteAntigo,clienteFisicoVindoDoBanco, _usuarioLogado);
            _contextoDb.SaveChanges();
            return clienteFisicoVindoDoBanco;
            
        }

        public ClienteJuridico AtualizarClienteJuridico(Guid id, ClienteJuridicoUpdateDto dto)
        {

            //if (!ClienteValidacao.validarInscricaoEstadual(dto.InscricaoEstadual))
            //{
            //    throw new ExcecaoDeRegraDeNegocio(400, "Inscrição estadual inválida");
            //}
            //if (!string.IsNullOrEmpty(inscricaoEstadualSemPontosTracosEBarras) || !ClienteUtil.VerificarSeAStringContemSomenteNumeros(inscricaoEstadualSemPontosTracosEBarras))
            //{
            //    throw new ExcecaoDeRegraDeNegocio(400, "A Incrição Estadual deve Conter apenas números");
            //}
            string inscricaoEstadual = dto.InscricaoEstadual;
            if (dto.InscricaoEstadual != null)
            {
                inscricaoEstadual = DocumentoUtil.RemoverPontosTracosEBarras(dto.InscricaoEstadual);
            }
   
            if (dto.Endereco.SiglaUf.Any(char.IsDigit))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "A sigla UF não deve conter números");
            }
            if (dto.Endereco.Cidade.Any(char.IsDigit))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O nome da cidade não deve conter numeros");
            }
            Cliente? clienteExistenteComEsseEmail = _contextoDb.Clientes.FirstOrDefault(c => c.Email == dto.Email && c.Ativo && c.Id != id);
            if (clienteExistenteComEsseEmail != null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um cliente cadastrado com esse E-mail");
            }
            ClienteJuridico? clienteJuridicoVindoDoBanco = _contextoDb.ClientesJuridicos.Include(c=>c.Endereco).Where(c => c.Id == id && c.Ativo).FirstOrDefault();
            if (clienteJuridicoVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(404, "Empresa não encontrado");
            }

            Endereco enderecoAntigo = clienteJuridicoVindoDoBanco.Endereco.Copia();

            ClienteJuridico clienteAntigo = clienteJuridicoVindoDoBanco.Copia();

            clienteJuridicoVindoDoBanco.Endereco.Logradouro = dto.Endereco.Logradouro;
            clienteJuridicoVindoDoBanco.Endereco.Numero = dto.Endereco.Numero;
            clienteJuridicoVindoDoBanco.Endereco.Cidade = dto.Endereco.Cidade;
            clienteJuridicoVindoDoBanco.Endereco.SiglaUf = dto.Endereco.SiglaUf;
            clienteJuridicoVindoDoBanco.Telefone = dto.Telefone;
            clienteJuridicoVindoDoBanco.Email = dto.Email;
            clienteJuridicoVindoDoBanco.RazaoSocial=dto.RazaoSocial;
            clienteJuridicoVindoDoBanco.NomeFantasia = dto.NomeFantasia;
            clienteJuridicoVindoDoBanco.InscricaoEstadual = inscricaoEstadual;
            
            _contextoDb.Update(clienteJuridicoVindoDoBanco);
            _enderecoLogService.CriarLogsDeAtualizacao(enderecoAntigo, clienteJuridicoVindoDoBanco.Endereco, clienteJuridicoVindoDoBanco, _usuarioLogado);
            _logService.CriarLogsDeAtualizacaoClienteJuridico(clienteAntigo, clienteJuridicoVindoDoBanco, _usuarioLogado);
            _contextoDb.SaveChanges();
            return clienteJuridicoVindoDoBanco;
        }

        public void DeletarCLientePorId(Guid id)
        {
            Cliente? clienteExistente = _contextoDb.Clientes.Where(c => c.Id == id && c.Ativo).Include(c=>c.Endereco).FirstOrDefault();

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
            _enderecoLogService.CriarLogsDeExclusao(clienteExistente.Endereco,clienteExistente, _usuarioLogado);
            _logService.CriarLogsDeExclusao(clienteExistente, _usuarioLogado); 
            _contextoDb.SaveChanges();
            
        }

        public List<ClienteDtoOutPut> BuscarClientesPorNome(string nome)
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

            List<ClienteDtoOutPut> todosClientesFormatoDto = new List<ClienteDtoOutPut>();
            List<ClienteFisicoDtoOutPut> clientesFisicoFormatoDtoOutput = new List<ClienteFisicoDtoOutPut>();

            foreach (ClienteFisico clienteFisicoIterado in clientesFisicos)
            {
                bool podeExcluirEsseCliente = !_contextoDb.Vendas.Any(v => v.IdCliente == clienteFisicoIterado.Id && v.Ativo);
                ClienteFisicoDtoOutPut clienteFormatoDtoOutput = new ClienteFisicoDtoOutPut(clienteFisicoIterado.Id, clienteFisicoIterado.Endereco, clienteFisicoIterado.DataCriacao,
                    clienteFisicoIterado.Telefone, clienteFisicoIterado.Email, clienteFisicoIterado.TipoCliente, podeExcluirEsseCliente, clienteFisicoIterado.Ativo, clienteFisicoIterado.Nome, clienteFisicoIterado.Cpf);
                clientesFisicoFormatoDtoOutput.Add(clienteFormatoDtoOutput);
            }
            todosClientesFormatoDto.AddRange(clientesFisicoFormatoDtoOutput);
            List<ClienteJuridicoDtoOutPut> clientesJuridicosFormatoDtoOutput = new List<ClienteJuridicoDtoOutPut>();
            foreach (ClienteJuridico clienteJuridicoIterado in clientesJuridicos)
            {
                bool podeExcluirEsseCliente = !_contextoDb.Vendas.Any(v => v.IdCliente == clienteJuridicoIterado.Id && v.Ativo);
                ClienteJuridicoDtoOutPut clienteFormatoDtoOutput = new ClienteJuridicoDtoOutPut(clienteJuridicoIterado.Id, clienteJuridicoIterado.Endereco, clienteJuridicoIterado.DataCriacao,
                    clienteJuridicoIterado.Telefone, clienteJuridicoIterado.Email, clienteJuridicoIterado.TipoCliente, podeExcluirEsseCliente, clienteJuridicoIterado.Ativo, clienteJuridicoIterado.RazaoSocial,
                    clienteJuridicoIterado.NomeFantasia, clienteJuridicoIterado.InscricaoEstadual, clienteJuridicoIterado.Cnpj);
                clientesJuridicosFormatoDtoOutput.Add(clienteFormatoDtoOutput);
            }
            todosClientesFormatoDto.AddRange(clientesJuridicosFormatoDtoOutput);

            return todosClientesFormatoDto;
        }
        public ClienteDtoOutPut BuscarClientePorDocumentoIndentificador(DocumentoClienteInputDto dto)
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
        public ClienteFisicoDtoOutPut BuscarClientePorCpf(string cpfEnviado)
        {
            string cpfSomentNumeros=DocumentoUtil.RemoverNaoNumericos(cpfEnviado);
            if (!DocumentoUtil.ValidarCpf(cpfSomentNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Cpf Ínválido");
            }
            ClienteFisico? clienteVindoDoBanco = _contextoDb.ClientesFisicos.Include(c=>c.Endereco).FirstOrDefault(c => c.Cpf == cpfSomentNumeros && c.Ativo);
            if(clienteVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Cliente não encontrado!!");
            }
            bool podeExcluirEsseCliente = !_contextoDb.Vendas.Any(v => v.IdCliente == clienteVindoDoBanco.Id && v.Ativo);
            ClienteFisicoDtoOutPut clienteFormatoDto = new ClienteFisicoDtoOutPut(clienteVindoDoBanco.Id, clienteVindoDoBanco.Endereco, clienteVindoDoBanco.DataCriacao, clienteVindoDoBanco.Telefone, clienteVindoDoBanco.Email,
                clienteVindoDoBanco.TipoCliente, podeExcluirEsseCliente, clienteVindoDoBanco.Ativo, clienteVindoDoBanco.Nome, clienteVindoDoBanco.Cpf);

            return clienteFormatoDto;

        }

        public ClienteJuridicoDtoOutPut BuscarClientePorCnpj(string cnpjEnviado)
        {
            string cnpjSomenteNumeros = DocumentoUtil.RemoverNaoNumericos(cnpjEnviado);

            if (!DocumentoUtil.ValidarCnpj(cnpjSomenteNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cnpj Inválido");
            }
            ClienteJuridico? clienteVindoDoBanco = _contextoDb.ClientesJuridicos.Include(c=>c.Endereco).FirstOrDefault(c => c.Cnpj == cnpjSomenteNumeros && c.Ativo);
            if(clienteVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cliente não encontrado!!");
            }
            bool podeExcluirEsseCliente = !_contextoDb.Vendas.Any(v => v.IdCliente == clienteVindoDoBanco.Id && v.Ativo);
            ClienteJuridicoDtoOutPut clienteFormatoDto = new ClienteJuridicoDtoOutPut(clienteVindoDoBanco.Id, clienteVindoDoBanco.Endereco, clienteVindoDoBanco.DataCriacao, clienteVindoDoBanco.Telefone, clienteVindoDoBanco.Email,
                clienteVindoDoBanco.TipoCliente, podeExcluirEsseCliente, clienteVindoDoBanco.Ativo, clienteVindoDoBanco.RazaoSocial, clienteVindoDoBanco.NomeFantasia, clienteVindoDoBanco.InscricaoEstadual, clienteVindoDoBanco.Cnpj);

            return clienteFormatoDto;
        }

        public List<BaseLogOutputDto> BuscarLogsClientePorIdCliente(Guid idCliente)
        {
            List<ClienteLog> clienteLogs=_contextoDb.ClienteLogs.Where(l=>l.IdCliente==idCliente).ToList();

            List<ClienteLogOutputDto> clienteDtoLogs = clienteLogs.Select(l =>
                new ClienteLogOutputDto(
                        l.IdCliente,
                        l.Acao,
                        l.CampoAlterado,
                        l.ValorAntigo,
                        l.ValorNovo,
                        l.IdUsuarioResponsavel,
                        l.DataCriacao
                    )
            ).ToList();

            List<EnderecoLog> enderecoLogs = _contextoDb.EnderecoLogs.Where(l => l.IdCliente == idCliente).ToList();

            List<EnderecoLogOutputDto> enderecoDtoLogs = enderecoLogs.Select(l =>
                new EnderecoLogOutputDto(
                        l.IdEndereco,
                        l.Acao,
                        l.CampoAlterado,
                        l.ValorAntigo,
                        l.ValorNovo,
                        l.IdUsuarioResponsavel,
                        l.DataCriacao
                    )
            ).ToList();

            List<BaseLogOutputDto> logDtos= new List<BaseLogOutputDto>();

            logDtos.AddRange(clienteDtoLogs);
            
            logDtos.AddRange(enderecoDtoLogs);

            return logDtos.OrderByDescending(l => l.DataCriacao).ToList();

            
        }

    }
}
