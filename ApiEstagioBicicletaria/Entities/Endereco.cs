namespace ApiEstagioBicicletaria.Entities
{
    public class Endereco
    {
        public Guid Id { get; private set; } = new Guid();
        public string Logradouro { get; set;}
        public string Numero { get; set;}
        public string Cidade { get; set;}
        public string SiglaUf {  get; set;}

        private Endereco()
        {

        }
        public Endereco(string logradouro,string numero, string cidade, string siglaUf)
        {
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Cidade = cidade;
            this.SiglaUf = siglaUf;
        }
    }
}
