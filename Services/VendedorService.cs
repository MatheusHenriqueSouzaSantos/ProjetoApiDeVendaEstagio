using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Validacao;

namespace ApiEstagioBicicletaria.Services
{
    public class VendedorService : IVendedorService
    {

        private ContextoDb _contexto;

        public VendedorService(ContextoDb contexto)
        {
            _contexto = contexto;
        }

        public List<Vendedor> BuscarTodosOsVendedores()
        {
            return _contexto.Vendedores.Where(v=>v.Ativo).ToList();
        }

        public Vendedor BuscarVendedorPorId(Guid id)
        {
            return _contexto.Vendedores.FirstOrDefault(v=>v.Id == id && v.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(400,"Vendedor não encontrado");
        }

        public Vendedor BuscarVendedorPorCpf(string cpf)
        {
            string cpfSoNumeros=DocumentoUtil.RemoverNaoNumericos(cpf);
            bool cpfValido= DocumentoUtil.ValidarCpf(cpfSoNumeros);
            if (!cpfValido)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            return _contexto.Vendedores.FirstOrDefault(v => v.Cpf == cpfSoNumeros && v.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");
        }

        public List<Vendedor> BuscarVendedoresPorNome(string nome)
        {
            return _contexto.Vendedores.Where(v => v.NomeCompleto.Contains(nome)).Take(10).ToList();
        }


        public Vendedor CriarVendedor(VendedorCreateDto dto)
        {
            string cpfSoNumeros = DocumentoUtil.RemoverNaoNumericos(dto.Cpf);
            if (!DocumentoUtil.ValidarCpf(cpfSoNumeros))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Cpf inválido");
            }
            bool vendedorExistenteComEsseCpf = _contexto.Vendedores.Any(v => v.Cpf == cpfSoNumeros && v.Ativo);
            if (vendedorExistenteComEsseCpf)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um vendedor cadastrado com esse cpf");
            }
            bool vendedorCadastradoComEsseEmail = _contexto.Vendedores.Any(v => v.Email == dto.Email && v.Ativo);

            if (vendedorCadastradoComEsseEmail)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um vendedor cadastrado com esse email");
            }

            Vendedor vendedor = new Vendedor(dto.Telefone,dto.Email,dto.NomeCompleto,dto.Cpf);

            _contexto.Vendedores.Add(vendedor);

            return vendedor;
        }
        public Vendedor AtualizarVendedor(Guid id,VendedorUpdatedDto dto)
        {
            Vendedor vendedor = _contexto.Vendedores.FirstOrDefault(v => v.Id == id && v.Ativo)
                ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");
            
            bool vendedorCadastradoComEsseEmail = _contexto.Vendedores.Any(v => v.Email == dto.Email && v.Ativo && v.Id!=id);

            if (vendedorCadastradoComEsseEmail)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um vendedor cadastrado com esse email");
            }

            vendedor.Telefone = dto.Telefone;
            vendedor.Email = dto.Email;
            vendedor.NomeCompleto = dto.NomeCompleto;

            _contexto.Vendedores.Update(vendedor);

            return vendedor;
        }

        public void DesativarVendedor(Guid id)
        {
            Vendedor vendedor = _contexto.Vendedores.FirstOrDefault(v => v.Id == id && v.Ativo)
               ?? throw new ExcecaoDeRegraDeNegocio(400, "Vendedor não encontrado");

            bool vendedorPresenteEmAlgumaVenda = _contexto.Vendas.Any(v => v.Vendedor.Id == id && v.Ativo);

            if (vendedorPresenteEmAlgumaVenda)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "O Vendedor Está presente em uma venda!, A Venda deve ser excluída antes de poder excluír o cliente");
            }
            vendedor.Ativo = false;
            _contexto.Vendedores.Update(vendedor);
        }
    }
}
