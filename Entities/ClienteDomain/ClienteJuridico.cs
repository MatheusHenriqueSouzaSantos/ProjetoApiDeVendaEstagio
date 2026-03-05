namespace ApiEstagioBicicletaria.Entities.ClienteDomain
{
    public class ClienteJuridico : Cliente
    {
        public string RazaoSocial { get; set; } = string.Empty;
        //devo colocar ? pois nome fantasia pode ser nulo
        //mais não deixo setar como nulo por padrão lá no service, verificar
        public string NomeFantasia { get; set; } = string.Empty;
        public string InscricaoEstadual { get; set; } = string.Empty;
        public string Cnpj { get; private set; } = string.Empty;

        protected ClienteJuridico() : base()
        {
        }
        public ClienteJuridico(Endereco endereco, string telefone,
            string email,
            string razaoSocial, string nomeFantasia, string inscricaoEstadual, string cnpj)
            : base(endereco, telefone, email, TipoCliente.PessoaJuridica)
        {
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            InscricaoEstadual = inscricaoEstadual;
            Cnpj = cnpj;
        }
    }
}
