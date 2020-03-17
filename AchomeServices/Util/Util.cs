using AchomeModels.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AchomeModels.Util
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:必須驗證公用方法的引數", Justification = "<暫止>")]
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, bool Desc)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            if (property == null)
            {
                return source;
            }
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), $"OrderBy{(Desc ? "Descending" : string.Empty)}", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
        public static IQueryable<T> OrderByDesc<T>(this IQueryable<T> source, string ordering)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var OrderByDescExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(OrderByDescExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
