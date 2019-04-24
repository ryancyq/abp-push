using Microsoft.EntityFrameworkCore;
using Abp.Push.Devices;
using Abp.Push.Requests;

namespace Abp.Push.EntityFrameworkCore
{
    public interface IAbpPushDbContext<TDevice> where TDevice : AbpPushDevice, new()
    {
        DbSet<TDevice> PushDevices { get; set; }

        DbSet<PushRequest> PushRequests { get; set; }

        DbSet<PushRequestSubscription> PushRequestSubscriptions { get; set; }
    }
}