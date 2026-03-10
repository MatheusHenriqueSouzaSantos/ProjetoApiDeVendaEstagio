using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services.Interfaces;

namespace ApiEstagioBicicletaria.Services
{
    public class EstoqueService : IEstoqueService
    {
        private ContextoDb _contexto;

        public EstoqueService(ContextoDb contexto)
        {
            _contexto = contexto;
        }

        public EstoqueSimplificadoOutputDto BuscarPorId(Guid id)
        {
            Estoque estoque = _contexto.Estoques.FirstOrDefault(e=>e.Id==id && e.Ativo)
                    ?? throw new ExcecaoDeRegraDeNegocio(400,"Estoque não encontrado");
            return EntityToDto(estoque);
        }

        public EstoqueSimplificadoOutputDto AdicionarQuantidadeEmEstoque(Guid id, int quantidade)
        {
            Estoque? estoqueVindoDoBanco = _contexto.Estoques.FirstOrDefault(p => p.Id == id && p.Ativo);
            if (estoqueVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Estoque não encontrado");
            }
            if (quantidade < 0)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Não é possível adicionar uma quantidade negativa");
            }
            estoqueVindoDoBanco.QuantidadeEmEstoque = estoqueVindoDoBanco.QuantidadeEmEstoque + quantidade;
            _contexto.Estoques.Update(estoqueVindoDoBanco);
            _contexto.SaveChanges();
            return EntityToDto(estoqueVindoDoBanco);
        }
        public EstoqueSimplificadoOutputDto AbaterQuantidadeEmEstoque(Guid id, int quantidade)
        {
            Estoque? estoqueVindoDoBanco = _contexto.Estoques.FirstOrDefault(p => p.Id == id && p.Ativo);
            if (estoqueVindoDoBanco == null)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Estoque não encontrado");
            }
            if (quantidade < 0)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Não é possível abater uma quantidade negativa");
            }
            if (quantidade > estoqueVindoDoBanco.QuantidadeEmEstoque)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Não existe quantidade em estoque sufisciente para remover, pois o estoque não pode ser negativo!!");
            }
            estoqueVindoDoBanco.QuantidadeEmEstoque = estoqueVindoDoBanco.QuantidadeEmEstoque - quantidade;
            _contexto.Estoques.Update(estoqueVindoDoBanco);
            _contexto.SaveChanges();
            return EntityToDto(estoqueVindoDoBanco);
        }


        private EstoqueSimplificadoOutputDto EntityToDto(Estoque estoque)
        {
            return new EstoqueSimplificadoOutputDto(estoque.Id, estoque.QuantidadeEmEstoque);
        }

    }
}
