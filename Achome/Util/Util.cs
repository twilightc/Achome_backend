using Achome.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Achome.Util
{
    public static class Util
    {
        public static string PasswordEncoding(string Pd)
        {
            Pd = Pd ?? "";
            using (SHA256 sha256 = new SHA256CryptoServiceProvider())//建立一個SHA256
            {
                byte[] source = System.Text.Encoding.Default.GetBytes(Pd);//將字串轉為Byte[]
                byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
                string result = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串
                return result;
            }
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> data, int pageIndex, int pageSize)
        {
            return data.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        }

        public static IQueryable<T> SortByOrder<T, T2>(this IQueryable<T> query, Expression<Func<T, T2>> keySelector, OrderTypeEnum orderTypeEnum)
        {
            switch (orderTypeEnum)
            {
                case OrderTypeEnum.Desc:
                    query = query.OrderByDescending(keySelector);
                    break;
                //case OrderTypeEnum.Asc:
                //    query = query.OrderBy(keySelector);
                //    break;
                default:
                    query = query.OrderBy(keySelector);
                    break;
            }
            return query;
        }
    }
}
