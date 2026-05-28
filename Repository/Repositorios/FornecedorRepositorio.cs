using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Repositories;

namespace ApiEstagioBicicletaria.Repository.Repositorios;

public class FornecedorRepositorio
{
    private ContextoDb _contexto;

    public FornecedorRepositorio(ContextoDb contexto)
    {
        _contexto = contexto;
    }

    public Fornecedor? BuscarFornecedorPorId(Guid id)
    {
        return _contexto.Fornecedores.FirstOrDefault(v=>v.Id==id && v.Ativo);
    }

    public List<Fornecedor> BuscarFornecedoresPorNome(string nome)
    {
        return _contexto.Fornecedores.Where(f=>f.RazaoSocial.Contains(nome)).Take(10).ToList();
    }
}