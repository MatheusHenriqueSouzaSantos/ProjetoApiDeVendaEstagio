namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteJuridicoUpdateDto : ClienteJuridicoInputDto
    {
        public ClienteJuridicoUpdateDto()
        {
        }

        public ClienteJuridicoUpdateDto(EnderecoDto endereco, string telefone, string email) : base(endereco, telefone, email)
        {
        }
    }
}
