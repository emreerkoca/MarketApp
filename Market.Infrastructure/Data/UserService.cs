using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Market.Core.Entities;
using Market.Core.Interfaces;
using Market.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Market.Infrastructure.Data
{
    public class UserService : IUserService 
    {
        protected readonly AppDbContext _appDbContext;
        public IConfiguration Configuration { get; }

        public UserService(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            Configuration = configuration;
        }

        public User Authenticate(string username, string password)
        {
            var user = _appDbContext.User.SingleOrDefault(x => x.UserName == username && x.Password == GetSaltedEndHashedPassword(password));

            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AuthorityManagement:Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetUsersForClient()
        {
            throw new NotImplementedException();
        }

        string GetSaltedEndHashedPassword(string password)
        {
            return "";
        }
    }
}
