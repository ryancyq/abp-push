using System.Threading.Tasks;
using Abp.Push.Devices;
using Abp.Push.Requests;

namespace Abp.Push.Providers
{
    /// <summary>
    /// Interface of push service provider
    /// </summary>
    public interface IPushServiceProvider
    {
        ServiceProviderInfo ProviderInfo { get; }

        void Initialize(ServiceProviderInfo providerInfo);

        /// <summary>
        /// This method tries to deliver a single push to specified users.
        /// If a user does not have any registered device, it should ignore him.
        /// </summary>
        Task PushAsync(IUserIdentifier[] userIdentifiers, PushRequest pushRequest);

        /// <summary>
        /// This method tries to deliver a single push to specified devices.
        /// </summary>
        Task PushAsync(IDeviceIdentifier[] deviceIdentifiers, PushRequest pushRequest);
    }
}