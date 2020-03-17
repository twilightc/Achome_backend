
using AchomeModels.DbModels;
using AchomeModels.Models;
using AchomeModels.Models.RequestModels;
using AchomeModels.Models.ResponseModels;
using AchomeModels.Service;
using AchomeModels.Util;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchandiseController : ControllerBase
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IMerchandiseService merchandiseService;
        private readonly IMapper mapper;


        public MerchandiseController(IOptions<ApplicationSettings> appSettings, IMerchandiseService merchandiseService, AChomeContext context, IMapper mapper)
        {
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.merchandiseService = merchandiseService;
            this.mapper = mapper;

        }

        [HttpGet("[action]")]
        public async Task<BaseResponse<List<CategoryListViewModel>>> GetCategoryListAsync()
        {
            return await merchandiseService.GetCategoryListAsync().ConfigureAwait(false);
        }

        [HttpGet("[action]")]
        public BaseResponse<List<MerchandiseViewModel>> GetCategoryDetailItems([FromQuery]string CategoryId, string CategoryDetailId)
        {
            return this.merchandiseService.GetCategoryDetailItems(CategoryId, CategoryDetailId);
        }

        [HttpGet("[action]")]
        public BaseResponse<MerchandiseViewModel> GetMerchandise([FromQuery]string ItemId)
        {
            return this.merchandiseService.GetMerchandise(ItemId);
        }


        [HttpPost("[action]")]
        public BaseResponse<bool> PostAskingForm([FromBody]MerchandiseQa form)
        {
            if (form == null)
            {
                return new BaseResponse<bool>(false, "model data is null", default);
            }
            var Account = User.Claims.Where(c => c.Type.Equals(ClaimString.AccountName, StringComparison.InvariantCulture)).FirstOrDefault().Value;
            form.QuestionAccount = Account;

            return this.merchandiseService.PostAskingForm(form);
        }

        [HttpPost("[action]")]
        public BaseResponse<MerchandiseWrapper> GetMerchandiseListBySearching([FromBody]SearchRequestModel searchRequestModel)
        {

            return this.merchandiseService.GetMerchandiseListBySearching(searchRequestModel);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<BaseResponse<bool>> AddMerchandise([FromForm]AddMerchandiseRequestModel addMerchandiseRequestModel)
        {
            string account = "tychang";
            return await merchandiseService.AddMerchandise(addMerchandiseRequestModel, account).ConfigureAwait(false);
        }

        /// <summary>
        /// Get backstage's merchandise list by specfic condition
        /// </summary>
        /// <param name="backStageSearchModel"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost("[action]")]
        public async Task<BaseResponse<MerchandiseWrapper>> GetBSMerhandiseListDetail([FromBody]BackStageSearchModel backStageSearchModel)
        {
            string account = "tychang";
            //backStageSearchModel.PageIndex = 0;
            //backStageSearchModel.PageSize = 1;
            //backStageSearchModel.OrderString = "Price";
            return await merchandiseService.GetMerhandiseListDetail(backStageSearchModel, account).ConfigureAwait(false);
        }

        /// <summary>
        /// Edit BackStage's merchandise info
        /// </summary>
        /// <param name="merchandise"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<BaseResponse<bool>> EditBSMerchandise([FromBody]MerchandiseViewModel merchandise)
        {
            return await merchandiseService.EditMerchandise(merchandise).ConfigureAwait(false);
        }

    }
}
