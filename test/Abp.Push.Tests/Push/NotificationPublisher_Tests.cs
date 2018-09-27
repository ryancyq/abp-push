using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Domain.Uow;
using Abp.Push.Configurations;
using Abp.Push.Requests;
using NSubstitute;
using Xunit;

namespace Abp.Tests.Push
{
    public class PushRequestPublisher_Tests : TestBaseWithLocalIocManager
    {
        private readonly IPushRequestStore _store;
        private readonly IBackgroundJobManager _backgroundJobManager; 
        private readonly IPushRequestDistributor _distributor;
        private readonly IPushConfiguration _configuration;
        private readonly IGuidGenerator _generator;
        private readonly AbpPushRequestPublisher _publisher;

        public PushRequestPublisher_Tests()
        {
            _store = Substitute.For<IPushRequestStore>();
            _backgroundJobManager = Substitute.For<IBackgroundJobManager>();
            _distributor = Substitute.For<IPushRequestDistributor>();
            _configuration = Substitute.For<IPushConfiguration>();
            _generator = Substitute.For<IGuidGenerator>();
            _publisher = new AbpPushRequestPublisher(
                _store,
                _backgroundJobManager,
                _distributor,
                _configuration,
                _generator
            );
            _publisher.UnitOfWorkManager = Substitute.For<IUnitOfWorkManager>();
            _publisher.UnitOfWorkManager.Current.Returns(Substitute.For<IActiveUnitOfWork>());
        }

        [Fact]
        public async Task Should_Publish_General_Push_Request()
        {
            //Arrange
            var pushRequestData = CreatePushRequestData();

            //Act
            await _publisher.PublishAsync("TestPushRequest", pushRequestData, priority: PushRequestPriority.AboveNormal);

            //Assert
            await _store.Received()
                        .InsertRequestAsync(
                            Arg.Is<PushRequest>(n =>
                                                n.Name == "TestPushRequest" &&
                                                n.Priority == PushRequestPriority.AboveNormal &&
                                                n.DataTypeName == pushRequestData.GetType().AssemblyQualifiedName &&
                                                n.Data.Contains("42")
                               )
                           );

            await _backgroundJobManager.Received()
                                       .EnqueueAsync<PushRequestDistributionJob, PushRequestDistributionJobArgs>(
                                           Arg.Any<PushRequestDistributionJobArgs>()
                                          );
        }

        private static PushRequestData CreatePushRequestData()
        {
            var pushRequestData = new PushRequestData();
            pushRequestData["TestValue"] = 42;
            return pushRequestData;
        }
    }
}
