using System;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Arguments for <see cref="PushRequestDistributionJob"/>.
    /// </summary>
    [Serializable]
    public class PushRequestDistributionJobArgs
    {
        /// <summary>
        /// Push Request Id.
        /// </summary>
        public Guid PushRequestId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PushRequestDistributionJobArgs"/> class.
        /// </summary>
        public PushRequestDistributionJobArgs(Guid pushRequestId)
        {
            PushRequestId = pushRequestId;
        }
    }
}