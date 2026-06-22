namespace ApiEstagioBicicletaria.Entities.ServicoDomain
{
    public class Servico: EntidadeBase
    {

        public string CodigoDoServico { get;  set; }

        public string NomeServico { get; set; }

        public string Descricao { get; set; }

        public decimal Preco { get; set; }


        protected Servico()
        {
        }

        public Servico(string codigoDoServico, string nomeServico, string descricao, decimal preco)
        {
            CodigoDoServico = codigoDoServico;
            NomeServico = nomeServico;
            Descricao = descricao;
            Preco = preco;
        }

        public Servico Copia()
        {
            return new Servico(CodigoDoServico, NomeServico, Descricao, Preco);
        }
    }
}
