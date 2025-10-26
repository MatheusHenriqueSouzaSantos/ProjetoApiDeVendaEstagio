﻿using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ItemVenda
    {
        //todos os atributos deixo que nunca podme ser alterados? se errou crie outro item venda?
        //deixar privado pois ao enviar a venda já esta concluida
        public Guid Id { get; private set; } = new Guid();

        public Venda Venda { get; private set; }

        public Produto Produto { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public int Quantidade { get; set; }

        public decimal DescontoUnitario { get;  set; }

        public decimal PrecoUnitarioNaVenda { get;  set; }

        public bool Ativo { get; set; } = true;

        protected ItemVenda()
        {

        }

        public ItemVenda(Guid id, Venda venda, Produto produto, int quantidade, decimal descontoPorUnidade, decimal precoUnitarioNaVenda)
        {
            Id = id;
            Venda = venda;
            Produto = produto;
            Quantidade = quantidade;
            DescontoPorUnidade = descontoPorUnidade;
            PrecoUnitarioNaVenda = precoUnitarioNaVenda;
        }
    }
}
