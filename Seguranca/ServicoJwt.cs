using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiEstagioBicicletaria.Seguranca
{
    public class ServicoJwt
    {
        private readonly string _chaveJwtEmString;

        public ServicoJwt(IConfiguration configuracao)
        {
            _chaveJwtEmString = configuracao["JWT_KEY"] ?? throw new Exception("Variavel de ambiente JWT_KEY não encontrada");
        }

        public string GerarJWT(Usuario usuario)
        {
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_chaveJwtEmString));
            var configuracoesDeAssinatura = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,usuario.Id.ToString()),
                new Claim(ClaimTypes.Name,usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role,usuario.PerfilUsuario.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: configuracoesDeAssinatura
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
