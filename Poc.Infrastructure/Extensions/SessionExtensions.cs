using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;  // JsonConvert ke liye zaroori

namespace Poc.Infrastructure.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            // object ko JSON string me convert karke session me save karo
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            // agar value null ho to default return ho jaye warna object deserialize karo
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
