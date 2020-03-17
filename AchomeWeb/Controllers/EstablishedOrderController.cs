
using AchomeModels.DbModels;
using AchomeModels.Models;
using AchomeModels.Models.ResponseModels;
using AchomeModels.Service;
using AchomeModels.Service.Implement;
using AchomeModels.Util;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablishedOrderController : ControllerBase
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IEstablishedOrderService establishedOrderService;
        private readonly IMapper mapper;
        public EstablishedOrderController(IOptions<ApplicationSettings> appSettings, AChomeContext context, IMapper mapper,
            IEstablishedOrderService establishedOrderService, ILogger<EstablishedOrderService> logger)
        {
            logger.LogInformation("EstablishOrderController");
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.establishedOrderService = establishedOrderService;
            this.mapper = mapper;
        }


        [Authorize]
        [HttpGet("[action]")]
        public async Task<EstablishOrderResponse> GetEstablishOrderList()
        {
            //var account = "a123";
            var account = User.Claims.Where(c => c.Type.Equals(ClaimString.AccountName, StringComparison.InvariantCulture)).FirstOrDefault().Value;
            return await establishedOrderService.GetEstablishOrderList(account).ConfigureAwait(false);
        }

    }
}
