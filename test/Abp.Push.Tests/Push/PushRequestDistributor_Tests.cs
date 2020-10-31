using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Push;
using Abp.Push.Configuration;
using Abp.Push.Providers;
using Abp.Push.Requests;
using NSubstitute;
using Xunit;

namespace Abp.Tests.Push
{
    public class PushRequestDistributor_Tests : TestBaseWithLocalIocManager
    {
        private readonly IPushRequestStore _store;
        private readonly IPushDefinitionManager _definitionManager;
        private readonly IAbpPushConfiguration _configuration;
        private readonly IIocResolver _iocResolver;
        private readonly IGuidGenerator _generator;
        private readonly AbpPushRequestDistributor _distributor;

        public PushRequestDistributor_Tests()
        {
            _store = Substitute.For<IPushRequestStore>();
            _definitionManager = Substitute.For<IPushDefinitionManager>();
            _configuration = Substitute.For<IAbpPushConfiguration>();
            _configuration.ServiceProviders.Returns(new List<ServiceProviderInfo>());
            _iocResolver = Substitute.For<IIocResolver>();
            _generator = Substitute.For<IGuidGenerator>();
            _distributor = new AbpPushRequestDistributor(
                _store,
                _definitionManager,
                _configuration,
                _iocResolver,
                _generator
            );
            _distributor.UnitOfWorkManager = Substitute.For<IUnitOfWorkManager>();
            _distributor.UnitOfWorkManager.Current.Returns(Substitute.For<IActiveUnitOfWork>());

            var _settingManager = Substitute.For<ISettingManager>();
            _settingManager.GetSettingValueForUser(Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<long>())
                           .Returns("true");
            _distributor.SettingManager = _settingManager;
        }

        [Fact]
        public async Task Should_Distribute_Push_Request()
        {
            //Arrange
            var guid = _generator.Create();
            _store.GetRequestOrNullAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(CreatePushRequest());
            _store.GetSubscriptionsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                  .ReturnsForAnyArgs(new List<PushRequestSubscription> { CreatePushRequestSubscription() });

            //Act
            await _distributor.DistributeAsync(guid);

            //Assert
            await _store.Received().GetRequestOrNullAsync(Arg.Any<Guid>());
            await _store.Received().DeleteRequestAsync(Arg.Is<Guid>(n => n.Equals(guid)));
        }

        private static PushRequest CreatePushRequest()
        {
            return new PushRequest
            {
                UserIds = "1,2,3"
            };
        }

        private static PushRequestSubscription CreatePushRequestSubscription()
        {
            return new PushRequestSubscription();
        }
    }
}
