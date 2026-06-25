using ApiEstagioBicicletaria.Entities.FornedorDomain;

namespace ApiEstagioBicicletaria.Entities.EntradaEstoque
{
    public class EntradaEstoque : EntidadeBase
    {
        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Fornecedor Fornecedor { get; set; }

        public Guid IdFornecedor { get; set; }

        [AtributoASerIgnoradoLogAtualizacao]
        public string CodigoEntrada { get; private set; }

        public StatusEntradaEstoque Status {get; set;}=StatusEntradaEstoque.Criada;

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public List<ItemEntradaEstoque> Itens { get; set;}

        protected EntradaEstoque()
        {

        }

        public EntradaEstoque(Fornecedor fornecedor, string codigoEntrada, StatusEntradaEstoque status)
        {
            Fornecedor = fornecedor;
            IdFornecedor = fornecedor.Id;
            CodigoEntrada = codigoEntrada;
            Status = status;
        }

        public EntradaEstoque Copia()
        {
            return new EntradaEstoque(Fornecedor, CodigoEntrada,Status);   
        }
    }
}