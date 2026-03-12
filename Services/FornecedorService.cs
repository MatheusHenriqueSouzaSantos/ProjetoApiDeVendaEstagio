using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Validacao;

namespace ApiEstagioBicicletaria.Services
{
    public class FornecedorService : IFornecedorService
    {

        private ContextoDb _contexto;

        public FornecedorService(ContextoDb contexto)
        {
            _contexto = contexto;
        }

        public List<Fornecedor> BuscarTodos()
        {
            return _contexto.Fornecedores.Where(f => f.Ativo).ToList();
        }

        public Fornecedor BuscarPorId(Guid id)
        {
            return _contexto.Fornecedores.FirstOrDefault(e=>e.Id==id && e.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404,"Fornecedor nao encontrado");
        }
       

        public Fornecedor BuscarPorCnpj(string cnpj)
        {
            string cnpjSomenteNumeros=DocumentoUtil.RemoverPontosTracosEBarras(cnpj);

            if (!DocumentoUtil.ValidarCnpj(cnpjSomenteNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O CNPJ deve estar em um formato valido");
            }

            return _contexto.Fornecedores.FirstOrDefault(e => e.Cnpj == cnpjSomenteNumeros && e.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Fornecedor nao encontrado");
        }


        public Fornecedor Cadastrar(FornecedorCreateDto dto)
        {
            string cnpjSomenteNumeros = DocumentoUtil.RemoverPontosTracosEBarras(dto.Cnpj);

            if (!DocumentoUtil.ValidarCnpj(cnpjSomenteNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O CNPJ deve estar em um formato valido");
            }
            if(_contexto.Fornecedores.Any(f=>f.Cnpj==cnpjSomenteNumeros && f.Ativo))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um fornecedor cadastrado com esse cnpj");
            }
        }

        public Fornecedor Atualizar(Guid id, FornecedorUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public void Desativar(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
