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

        public void CriarLogsDeCriacao(Estoque estoque, Usuario usuarioResponsavel)
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
                    estoque.Produto,
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
            _repositorio.CriarLog(new EstoqueLog(acaoQueAlterouEstoque,estoqueModificado,produtoDoEstoque , Entities.LogAcao.Atualizacao, "QuantidadeEmEstoque",
                quantidadeAnterior.ToString(), quantidadeAtual.ToString(), usuarioResponsavel));
        }
        public void CriarLogsDeExclusao(Estoque estoque, Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new EstoqueLog(AcaoQueAlterouEstoque.ExclusaoEntradaEstoque,estoque,estoque.Produto, Entities.LogAcao.Exclusao, "Ativo", true.ToString(),
                false.ToString(), usuarioResponsavel));
        }
    }
}
