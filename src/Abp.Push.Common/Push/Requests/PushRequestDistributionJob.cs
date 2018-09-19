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
        private readonly IPushRequestDistributer _pushRequestDistributer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushRequestDistributionJob"/> class.
        /// </summary>
        public PushRequestDistributionJob(IPushRequestDistributer pushRequestDistributer)
        {
            _pushRequestDistributer = pushRequestDistributer;
        }

        public override void Execute(PushRequestDistributionJobArgs args)
        {
            AsyncHelper.RunSync(() => _pushRequestDistributer.DistributeAsync(args.PushRequestId));
        }
    }
}
