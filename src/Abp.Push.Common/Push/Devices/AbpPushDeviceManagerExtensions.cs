using Abp.Threading;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Extension methods for <see cref="AbpPushDeviceManager{TDevice}"/>.
    /// </summary>
    public static class AbpPushDeviceManagerExtensions
    {
        /// <summary>
        /// Determines whether the specified user has device to be pushed to.
        /// </summary>
        /// <param name="manager">The push device manager.</param>
        /// <param name="user">The user.</param>
        public static bool CanPush<TDevice>(this AbpPushDeviceManager<TDevice> manager, IUserIdentifier user)
            where TDevice : AbpPushDevice
        {
            return AsyncHelper.RunSync(() => manager.GetCountByUserAsync(user)) > 0;
        }
    }
}