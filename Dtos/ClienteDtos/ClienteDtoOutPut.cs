using ApiEstagioBicicletaria.Entities.ClienteDomain;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public abstract class ClienteDtoOutPut
    {
        public Guid Id { get; set; }
        public Endereco Endereco { get; set; }

        public DateTime DataCriacao { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public TipoCliente TipoCliente { get;  set; }
        public bool PodeExcluir { get; set; }

        public bool Ativo { get; set; } = true;

        protected ClienteDtoOutPut() { }

        protected ClienteDtoOutPut(Guid id, Endereco endereco, DateTime dataCriacao, string telefone, string email, TipoCliente tipoCliente, bool podeExcluir, bool ativo)
        {
            Id = id;
            Endereco = endereco;
            DataCriacao = dataCriacao;
            Telefone = telefone;
            Email = email;
            TipoCliente = tipoCliente;
            PodeExcluir = podeExcluir;
            Ativo = ativo;
        }
    }
}
