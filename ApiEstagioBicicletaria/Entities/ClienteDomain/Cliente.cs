namespace ApiEstagioBicicletaria.Entities.ClienteDomain
{
    public abstract class Cliente
    {
        //mudar atributos para set private e criar metodos especificos!!!
        public Guid Id { get; private set; } = Guid.NewGuid();
        //usar UUID
        public Endereco Endereco { get; set; }
        //Ter fk também ou só o objeto
        public DateTime DataCriacao { get; private set; } = DateTime.Now;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //deixo tipo cliente na entidade ou só no DTO?

        //campo tipo cliente desnecessário com rota para pessoa física e jurídica separada?
        public TipoCliente TipoCliente { get; private set; }
        //colocar no dto?
        public bool Ativo { get; set; } = true;

        protected Cliente()
        {

        }
        protected Cliente(Endereco endereco, string telefone, string email, TipoCliente tipoCliente)
        {
            Endereco = endereco;
            Telefone = telefone;
            Email = email;
            TipoCliente = tipoCliente;
            //this.Ativo= ativo;
        }
    }
}
