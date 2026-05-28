namespace ApiEstagioBicicletaria.Entities.VendedorDomain 
{ 
    public class Vendedor : EntidadeBase
    { 
        public string Telefone { get; set; } 
        public string Email { get; set; } 
        public string NomeCompleto { get; set; } 
        public string Cpf { get; set; } 

        public Vendedor(string telefone, string email, string nomeCompleto, string cpf)
        {
            Telefone = telefone;
            Email = email;
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
        }
    }
}