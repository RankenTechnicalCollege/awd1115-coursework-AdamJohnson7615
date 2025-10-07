using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace Project1_TripsLog.Helpers
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T? Get<T>(this ITempDataDictionary tempData, string key)
        {
            if (tempData.TryGetValue(key, out var obj) && obj is string json)
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            return default;
        }
    }
}