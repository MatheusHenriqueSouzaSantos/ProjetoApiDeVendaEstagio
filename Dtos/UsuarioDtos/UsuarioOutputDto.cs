namespace ApiEstagioBicicletaria.Dtos.Usuario
{
    public class UsuarioOutputDto
    {
        public Guid Id { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public bool Ativo { get; private set; }

        public string Nome { get; private set; }

        public string Email { get; private set; }

        public UsuarioOutputDto(Guid id, DateTime dataCriacao, bool ativo, string nome, string email)
        {
            Id = id;
            DataCriacao = dataCriacao;
            Ativo = ativo;
            Nome = nome;
            Email = email;
        }
    }
}
