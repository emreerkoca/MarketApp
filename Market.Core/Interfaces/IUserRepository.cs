using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Core.Interfaces
{
    public interface IUserRepository
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetUsersForClient();
        Task<User> AddNewUserAsync(User user);
        Task<Basket> AddToBasketAsync(BasketItem basketItem);
    }
}
