using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Models.ResponseModels
{
    public class BaseResponse<T>
    {
        public BaseResponse(bool success, string msg, T data)
        {
            Success = success;
            Msg = msg;
            Data = data;
        }

        public bool Success { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
    }
}
