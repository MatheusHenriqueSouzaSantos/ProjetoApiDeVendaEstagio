using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Repository.Repositorios
{
    public class ProdutoLogRepositorio
    {
        private readonly DbSet<ProdutoLog> _repositorio;
        public ProdutoLogRepositorio(ContextoDb contexto)
        {
            _repositorio = contexto.ProdutoLogs;
        }

        public void criarLog(Produto produto,LogAcao acao,string campoAlterado,string valorAntigo,string valorNovo,Usuario usuarioReponsavel)
        {
            ProdutoLog log = new(produto, acao, campoAlterado, valorAntigo, valorNovo, usuarioReponsavel);
            _repositorio.Add(log);
        }
    }
}
