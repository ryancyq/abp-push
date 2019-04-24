using Abp.Threading;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Extension methods for <see cref="AbpPushDeviceManager{TDevice}"/>.
    /// </summary>
    public static class AbpPushDeviceManagerExtensions
    {
        /// <summary>
        /// Determines whether the specified user has any devices can be pushed to.
        /// </summary>
        /// <param name="manager">The push device manager.</param>
        /// <param name="user">The user.</param>
        public static bool HasAny<TDevice>(this AbpPushDeviceManager<TDevice> manager, IUserIdentifier user)
            where TDevice : AbpPushDevice, new()
        {
            return AsyncHelper.RunSync(() => manager.GetCountByUserAsync(user)) > 0;
        }
    }
}