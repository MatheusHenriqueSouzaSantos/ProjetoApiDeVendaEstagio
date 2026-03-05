namespace ApiEstagioBicicletaria.Entities 
{ 
    public class Vendedor { 
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public DateTime DataCriacao { get; set; }=DateTime.Now; 
        public string Telefone { get; set; } 
        public string Email { get; set; } 
        public string NomeCompleto { get; set; } 
        public string Cpf { get; set; } 
        public bool Ativo { get; set; }=true;

        public Vendedor(string telefone, string email, string nomeCompleto, string cpf)
        {
            Telefone = telefone;
            Email = email;
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
        }
    }
}