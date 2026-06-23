namespace ApiEstagioBicicletaria.Entities.ClienteDomain
{
    public abstract class Cliente : EntidadeBase
    {
        [AnotacaoDeAtributoASerIgnoradoLog]
        public Endereco Endereco { get; set; }
        
        public Guid IdEndereco { get; set; }
        
        public string Telefone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public TipoCliente TipoCliente { get; private set; }


        protected Cliente()
        {

        }
        protected Cliente(Endereco endereco, string telefone, string email, TipoCliente tipoCliente)
        {
            Endereco = endereco;
            IdEndereco=endereco.Id;
            Telefone = telefone;
            Email = email;
            TipoCliente = tipoCliente;
        }
    }
}
