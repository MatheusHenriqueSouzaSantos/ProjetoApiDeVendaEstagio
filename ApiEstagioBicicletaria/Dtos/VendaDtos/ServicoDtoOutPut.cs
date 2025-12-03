namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ServicoDtoOutPut
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string CodigoDoServico { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public string NomeServico { get; set; }

        public string Descricao { get; set; }

        public decimal PrecoServico { get; set; }

        public bool Ativo { get; set; } = true;

        public bool PodeExcluir { get; set; }

        public ServicoDtoOutPut(Guid id, string codigoDoServico, DateTime dataCriacao, string nomeServico, string descricao, decimal precoServico, bool ativo, bool podeExcluir)
        {
            Id = id;
            CodigoDoServico = codigoDoServico;
            DataCriacao = dataCriacao;
            NomeServico = nomeServico;
            Descricao = descricao;
            PrecoServico = precoServico;
            Ativo = ativo;
            PodeExcluir = podeExcluir;
        }
    }
}
