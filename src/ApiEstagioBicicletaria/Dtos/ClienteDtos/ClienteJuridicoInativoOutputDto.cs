using ApiEstagioBicicletaria.Entities.ClienteDomain;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteJuridicoInativoOutputDto : ClienteInativoOutputDto
    {

        public string RazaoSocial { get; set; }

        public string NomeFantasia { get; set; } = string.Empty;

        public string InscricaoEstadual { get; set; } = string.Empty;
        public string Cnpj { get; set; }
        public ClienteJuridicoInativoOutputDto(string razaoSocial,string nomeFantasia,string inscricaoEstadual,string cnpj,Guid id, Endereco endereco,
            DateTime dataCriacao, string telefone, string email, TipoCliente tipoCliente, 
            bool ativo) : base(id, endereco, dataCriacao, telefone, email, tipoCliente, ativo)
        {
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            InscricaoEstadual = inscricaoEstadual;  
            Cnpj = cnpj;
        }
    }
    
}
