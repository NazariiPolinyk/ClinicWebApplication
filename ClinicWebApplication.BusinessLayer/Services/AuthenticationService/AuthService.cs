using ClinicWebApplication.Interfaces;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System;

namespace ClinicWebApplication.BusinessLayer.Services.AuthenticationService
{
    public class AuthService<T> : IAuthService<T> 
        where T : IAccount, IEntity
    {
        private readonly IRepository<T> _repository;
        private readonly TokenOptions _tokenOptions;
        public AuthService(IRepository<T> repository, IOptions<TokenOptions> tokenOptions)
        {
            _repository = repository;
            _tokenOptions = tokenOptions.Value;

        }

        public IAccount Authenticate(string email, string password)
        {
            var account = _repository.GetAll().Result.AsQueryable()
                .SingleOrDefault(x => x.Email == email && x.Password == password);

            if (account == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Name, account.Email),
                    new Claim(ClaimTypes.Role, account.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            account.Token = tokenHandler.WriteToken(token);

            return account;
        }
    }
}
