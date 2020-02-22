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
using Market.Infrastructure.Helper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Market.Infrastructure.Data
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public IConfiguration Configuration { get; }

        public UserRepository(AppDbContext appDbContext, IConfiguration configuration) : base(appDbContext)
        {
            Configuration = configuration;
        }

        public async Task<User> AddNewUserAsync(User user)
        {
            var existingUsers = _appDbContext.User.FirstOrDefault(x => x.UserName == user.UserName);

            if (existingUsers != null)
            {
                return null;
            }

            AuthenticationData authenticationData = new AuthenticationData();

            authenticationData = GeneratePassword(user.Password);

            user.Salt = authenticationData.Salt;
            user.Password = authenticationData.HashedPassword;

            _appDbContext.User.Add(user);

            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public User Authenticate(string username, string password)
        {
            var salt = _appDbContext.User.FirstOrDefault(x => x.UserName == username).Salt;
            var hashedPassword = GeneratePasswordBySaltAndPassword(salt, password);
            var user = _appDbContext.User.SingleOrDefault(x => x.UserName == username && x.Password == hashedPassword);

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

        public async Task<Basket> AddToBasketAsync(BasketItem basketItem)
        {
            var stockCount = _appDbContext.Product.FirstOrDefault(x => x.Id == basketItem.ProductId).StockCount;

            if (stockCount < basketItem.Quantity)
            {
                return null;
            }

            var basket = _appDbContext.Basket.FirstOrDefault(x => x.UserId == basketItem.UserId && x.ActivationStatus);

            if (basket != null)
            {
                basket.Items.Add(basketItem);

                _appDbContext.Entry(basket).State = EntityState.Modified;

                var result = await _appDbContext.SaveChangesAsync();

                if (result == 1)
                {
                    return basket;
                }
            }
            else
            {
                Basket usersBasket = new Basket();

                usersBasket.UserId = basketItem.UserId;
                usersBasket.ActivationStatus = true;

                _appDbContext.Basket.Add(usersBasket);

                var result = await _appDbContext.SaveChangesAsync();

                if (result == 1)
                {
                    return basket;
                }
            }

            return null;
        }

        private AuthenticationData GeneratePassword(string password)
        {
            AuthenticationData authenticationData = new AuthenticationData();
            byte[] salt = new byte[128 / 8];

            using (var rndNumberGenerator = RandomNumberGenerator.Create())
            {
                rndNumberGenerator.GetBytes(salt);
            }

            string hashedPassword = GenerateHashedPassword(salt, password);

            authenticationData.HashedPassword = hashedPassword;
            authenticationData.Salt = Convert.ToBase64String(salt);

            return authenticationData;
        }

        private string GenerateHashedPassword(byte[] salt, string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        private string GeneratePasswordBySaltAndPassword(string salt, string password)
        {
            return GenerateHashedPassword(Convert.FromBase64String(salt), password);
        }
    }
}
