namespace ApiEstagioBicicletaria.Entities
{
    public class Fornecedor
    {
        public Guid Id { get; set; }=Guid.NewGuid();

        public DateTime DataCriacao { get; set; }=DateTime.Now;

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string RazaoSocial { get; set; }

        public string NomeFantasia { get; set; }

        public string Cnpj {  get; set; }

        public string InscricaoEstadual { get; set; }

        public bool Ativo { get; set; } = true;

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
