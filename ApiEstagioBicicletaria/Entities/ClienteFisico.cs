namespace ApiEstagioBicicletaria.Entities
{
    public class ClienteFisico: Cliente
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; private set; } = string.Empty;

        public ClienteFisico(): base()
        {

        }
        public ClienteFisico(Endereco endereco,string telefone, string email, 
            string nome,string cpf) : base(endereco,telefone,email,TipoCliente.PessoaFisica)
        {
            this.Nome = nome;
            this.Cpf = cpf;
        }
    }
}
