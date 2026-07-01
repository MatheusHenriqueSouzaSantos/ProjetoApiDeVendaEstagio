using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Repository.Repositorios
{
    public class UsuarioRepositorio
    {
        private readonly DbSet<Usuario> _usuarios;

        public UsuarioRepositorio(ContextoDb contexto)
        {
            _usuarios=contexto.Usuarios;
        }

        public List<Usuario> BuscarTodos()
        {
            return _usuarios.Where(u=>u.Ativo).ToList();
        }
        public Usuario? BuscarPorId(Guid id)
        {
            return _usuarios.FirstOrDefault(u => u.Id == id && u.Ativo);
        }

        public Usuario? BuscarPorEmail(string email)
        {
            return _usuarios.FirstOrDefault(u => u.Email == email && u.Ativo);
        }

        public bool UsuarioExistentePorEmail(string email)
        {
            return _usuarios.Any(u => u.Email == email && u.Ativo);
        }

        public void Cadastrar(Usuario usuario)
        {
            _usuarios.Add(usuario);
        }

        public void Atualizar(Usuario usuario)
        {
            _usuarios.Update(usuario);
        }
    }
}
