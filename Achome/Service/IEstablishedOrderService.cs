using Achome.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Service
{
    public interface IEstablishedOrderService
    {
        Task<EstablishOrderResponse> GetEstablishOrderList(string account);


    }
}
