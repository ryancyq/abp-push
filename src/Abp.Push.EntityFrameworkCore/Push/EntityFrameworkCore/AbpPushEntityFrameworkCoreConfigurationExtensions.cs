using Microsoft.EntityFrameworkCore;
using Abp.Push.Devices;
using Abp.Push.Requests;

namespace Abp.Push.EntityFrameworkCore
{
    public static class AbpPushEntityFrameworkCoreConfigurationExtensions
    {
        public static void ConfigureAbpPushEntities(this ModelBuilder modelBuilder, string prefix = null, string schemaName = null)
        {
            prefix = prefix ?? "Abp";
            modelBuilder.Entity<PushDevice>(device =>
            {
                var tableName = prefix + "PushDevices";
                if (schemaName == null)
                {
                    device.ToTable(tableName);
                }
                else
                {
                    device.ToTable(tableName, schemaName);
                }

                device.HasIndex(e => new { e.TenantId, e.UserId });
                device.HasIndex(e => new { e.TenantId, e.DeviceIdentifier });
                device.HasIndex(e => new { e.TenantId, e.NormalizedDeviceName });
                device.HasIndex(e => new { e.TenantId, e.DevicePlatform });
                device.HasIndex(e => new { e.TenantId, e.ServiceProvider });
            });

            modelBuilder.Entity<PushRequest>(request =>
            {
                var tableName = prefix + "PushRequests";
                if (schemaName == null)
                {
                    request.ToTable(tableName);
                }
                else
                {
                    request.ToTable(tableName, schemaName);
                }

                request.HasIndex(e => new { e.Name });
                request.HasIndex(e => new { e.CreationTime });
                request.HasIndex(e => new { e.ExpirationTime });
            });

            modelBuilder.Entity<PushRequestSubscription>(subscription =>
            {
                var tableName = prefix + "PushRequestSubscriptions";
                if (schemaName == null)
                {
                    subscription.ToTable(tableName);
                }
                else
                {
                    subscription.ToTable(tableName, schemaName);
                }

                subscription.HasIndex(e => new { e.PushRequestName, e.EntityTypeName, e.EntityId, e.UserId });
                subscription.HasIndex(e => new { e.TenantId, e.PushRequestName, e.EntityTypeName, e.EntityId, e.UserId });
            });
        }
    }
}
