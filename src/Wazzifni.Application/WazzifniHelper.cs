using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Wazzifni;

public static class WazzifniHelper
{
    public static string GenereateVerficationCode()
    {
        Random generator = new Random();
        // return "123456";
        return generator.Next(0, 1000000).ToString("D6");

    }
    public static string FormatFileSize(double bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        int order = 0;

        while (bytes >= 1024 && order < sizes.Length - 1)
        {
            order++;
            bytes = bytes / 1024;
        }

        return $"{bytes:0.##} {sizes[order]}";
    }


    public static string GetLocalizedMD5(this object obj)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.None,
            Culture = CultureInfo.InvariantCulture
        };

        var json = JsonConvert.SerializeObject(obj, settings);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var md5Bytes = MD5.HashData(jsonBytes);
        return $"{Convert.ToBase64String(md5Bytes)}-{CultureInfo.CurrentUICulture.Name}";
    }

    public static string GetLocalizedId<TPrimaryKey>(this IEntityDto<TPrimaryKey> entity)
    {
        return $"{entity.Id}-{CultureInfo.CurrentUICulture.Name}";
    }

    public static string GetIpAddress(this IHttpContextAccessor httpContextAccessor)
    {
        if (!string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"]))
            return httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];

        return httpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    public static IQueryable<T> UnionIf<T>(this IQueryable<T> leftQuery, bool condition, IQueryable<T> rightQuery) =>
        condition ? leftQuery.Union(rightQuery) : leftQuery;
}