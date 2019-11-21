using Achome.DbModels;
using Achome.Models;
using Achome.Models.RequestModels;
using Achome.Models.ResponseModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Service.Implement
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IMapper mapper;
        public ShoppingCartService(IOptions<ApplicationSettings> appSettings, AChomeContext context, IMapper mapper, ILogger<MerchandiseService> logger)
        {
            logger.LogDebug("ShoppingCartService");
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.mapper = mapper;
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

                var result = context.ShoppingCart.Where(data => data.Account == shoppingCartModel.Account && data.ProdId == shoppingCartModel.ProdId && data.SpecId == shoppingCartModel.SpecId).FirstOrDefault();
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
            catch (Exception)
            {
                return new BaseResponse<List<ShoppingCartWrapper>>(false, "Cannot get list of shoppingCart", null);
            }
        }

        public BaseResponse<bool> RemoveShoppingCartItem(List<PendedShoppingCartItemRequestModel> items, string account)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            try
            {
                foreach (var item in items)
                {
                    ShoppingCart temp = context.ShoppingCart.Where(data => data.Account == account && data.ProdId == item.ProdId && data.SpecId == item.SpecId).FirstOrDefault();
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
