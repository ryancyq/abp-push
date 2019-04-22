using Microsoft.EntityFrameworkCore;
using Abp.Push.Devices;
using Abp.Push.Requests;

namespace Abp.Push.EntityFrameworkCore
{
    public interface IAbpPushDbContext
    {
        DbSet<PushDevice> PushDevices { get; set; }

        DbSet<PushRequest> PushRequests { get; set; }

        DbSet<PushRequestSubscription> PushRequestSubscriptions { get; set; }
    }
}