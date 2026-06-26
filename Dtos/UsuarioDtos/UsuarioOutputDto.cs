using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Dtos.Usuario
{
    public class UsuarioOutputDto
    {
        public Guid Id { get;  set; }

        public DateTime DataCriacao { get;  set; }

        public bool Ativo { get;  set; }

        public string Nome { get;  set; }

        public string Email { get;  set; }

        public PerfilUsuario PerfilUsuario { get;  set; }

        public UsuarioOutputDto(Guid id, DateTime dataCriacao, bool ativo, string nome, string email, PerfilUsuario perfilUsuario)
        {
            Id = id;
            DataCriacao = dataCriacao;
            Ativo = ativo;
            Nome = nome;
            Email = email;
            PerfilUsuario = perfilUsuario;
        }
    }
}
