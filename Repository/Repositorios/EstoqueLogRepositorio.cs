using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Repository.Repositorios
{
    public class EstoqueLogRepositorio
    {
        private readonly DbSet<EstoqueLog> _repositorio;
        public EstoqueLogRepositorio(ContextoDb contexto)
        {
            _repositorio = contexto.EstoqueLogs;
        }

        public void criarLog(Estoque estoque,LogAcao acao,string campoAlterado,string valorAntigo,string valorNovo,Usuario usuarioResponsavel)
        {
            EstoqueLog log = new(estoque,acao,campoAlterado,valorAntigo,valorNovo,usuarioResponsavel);
            _repositorio.Add(log);
        }
    }
}
