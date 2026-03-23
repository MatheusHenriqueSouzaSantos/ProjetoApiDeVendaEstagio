namespace ApiEstagioBicicletaria.Entities.ServicoDomain
{
    public class Servico: EntityBase
    {

        public string CodigoDoServico { get; private set; }

        public string NomeServico { get; set; }

        public string Descricao { get; set; }

        public decimal PrecoServico { get; set; }


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
