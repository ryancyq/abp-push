using System.Threading.Tasks;
using Abp.Push.Requests;

namespace Abp.Push.Providers
{
    /// <summary>
    /// Interface of push service provider
    /// </summary>
    public interface IPushServiceProvider
    {
        PushServiceProviderInfo ProviderInfo { get; }

        void Initialize(PushServiceProviderInfo providerInfo);

        /// <summary>
        /// This method tries to deliver a single push to specified users.
        /// If a user does not have any registered device, it should ignore him.
        /// </summary>
        Task PushAsync(IUserIdentifier[] userIdentifiers, PushRequest pushRequestInfo);
    }
}