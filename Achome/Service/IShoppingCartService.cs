using Achome.DbModels;
using Achome.Models.RequestModels;
using Achome.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Service
{
    public interface IShoppingCartService
    {
        BaseResponse<bool> AddToShoppingCart(ShoppingCart shoppingCartModel);
        BaseResponse<List<ShoppingCartWrapper>> GetShoppingCart(string account);
        BaseResponse<bool> RemoveShoppingCartItem(List<RemoveShoppingCartItemRequestModel> items);
    }
}
