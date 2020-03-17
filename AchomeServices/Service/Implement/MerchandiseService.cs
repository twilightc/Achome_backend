using AchomeModels.DbModels;
using AchomeModels.Models;
using AchomeModels.Models.RequestModels;
using AchomeModels.Models.ResponseModels;
using AchomeModels.Util;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Service.Implement
{
    public class MerchandiseService : IMerchandiseService
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IMapper mapper;
        private readonly string _folder;
        public MerchandiseService(IOptions<ApplicationSettings> appSettings, AChomeContext context, IMapper mapper, ILogger<MerchandiseService> logger, IHostingEnvironment env)
        {
            logger.LogDebug("MerchandiseService");
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.mapper = mapper;
            this._folder = $@"{env.WebRootPath}\img";
        }

        public async Task<BaseResponse<List<CategoryListViewModel>>> GetCategoryListAsync()
        {
            using (var con = new SqlConnection(appSettings.IdentityConnection))
            {
                await con.OpenAsync().ConfigureAwait(false);

                var rawData = con.Query<RawCategoryListViewModel>(@"select * from Category cat
  left join CategoryDetail on cat.cid = CategoryDetail.Cid");
                List<CategoryListViewModel> result = new List<CategoryListViewModel>();


                foreach (var data in rawData)
                {
                    var categoryMain = result.SingleOrDefault(a => string.Equals(a.Cid, data.Cid, StringComparison.InvariantCulture));
                    var detail = new CategoryListDetailViewModel()
                    {
                        DetailId = data.DetailId,
                        DetailName = data.DetailName,
                        Seq = data.Seq
                    };
                    if (categoryMain == null)
                    {
                        result.Add(new CategoryListViewModel() { Cid = data.Cid, Cname = data.Cname, GroupSeq = data.GroupSeq, Detail = new List<CategoryListDetailViewModel>() { detail } }); ;
                    }
                    else
                    {
                        categoryMain.Detail.Add(detail);
                    }
                }
                return new BaseResponse<List<CategoryListViewModel>>(true, "", result);
            }
        }

        public BaseResponse<List<MerchandiseViewModel>> GetCategoryDetailItems(string CategoryId, string CategoryDetailId)
        {
            try
            {
                var rawData = context.Merchandise.Where(data => data.CategoryId.Equals(CategoryId, StringComparison.InvariantCulture) && data.CategoryDetailId.Equals(CategoryDetailId, StringComparison.InvariantCulture)).Page(0, 15);
                var result = rawData.Select(data => mapper.Map<Merchandise, MerchandiseViewModel>(data)).ToList();

                return new BaseResponse<List<MerchandiseViewModel>>(true, "", result);
            }
            catch (Exception)
            {
                return new BaseResponse<List<MerchandiseViewModel>>(false, "cannot get detailItems list", null);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:不要將常值當做已當地語系化的參數傳遞", Justification = "<暫止>")]
        public BaseResponse<MerchandiseViewModel> GetMerchandise(string ItemId)
        {
            try
            {
                var specData = context.MerchandiseSpec.Where(data => data.MerchandiseId.Equals(ItemId)).ToList();
                var specResult = specData.Select(data => mapper.Map<MerchandiseSpec, MerchandiseSpecViewModel>(data)).ToList();

                var qaData = context.MerchandiseQa.Where(data => data.MerchandiseId.Equals(ItemId)).ToList();
                var qaResult = qaData.Select(data => mapper.Map<MerchandiseQa, MerchandiseQaViewModel>(data)).ToList();

                var rawData = context.Merchandise.SingleOrDefault(data => data.MerchandiseId.Equals(ItemId));
                if (rawData == null)
                {
                    throw new Exception("cannot get merchandise by Id");
                }
                var result = mapper.Map<Merchandise, MerchandiseViewModel>(rawData);
                result.MerchandiseSpec = specResult;
                result.MerchandiseQa = qaResult;
                result.Spec1 = specResult.Select(data => data.Spec1).Distinct().ToList();
                result.Spec2 = specResult.Select(data => data.Spec2).Distinct().ToList();
                return new BaseResponse<MerchandiseViewModel>(true, "", result);

            }
            catch (Exception ex)
            {
                return new BaseResponse<MerchandiseViewModel>(false, ex.Message, null);
            }
        }

        public BaseResponse<MerchandiseWrapper> GetMerchandiseListBySearching(SearchRequestModel searchRequestModel)
        {
            try
            {
                if (searchRequestModel == null)
                {
                    throw new ArgumentNullException(nameof(searchRequestModel));
                }
                bool isPartOfCategory = false;
                var rawData = context.Merchandise.AsQueryable();
                if (searchRequestModel.CategoryId != null)
                {
                    isPartOfCategory = true;
                    rawData = rawData.Where(data => data.CategoryId.Equals(searchRequestModel.CategoryId, StringComparison.InvariantCulture));
                }

                if (searchRequestModel.CategoryDetailId != null)
                {
                    isPartOfCategory = true;
                    rawData = rawData.Where(data => data.CategoryDetailId.Equals(searchRequestModel.CategoryDetailId, StringComparison.InvariantCulture));
                }
                if (searchRequestModel.Keyword != null)
                {
                    rawData = rawData.Where(data => data.MerchandiseTitle.ToLower(CultureInfo.CurrentCulture).Contains(searchRequestModel.Keyword, StringComparison.InvariantCulture));
                }

                switch (searchRequestModel.SortType)
                {
                    case SortTypeEnum.Price:
                        //rawData = rawData.OrderBy(data => data.Price);
                        rawData = rawData.SortByOrder(data => data.Price, searchRequestModel.OrderType ?? OrderTypeEnum.None);
                        break;
                    case SortTypeEnum.SoldQty:
                        //rawData = rawData.OrderBy(data => data.SoldQty);
                        rawData = rawData.SortByOrder(data => data.SoldQty, searchRequestModel.OrderType ?? OrderTypeEnum.None);
                        break;
                    case SortTypeEnum.Date:
                        break;
                    default:
                        break;
                }

                int rawDatacount = rawData.Count();
                var rawDataList = rawData.Page(searchRequestModel.PageIndex, searchRequestModel.PageSize).Select(data => mapper.Map<Merchandise, MerchandiseViewModel>(data)).ToList();
                MerchandiseWrapper result = new MerchandiseWrapper()
                {
                    MerchandiseViewModel = rawDataList,
                    MerchandiseAmount = isPartOfCategory ? rawDatacount : context.Merchandise.Count()
                };

                return new BaseResponse<MerchandiseWrapper>(true, "", result);
            }
            catch (Exception)
            {
                return new BaseResponse<MerchandiseWrapper>(false, "cannot get merchandise list", null);
            }
        }

        public BaseResponse<bool> PostAskingForm(MerchandiseQa form)
        {
            try
            {
                if (form == null)
                {
                    throw new ArgumentNullException(nameof(form));
                }


                var result = context.MerchandiseQa.Where(data => data.MerchandiseId == form.MerchandiseId).ToList();


                if (result == null)
                {
                    context.MerchandiseQa.Add(form);
                }
                else
                {
                    form.AskingTime = DateTime.Now;
                    form.Seq = result.Count + 1;
                    context.MerchandiseQa.Add(form);
                }


                context.SaveChanges();
                return new BaseResponse<bool>(true, "發佈提問完成!", default);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, ex.Message, default);
            }
        }

        public async Task<BaseResponse<bool>> AddMerchandise(AddMerchandiseRequestModel addMerchandiseRequestModel, string account)
        {
            string path = "";
            try
            {
                if (addMerchandiseRequestModel == null)
                {
                    throw new ArgumentNullException(nameof(addMerchandiseRequestModel));
                }

                string guid = Guid.NewGuid().ToString();
                var size = addMerchandiseRequestModel.MerchandisePhotos.Length;
                if (size > 0)
                {
                    path = $@"{_folder}\{guid}.{addMerchandiseRequestModel.MerchandisePhotos.FileName.Split(".")[1]}";
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await addMerchandiseRequestModel.MerchandisePhotos.CopyToAsync(stream).ConfigureAwait(false);
                    }
                }
                var toBeAddItem = mapper.Map<AddMerchandiseRequestModel, Merchandise>(addMerchandiseRequestModel);
                toBeAddItem.MerchandiseId = guid;
                toBeAddItem.OwnerAccount = account;
                toBeAddItem.ImagePath = $"{guid}.{addMerchandiseRequestModel.MerchandisePhotos.FileName.Split(".")[1]}";

                context.Merchandise.Add(toBeAddItem);
                if (addMerchandiseRequestModel.EnableSpec)
                {
                    int specIndex = 1;
                    var specs = addMerchandiseRequestModel.SpecList.Select(data =>
                    {
                        var temp = mapper.Map<AddSpecModel, MerchandiseSpec>(data);
                        temp.MerchandiseId = guid;
                        temp.SpecId = specIndex++;
                        return temp;
                    });

                    context.MerchandiseSpec.AddRange(specs);
                }

                context.SaveChanges();
                return new BaseResponse<bool>(true, "上傳成功", true);
            }
            catch (Exception ex)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return new BaseResponse<bool>(false, "上傳失敗", false);
            }
        }

        public async Task<BaseResponse<MerchandiseWrapper>> GetMerhandiseListDetail(BackStageSearchModel backStageSearchModel, string account)
        {
            try
            {
                if (backStageSearchModel == null)
                {
                    throw new ArgumentNullException(nameof(backStageSearchModel));
                }
                var customerData = context.Merchandise.Where(data => data.OwnerAccount == account);

                Console.WriteLine(backStageSearchModel.PageIndex);
                var customerDataList = customerData.OrderBy(backStageSearchModel.OrderString, backStageSearchModel.isOrderByDesc).Page(backStageSearchModel.PageIndex, backStageSearchModel.PageSize).Select(data => mapper.Map<Merchandise, MerchandiseViewModel>(data)).ToList();
                MerchandiseWrapper result = new MerchandiseWrapper()
                {
                    MerchandiseViewModel = customerDataList,
                    MerchandiseAmount = customerData.Count()
                };
                Console.WriteLine(result);



                return new BaseResponse<MerchandiseWrapper>(true, "", result);
            }
            catch (Exception ex)
            {
                return new BaseResponse<MerchandiseWrapper>(false, "cannot get merchandise list", null);
            }
        }

        public async Task<BaseResponse<bool>> EditMerchandise(MerchandiseViewModel merchandise)
        {
            try
            {
                if (merchandise == null)
                {
                    throw new ArgumentNullException(nameof(merchandise));
                }

                var beenModifiedData = context.Merchandise.Where(data => data.MerchandiseId == merchandise.MerchandiseId).FirstOrDefault();
                beenModifiedData.MerchandiseTitle = merchandise.MerchandiseTitle;
                beenModifiedData.MerchandiseContent = merchandise.MerchandiseContent;


                var beenModifiedSpecData = context.MerchandiseSpec.Where(data => data.MerchandiseId == merchandise.MerchandiseId).ToList();
                if (beenModifiedSpecData != null)
                {
                    int totalAmount = 0;
                    for (int i = 0; i < merchandise.MerchandiseSpec.Count; i++)
                    {
                        MerchandiseSpecViewModel newData = merchandise.MerchandiseSpec[i];
                        totalAmount += newData.RemainingQty;
                        beenModifiedSpecData[i].RemainingQty = newData.RemainingQty;
                    }

                    beenModifiedData.RemainingQty = totalAmount;
                }

                context.SaveChanges();
                return new BaseResponse<bool>(true, "修改商品資料完成", true);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, "edit failed", default);
            }
        }

    }
}
