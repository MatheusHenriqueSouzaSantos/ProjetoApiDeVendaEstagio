namespace ApiEstagioBicicletaria.Entities
{
    public class Fornecedor : EntidadeBase
    {

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string RazaoSocial { get; set; }

        public string NomeFantasia { get; set; }

        public string Cnpj {  get; set; }

        public string InscricaoEstadual { get; set; }

        public Fornecedor(string telefone, string email, string razaoSocial, string nomeFantasia, string cnpj, string inscricaoEstadual)
        {
            Telefone = telefone;
            Email = email;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            Cnpj = cnpj;
            InscricaoEstadual = inscricaoEstadual;
        }
    }
}
