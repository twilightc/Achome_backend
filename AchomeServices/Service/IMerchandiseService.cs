using AchomeModels.DbModels;
using AchomeModels.Models;
using AchomeModels.Models.RequestModels;
using AchomeModels.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AchomeModels.Service
{
    public interface IMerchandiseService
    {
        Task<BaseResponse<List<CategoryListViewModel>>> GetCategoryListAsync();
        BaseResponse<List<MerchandiseViewModel>> GetCategoryDetailItems(string CategoryId, string CategoryDetailId);
        BaseResponse<MerchandiseWrapper> GetMerchandiseListBySearching(SearchRequestModel searchRequestModel);
        BaseResponse<MerchandiseViewModel> GetMerchandise(string ItemId);
        BaseResponse<bool> PostAskingForm(MerchandiseQa form);
        
        Task<BaseResponse<bool>> AddMerchandise(AddMerchandiseRequestModel addMerchandiseRequestModel,string account);

        Task<BaseResponse<bool>> EditMerchandise(MerchandiseViewModel merchandise);

        Task<BaseResponse<MerchandiseWrapper>> GetMerhandiseListDetail(BackStageSearchModel backStageSearchModel,string account);

        
    }
}
