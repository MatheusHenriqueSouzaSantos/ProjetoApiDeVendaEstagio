using ApiEstagioBicicletaria.Entities.ClienteDomain;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public abstract class ClienteInativoOutputDto
    {
        public Guid Id { get; set; }
        public Endereco Endereco { get; set; }

        public DateTime DataCriacao { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public TipoCliente TipoCliente { get; set; }
        public bool Ativo { get; set; } = true;

        protected ClienteInativoOutputDto(Guid id, Endereco endereco, DateTime dataCriacao, string telefone, string email, 
            TipoCliente tipoCliente, bool ativo)
        {
            Id = id;
            Endereco = endereco;
            DataCriacao = dataCriacao;
            Telefone = telefone;
            Email = email;
            TipoCliente = tipoCliente;
            Ativo = ativo;
        }
    }
}
