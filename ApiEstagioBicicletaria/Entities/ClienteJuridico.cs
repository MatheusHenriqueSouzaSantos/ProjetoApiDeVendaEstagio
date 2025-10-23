namespace ApiEstagioBicicletaria.Entities
{
    public class ClienteJuridico : Cliente
    {
        public string RazaoSocial { get; set; }=string.Empty;
        public string NomeFantasia { get; set; }=string.Empty;
        public string InscricaoEstadual { get; set; } = string.Empty;
        public string Cnpj { get; private set; } = string.Empty;
        
        protected ClienteJuridico(): base() {
        }
        public ClienteJuridico(Endereco endereco,string telefone,
            string email,
            string razaoSocial,string nomeFantasia,string inscricaoEstadual, string cnpj)
            :base(endereco,telefone,email,TipoCliente.PessoaJuridica)
        {
            this.RazaoSocial = razaoSocial;
            this.NomeFantasia = nomeFantasia;
            this.InscricaoEstadual = inscricaoEstadual;
            this.Cnpj= cnpj;
        }
    }
}
