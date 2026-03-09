namespace ApiEstagioBicicletaria.Dtos.ProdutoDtos
{
    public class ProdutoDtoOutPut
    {
        public Guid Id { get;  set; } 

        public string CodigoDeBarra { get;  set; }

        public DateTime DataCriacao { get;  set; } 

        public string NomeProduto { get; set; }

        public string Descricao { get; set; }

        public decimal PrecoUnitario { get; set; }

        public bool Ativo { get; set; } = true;

        public bool PodeExcluir {  get; set; }

        public EstoqueSimplificadoOutputDto Estoque {  get; set; }

        public ProdutoDtoOutPut(Guid id, string codigoDeBarra, DateTime dataCriacao, string nomeProduto, 
            string descricao, decimal precoUnitario, bool ativo, bool podeExcluir, EstoqueSimplificadoOutputDto estoque)
        {
            Id = id;
            CodigoDeBarra = codigoDeBarra;
            DataCriacao = dataCriacao;
            NomeProduto = nomeProduto;
            Descricao = descricao;
            PrecoUnitario = precoUnitario;
            Ativo = ativo;
            PodeExcluir = podeExcluir;
            Estoque = estoque;
        }
    }
}
