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
            if(_contexto.Fornecedores.Any(f=>f.Email == dto.Email && f.Ativo))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um fornecedor cadastrado com esse email");
            }
            Fornecedor fornecedor = new(dto.Telefone, dto.Email, dto.RazaoSocial, dto.NomeFantasia, dto.Cnpj, dto.InscricaoEstadual);
            _contexto.Add(fornecedor);
            _contexto.SaveChanges();
            return fornecedor;
        }

        public Fornecedor Atualizar(Guid id, FornecedorUpdateDto dto)
        {
            Fornecedor fornecedorVindoDoBanco = _contexto.Fornecedores.FirstOrDefault(f => f.Id == id && f.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Fornecedor não encontrado");
            if (_contexto.Fornecedores.Any(f => f.Email == dto.Email && f.Ativo && f.Id!=id))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um fornecedor cadastrado com esse email");
            }
            fornecedorVindoDoBanco.Telefone=dto.Telefone;
            fornecedorVindoDoBanco.Email=dto.Email;
            fornecedorVindoDoBanco.RazaoSocial=dto.RazaoSocial;
            fornecedorVindoDoBanco.NomeFantasia=dto.NomeFantasia;
            fornecedorVindoDoBanco.InscricaoEstadual=dto.InscricaoEstadual;
            _contexto.Fornecedores.Update(fornecedorVindoDoBanco);
            _contexto.SaveChanges();
            return fornecedorVindoDoBanco; 

        }

        public void Desativar(Guid id)
        {
            //não deixar ser desativado se fornecedor já tiver feito uma entrada de estoque
            Fornecedor fornecedorVindoDoBanco = _contexto.Fornecedores.FirstOrDefault(f => f.Id == id && f.Ativo)
               ?? throw new ExcecaoDeRegraDeNegocio(404, "Fornecedor não encontrado");
            fornecedorVindoDoBanco.Ativo = false;
            _contexto.Fornecedores.Update(fornecedorVindoDoBanco);
            _contexto.SaveChanges();

        }
    }
}
