namespace ApiEstagioBicicletaria.Entities.EntradaEstoque
{
    public class EntradaEstoque : EntityBase
    {
        public Fornecedor Fornecedor { get; private set; }

        public Guid FornecedorId { get; private set; }

        public string CodigoEntrada { get; private set; }

        public EntradaEstoque(Fornecedor fornecedor, string codigoEntrada)
        {
            Fornecedor = fornecedor;
            FornecedorId = fornecedor.Id;
            CodigoEntrada = codigoEntrada;
        }
    }
}
