using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddNewUserAsync(User user);
        User Authenticate(string username, string password);
        Task<Basket> AddToBasketAsync(BasketItem basketItem);
    }
}
