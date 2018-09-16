using System.Linq;

namespace Abp.Push.Devices
{
    /// <summary>
    /// Extension methods for <see cref="PushDeviceManager{TDevice}"/>.
    /// </summary>
    public static class PushDeviceManagerExtensions
    {
        /// <summary>
        /// Determines whether the specified user is available for push or not.
        /// </summary>
        /// <param name="pushDeviceManager">The online device manager.</param>
        /// <param name="user">User.</param>
        public static bool CanPush(
            this IPushDeviceManager pushDeviceManager,
            [NotNull] UserIdentifier user)
        {
            Check.NotNull(user, nameof(user));

            return pushDeviceManager.GetAllByUserId(user).Any();
        }

        public static bool Remove(
            this PushDeviceManager pushDeviceManager,
            [NotNull] IPushDevice device)
        {
            Check.NotNull(device, nameof(device));

            return pushDeviceManager.Remove(device.Provider, device.ProviderKey);
        }
    }
}