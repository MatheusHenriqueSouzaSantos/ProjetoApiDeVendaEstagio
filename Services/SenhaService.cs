using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;

namespace ApiEstagioBicicletaria.Services
{
    public class SenhaService
    {
        public string GerarHashDaSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool ValidarSenha(string hashSenhaSalva, string senhaInformada)
        {
            var resultado = BCrypt.Net.BCrypt.Verify(senhaInformada, hashSenhaSalva);

            return resultado;
        }
    }
}