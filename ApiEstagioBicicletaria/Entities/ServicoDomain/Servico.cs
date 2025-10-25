namespace ApiEstagioBicicletaria.Entities.ServicoDomain
{
    public class Servico
    {
        public Guid Id { get; private set; } = new Guid();

        public string CodigoDoServico { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public string NomeServico { get; set; }

        public string Descricao { get; set; }

        public decimal PrecoServico { get; set; }

        public bool Ativo { get; set; } = true;


        protected Servico()
        {
        }

        public Servico(string codigoDoServico, string nomeServico, string descricao, decimal precoServico)
        {
            CodigoDoServico = codigoDoServico;
            NomeServico = nomeServico;
            Descricao = descricao;
            PrecoServico = precoServico;
        }
    }
}
