namespace ApiEstagioBicicletaria.Entities
{
    public class Servico
    {
        public Guid Id { get; private set; } = new Guid();

        public string CodigoServico { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public string NomeServico { get; set; }

        public string Descricao { get; set; }

        public decimal PrecoServico { get; set; }

        public bool Ativo { get; set; } = true;


        protected Servico() { 
        }

        public Servico(string codigoServico, string nomeServico, string descricao, decimal precoServico)
        {
            CodigoServico = codigoServico;
            NomeServico = nomeServico;
            Descricao = descricao;
            PrecoServico = precoServico;
        }
    }
}
