using Abp.Push.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Push.EntityFrameworkCore
{
    public static class AbpPushEntityFrameworkCoreSeriviceCollectionExtensions
    {
        public static void AddAbpPersistedPushRequests(this IServiceCollection services)
        {
            services.AddTransient<IPushRequestStore, AbpPersistentPushRequestStore>();
        }
    }
}
