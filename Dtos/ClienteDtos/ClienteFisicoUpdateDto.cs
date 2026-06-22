namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteFisicoUpdateDto : ClienteFisicoInputDto
    {
        public ClienteFisicoUpdateDto()
        {
        }

        public ClienteFisicoUpdateDto(string nome, EnderecoDto endereco, string telefone, string email) : base(nome,endereco, telefone, email)
        {
        }
    }
}
