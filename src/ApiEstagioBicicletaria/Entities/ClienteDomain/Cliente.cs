namespace ApiEstagioBicicletaria.Entities.ClienteDomain
{
    public abstract class Cliente : EntidadeBase
    {
        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Endereco Endereco { get; set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid IdEndereco { get; set; }
        
        public string Telefone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [AtributoASerIgnoradoLogAtualizacao]
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
