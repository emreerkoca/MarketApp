using Market.Api.Controllers;
using Market.Core.Entities;
using Market.Core.Interfaces;
using Market.Infrastructure.Data;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Market.Api.UnitTest
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private UserController _userController;

        public UserControllerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userController = new UserController(_mockUserRepository.Object);
        }

        [Fact]
        public async Task AddToBasket_ModelStateValidAsync()
        {
            BasketItem basketItem = null;

            _mockUserRepository.Setup(r => r.AddToBasketAsync(It.IsAny<BasketItem>()))
                .Callback<BasketItem>(x => basketItem = x);

            var sampleItem = new BasketItem
            {
                ProductId = 3,
                UserId = 1,
                Name = "Sample Item",
                Price = 12.99,
                Quantity = 12
            };

            await _userController.AddToBasket(sampleItem);

            _mockUserRepository.Verify(x => x.AddToBasketAsync(It.IsAny<BasketItem>()), Times.Once);

            Assert.Equal(basketItem.ProductId, sampleItem.ProductId);
            Assert.Equal(basketItem.UserId, sampleItem.UserId);
            Assert.Equal(basketItem.Name, sampleItem.Name);
            Assert.Equal(basketItem.Price, sampleItem.Price);
            Assert.Equal(basketItem.Quantity, sampleItem.Quantity);
        }
    }
}
