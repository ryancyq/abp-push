using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Threading;

namespace Abp.Push.Requests
{
    /// <summary>
    /// This background job distributes push requests to users.
    /// </summary>
    public class PushRequestDistributionJob : BackgroundJob<PushRequestDistributionJobArgs>, ITransientDependency
    {
        private readonly IPushRequestDistributor _pushRequestDistributor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushRequestDistributionJob"/> class.
        /// </summary>
        public PushRequestDistributionJob(IPushRequestDistributor pushRequestDistributer)
        {
            _pushRequestDistributor = pushRequestDistributer;
        }

        public override void Execute(PushRequestDistributionJobArgs args)
        {
            AsyncHelper.RunSync(() => _pushRequestDistributor.DistributeAsync(args.PushRequestId));
        }
    }
}
