﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Models.ResponseModels
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            Success = false;
            Msg = "";
            Data = default;
        }
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
