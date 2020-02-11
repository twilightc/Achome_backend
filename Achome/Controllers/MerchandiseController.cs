using Achome.DbModels;
using Achome.Models;
using Achome.Models.RequestModels;
using Achome.Models.ResponseModels;
using Achome.Service;
using Achome.Util;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchandiseController : ControllerBase
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IMerchandiseService merchandiseService;
        private readonly IMapper mapper;
        

        public MerchandiseController(IOptions<ApplicationSettings> appSettings, IMerchandiseService merchandiseService ,AChomeContext context, IMapper mapper)
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

        
        [HttpPost("[action]")]
        public async Task<BaseResponse<bool>> AddMerchandise([FromForm]AddMerchandiseRequestModel addMerchandiseRequestModel)
        {
            string account = "tychang";
            return await merchandiseService.AddMerchandise( addMerchandiseRequestModel, account).ConfigureAwait(false);
        }

    }
}
