using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class EstoqueLogService
    {
        private readonly LogRepositorio<EstoqueLog> _repositorio;

        public EstoqueLogService(LogRepositorio<EstoqueLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Estoque estoque,Produto produtoDoEstoque, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Estoque).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade = propriedade.GetValue(estoque);

                EstoqueLog log = new EstoqueLog(AcaoQueAlterouEstoque.CriacaoProduto,estoque,
                    produtoDoEstoque,
                    Entities.LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }
        public void CriarLogDeAtualizacaoQuantidadeEmEstoque(Estoque estoqueModificado,Produto produtoDoEstoque,int quantidadeAnterior,int quantidadeAtual,
            AcaoQueAlterouEstoque acaoQueAlterouEstoque,Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new EstoqueLog(acaoQueAlterouEstoque,estoqueModificado,produtoDoEstoque,Entities.LogAcao.Atualizacao, "QuantidadeEmEstoque",
                quantidadeAnterior.ToString(), quantidadeAtual.ToString(), usuarioResponsavel));
        }
        public void CriarLogsDeInativacao(Estoque estoque,Produto produtoDoEstoque, Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new EstoqueLog(AcaoQueAlterouEstoque.InativacaoProduto,estoque, produtoDoEstoque, Entities.LogAcao.Inativacao, "Ativo", true.ToString(),
                false.ToString(), usuarioResponsavel));
        }

        public void CriarLogsDeReativacao(Estoque estoque, Produto produtoDoEstoque, Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new EstoqueLog(AcaoQueAlterouEstoque.ReativacaoProduto, estoque, produtoDoEstoque, Entities.LogAcao.Reativacao, "Ativo", false.ToString(),
                true.ToString(), usuarioResponsavel));
        }
    }
}
