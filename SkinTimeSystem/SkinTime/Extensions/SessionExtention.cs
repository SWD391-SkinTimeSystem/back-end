using System.Text.Json;

namespace SkinTime.Extensions
{
    public static class SessionExtention
    {
        public static IServiceCollection AddSessionService(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(opt =>
            {
                //opt.IdleTimeout = TimeSpan.FromSeconds(10);
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
            });
            return services;
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
