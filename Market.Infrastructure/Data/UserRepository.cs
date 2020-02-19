﻿using System;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Market.Infrastructure.Data
{
    public class UserRepository : EfRepository<User> ,IUserRepository 
    {
        public IConfiguration Configuration { get; }

        public UserRepository(AppDbContext appDbContext, IConfiguration configuration) : base(appDbContext)
        {
            Configuration = configuration;
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

        public User Authenticate(string username, string password)
        {
            //var user = _appDbContext.User.SingleOrDefault(x => x.UserName == username && x.Password == GetSaltedEndHashedPassword(password));

            //if (user == null)
            //{
            //    return null;
            //}

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AuthorityManagement:Secret"));
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[] {
            //        new Claim(ClaimTypes.Name, user.Id.ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);

            //user.Token = tokenHandler.WriteToken(token);
            //user.Password = null;

            //return user;
            
            //It' s created temporarily. It' ll change. 
            User user = new User();

            user.FirstName = "test";
            user.LastName = "test";
            user.UserName = "test";
            user.Password = "test";

            if (username != user.UserName && password != user.Password)
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
            var stockCount = _appDbContext.Product.Single(x => x.Id == basketItem.ProductId).StockCount;

            if (stockCount < basketItem.Quantity)
            {
                return null;
            }

            var basket = _appDbContext.Basket.Single(x => x.UserId == basketItem.UserId && x.ActivationStatus);

            if (basket != null)
            {
                basket.Items.Add(basketItem);

                _appDbContext.Entry(basket).State = EntityState.Modified;

                var result = await _appDbContext.SaveChangesAsync();

                if(result == 1) 
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
    }
}
