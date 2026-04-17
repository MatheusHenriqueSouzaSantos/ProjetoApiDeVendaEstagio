﻿namespace ApiEstagioBicicletaria.Entities.EntradaEstoque
{
    public class EntradaEstoque : EntityBase
    {
        public Fornecedor Fornecedor { get; set; }

        public Guid IdFornecedor { get; set; }

        public string CodigoEntrada { get; private set; }

        protected EntradaEstoque()
        {

        }

        public EntradaEstoque(Fornecedor fornecedor, string codigoEntrada)
        {
            Fornecedor = fornecedor;
            IdFornecedor = fornecedor.Id;
            CodigoEntrada = codigoEntrada;
        }
    }
}