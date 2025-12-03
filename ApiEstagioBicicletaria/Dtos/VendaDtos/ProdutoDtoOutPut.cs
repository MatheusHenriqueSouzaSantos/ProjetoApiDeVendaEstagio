namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ProdutoDtoOutPut
    {
        public Guid Id { get;  set; } 

        public string CodigoDeBarra { get;  set; }

        public DateTime DataCriacao { get;  set; } 

        public string NomeProduto { get; set; }

        public string Descricao { get; set; }

        public int QuantidadeEmEstoque { get; set; } 

        public decimal PrecoUnitario { get; set; }

        public bool Ativo { get; set; } = true;

        public bool PodeExcluir {  get; set; }

        public ProdutoDtoOutPut(Guid id, string codigoDeBarra, DateTime dataCriacao, string nomeProduto, string descricao, int quantidadeEmEstoque, decimal precoUnitario, bool ativo, bool podeExcluir)
        {
            Id = id;
            CodigoDeBarra = codigoDeBarra;
            DataCriacao = dataCriacao;
            NomeProduto = nomeProduto;
            Descricao = descricao;
            QuantidadeEmEstoque = quantidadeEmEstoque;
            PrecoUnitario = precoUnitario;
            Ativo = ativo;
            PodeExcluir = podeExcluir;
        }
    }
}
