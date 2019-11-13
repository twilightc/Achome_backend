using Achome.DbModels;
using Achome.Models;
using Achome.Models.RequestModels;
using Achome.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Achome.Service
{
    public interface IMerchandiseService
    {
        Task<BaseResponse<List<CategoryListViewModel>>> GetCategoryListAsync();
        BaseResponse<List<MerchandiseViewModel>> GetCategoryDetailItems(string CategoryId, string CategoryDetailId);
        BaseResponse<MerchandiseWrapper> GetMerchandiseListBySearching(SearchRequestModel searchRequestModel);
       
        BaseResponse<MerchandiseViewModel> GetMerchandise(string ItemId);

        BaseResponse<bool> AddToShoppingCart(ShoppingCart shoppingCartModel);

        BaseResponse<List<ShoppingCartWrapper>> GetShoppingCart(string account);
    }
}
