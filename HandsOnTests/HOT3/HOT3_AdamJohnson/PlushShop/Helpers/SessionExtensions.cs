﻿using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace PlushShop.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var s = session.GetString(key);
            if (s == null) return default;
            return JsonSerializer.Deserialize<T>(s);
        }
    }
}
