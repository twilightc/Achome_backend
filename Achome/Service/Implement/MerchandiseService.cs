using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Achome.DbModels;
using Achome.Models;
using Achome.Models.RequestModels;
using Achome.Models.ResponseModels;
using Achome.Util;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;

namespace Achome.Service.Implement
{
    public class MerchandiseService : IMerchandiseService
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IMapper mapper;
        public MerchandiseService(IOptions<ApplicationSettings> appSettings, AChomeContext context, IMapper mapper)
        {
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.mapper = mapper;
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
                var specData = context.MerchandiseSpec.Where(data => data.MerchandiseId.Equals(ItemId, StringComparison.InvariantCulture)).ToList();
                var specResult = specData.Select(data => mapper.Map<MerchandiseSpec, MerchandiseSpecViewModel>(data)).ToList();

                var qaData = context.MerchandiseQa.Where(data => data.MerchandiseId.Equals(ItemId, StringComparison.InvariantCulture)).ToList();
                var qaResult = qaData.Select(data => mapper.Map<MerchandiseQa, MerchandiseQaViewModel>(data)).ToList();

                var rawData = context.Merchandise.SingleOrDefault(data => data.MerchandiseId.Equals(ItemId, StringComparison.InvariantCulture));
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


        public BaseResponse<bool> AddToShoppingCart(ShoppingCart shoppingCartModel)
        {
            try
            {
                if (shoppingCartModel == null)
                {
                    throw new ArgumentNullException(nameof(shoppingCartModel));
                }
                shoppingCartModel.AddTime = DateTime.Now;

                var result = context.ShoppingCart.Where(data => data.ProdId == shoppingCartModel.ProdId && data.SpecId == shoppingCartModel.SpecId).FirstOrDefault();
                if (result == null)
                {
                    context.ShoppingCart.Add(shoppingCartModel);
                }
                else
                {
                    //var result = existData.FirstOrDefault();
                    result.PurchaseQty += shoppingCartModel.PurchaseQty;
                }

                context.SaveChanges();
                return new BaseResponse<bool>(true, "add to shoppingcart success", default);

            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, ex.Message, default);
            }
        }

        public BaseResponse<List<ShoppingCartWrapper>> GetShoppingCart(string account)
        {
            try
            {
                var result = (from a in context.ShoppingCart
                              join b in context.Merchandise on a.ProdId equals b.MerchandiseId
                              join c in context.MerchandiseSpec on new { pid = a.ProdId, sid = a.SpecId } equals new { pid = c.MerchandiseId, sid = c.SpecId } into SubCart
                              from c in SubCart.DefaultIfEmpty()
                              where a.Account == account
                              select new ShoppingCartViewModel(b.MerchandiseTitle, b.OwnerAccount, b.MerchandiseId, (a.SpecId == 0) ? b.Price : c.Price, a.SpecId, c.Spec1, c.Spec2, a.PurchaseQty)).ToList();


                List<string> sellerAccount = new List<string>();
                foreach (var item in result)
                {
                    if (!sellerAccount.Contains(item.OwnerAccount))
                    {
                        sellerAccount.Add(item.OwnerAccount);
                    }
                }

                List<ShoppingCartWrapper> shoppingCartWrapper = new List<ShoppingCartWrapper>();
                ShoppingCartWrapper wrapper;
                foreach (var seller in sellerAccount)
                {
                    wrapper = new ShoppingCartWrapper()
                    {
                        SellerAccount = seller,
                        ShoppingCartViewModels = new List<ShoppingCartViewModel>(),
                    };
                    shoppingCartWrapper.Add(wrapper);
                }

                ShoppingCartViewModel temp;
                result.ForEach(resultData =>
                {
                    shoppingCartWrapper.ForEach(cartData =>
                    {
                        if (cartData.SellerAccount.Equals(resultData.OwnerAccount, StringComparison.InvariantCulture))
                        {
                            temp = new ShoppingCartViewModel(resultData.MerchandiseTitle, resultData.OwnerAccount, resultData.MerchandiseId, resultData.Price, resultData.SpecId, resultData.Spec1, resultData.Spec2, resultData.PurchaseQty);
                            cartData.ShoppingCartViewModels.Add(temp);
                        }
                    });
                });

                return new BaseResponse<List<ShoppingCartWrapper>>(true, "List of shoppingCart", shoppingCartWrapper);
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ShoppingCartWrapper>>(false, "Cannot get list of shoppingCart", null);
            }
        }

        public BaseResponse<bool> RemoveShoppingCartItem(List<RemoveShoppingCartItemRequestModel> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            try
            {
                foreach (var item in items)
                {
                    ShoppingCart temp = context.ShoppingCart.Where(data => data.Account == item.Account && data.ProdId == item.ProdId && data.SpecId == item.SpecId).FirstOrDefault();
                    context.ShoppingCart.Remove(temp);
                }
                context.SaveChanges();
                return new BaseResponse<bool>(true, "Remove Item Success!", true);
            }
            catch (Exception)
            {
                return new BaseResponse<bool>(false, "Remove Item failed!", false);
            }
        }


    }


}
