using ApiEstagioBicicletaria.Entities.ClienteDomain;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteFisicoInativoOutputDto : ClienteInativoOutputDto
    {

        public string Nome { get; set; }

        public string Cpf { get; set; }
        public ClienteFisicoInativoOutputDto(string nome,string cpf,Guid id, Endereco endereco, DateTime dataCriacao, string telefone, 
            string email, TipoCliente tipoCliente, bool ativo) : base(id, endereco, dataCriacao, telefone, email, tipoCliente, ativo)
        {
            Nome = nome;
            Cpf = cpf;
        }


    }
}
