using Microsoft.AspNetCore.Identity;

namespace ApiEstagioBicicletaria.Services
{
    public class SenhaService
    {
        private PasswordHasher<object> _passwordHasher;

        public SenhaService(PasswordHasher<object> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string GerarHashDaSenha(string senha)
        {
            return _passwordHasher.HashPassword(null, senha);
        }

        public bool ValidarSenha(string hashSenhaSalva, string senhaInformada)
        {
            var resultado= _passwordHasher.VerifyHashedPassword(null, hashSenhaSalva, senhaInformada);

            return resultado == PasswordVerificationResult.Success;
        }
    }
}
