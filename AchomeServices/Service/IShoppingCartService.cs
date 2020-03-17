using AchomeModels.DbModels;
using AchomeModels.Models.RequestModels;
using AchomeModels.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Service
{
    public interface IShoppingCartService
    {
        BaseResponse<bool> AddToShoppingCart(ShoppingCart shoppingCartModel);
        BaseResponse<List<ShoppingCartWrapper>> GetShoppingCart(string account);
        BaseResponse<bool> RemoveShoppingCartItem(List<PendedShoppingCartItemRequestModel> items,string account);
    }
}
