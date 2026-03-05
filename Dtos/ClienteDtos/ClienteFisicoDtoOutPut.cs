using ApiEstagioBicicletaria.Entities.ClienteDomain;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteFisicoDtoOutPut : ClienteDtoOutPut
    {
        public string Nome { get; set; }
       
        public string Cpf { get; set; }

        public ClienteFisicoDtoOutPut() { }

        public ClienteFisicoDtoOutPut(Guid id, Endereco endereco, DateTime dataCriacao, string telefone, string email, TipoCliente tipoCliente, bool podeExcluir, bool ativo,string nome,string cpf) : 
            base(id, endereco, dataCriacao, telefone, email, tipoCliente, podeExcluir, ativo)
        {
            Nome = nome;
            Cpf = cpf;
        }
    }
}
