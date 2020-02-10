using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Market.Core.Entities;
using Market.Core.Interfaces;
using Market.Infrastructure.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Market.Infrastructure.Data
{
    public class UserService : EfRepository<User> ,IUserService 
    {
        public IConfiguration Configuration { get; }

        public UserService(AppDbContext appDbContext, IConfiguration configuration) : base(appDbContext)
        {
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

        public async Task<User> AddNewUserAsync(User user)
        {
            var existingUsers = _appDbContext.User.Where(x => x.UserName == user.UserName).ToList();

            if (existingUsers != null)
            {
                return null;
            }

            SaltAndHashUsersPassword(user);

            _appDbContext.User.Add(user);

            await _appDbContext.SaveChangesAsync();

            return user;
        }

        void SaltAndHashUsersPassword(User user)
        {
            byte[] salt = new byte[128 / 8];

            using (var rndNumberGenerator = RandomNumberGenerator.Create())
            {
                rndNumberGenerator.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            user.Salt = Convert.ToBase64String(salt);
            user.Password = hashedPassword;
        }
    }
}
