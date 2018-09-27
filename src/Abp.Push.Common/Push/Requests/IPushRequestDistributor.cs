using System;
using System.Threading.Tasks;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Used to distribute push requests.
    /// </summary>
    public interface IPushRequestDistributor
    {
        /// <summary>
        /// Distributes given push request.
        /// </summary>
        /// <param name="pushRequestId">The push request id.</param>
        Task DistributeAsync(Guid pushRequestId);
    }
}