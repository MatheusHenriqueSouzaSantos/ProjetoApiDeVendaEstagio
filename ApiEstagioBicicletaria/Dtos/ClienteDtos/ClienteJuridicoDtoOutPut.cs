using ApiEstagioBicicletaria.Entities.ClienteDomain;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteJuridicoDtoOutPut : ClienteDtoOutPut
    {
        public string RazaoSocial { get; set; }

        public string NomeFantasia { get; set; } = string.Empty;

        public string InscricaoEstadual { get; set; } = string.Empty;
        public string Cnpj { get; set; }

        public ClienteJuridicoDtoOutPut()
        {

        }

        public ClienteJuridicoDtoOutPut(Guid id, Endereco endereco, DateTime dataCriacao, string telefone, string email, TipoCliente tipoCliente, bool podeExcluir, bool ativo,string razaoSocial,
            string nomeFantasia,string inscricaoestadual,string cnpj) : 
            base(id, endereco, dataCriacao, telefone, email, tipoCliente, podeExcluir, ativo)
        {
            RazaoSocial= razaoSocial;
            NomeFantasia= nomeFantasia;
            InscricaoEstadual = inscricaoestadual;
            Cnpj= cnpj;
        }

    }
}
