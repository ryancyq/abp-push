using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Implements <see cref="IPushRequestStore"/> using repositories.
    /// </summary>
    public class AbpPersistentPushRequestStore : AbpServiceBase, IPushRequestStore, ITransientDependency
    {
        protected readonly IRepository<PushRequest, Guid> RequestRepository;
        protected readonly IRepository<PushRequestSubscription, Guid> SubscriptionRepository;

        public AbpPersistentPushRequestStore(
            IRepository<PushRequest, Guid> pushRequestRepository,
            IRepository<PushRequestSubscription, Guid> pushSubscriptionRepository
        )
        {
            RequestRepository = pushRequestRepository;
            SubscriptionRepository = pushSubscriptionRepository;
        }

        [UnitOfWork]
        public virtual async Task InsertSubscriptionAsync(PushRequestSubscription subscription)
        {
            using (UnitOfWorkManager.Current.SetTenantId(subscription.TenantId))
            {
                await SubscriptionRepository.InsertAsync(subscription);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteSubscriptionAsync(IUserIdentifier user, string pushRequestName, string entityTypeName, string entityId)
        {
            using (UnitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                await SubscriptionRepository.DeleteAsync(s =>
                                                         s.UserId == user.UserId &&
                                                         s.PushRequestName == pushRequestName &&
                                                         s.EntityTypeName == entityTypeName &&
                                                         s.EntityId == entityId
                                                         );
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(string pushRequestName, string entityTypeName, string entityId)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await SubscriptionRepository.GetAllListAsync(s =>
                                                                    s.PushRequestName == pushRequestName &&
                                                                    s.EntityTypeName == entityTypeName &&
                                                                    s.EntityId == entityId
                                                                    );
            }
        }

        [UnitOfWork]
        public virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int?[] tenantIds, string pushRequestName, string entityTypeName, string entityId)
        {
            var subscriptions = new List<PushRequestSubscription>();

            foreach (var tenantId in tenantIds)
            {
                subscriptions.AddRange(await GetSubscriptionsAsync(tenantId, pushRequestName, entityTypeName, entityId));
            }

            return subscriptions;
        }

        [UnitOfWork]
        public virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(IUserIdentifier user)
        {
            using (UnitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                return await SubscriptionRepository.GetAllListAsync(s => s.UserId == user.UserId);
            }
        }

        [UnitOfWork]
        protected virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int? tenantId, string pushRequestName, string entityTypeName, string entityId)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return await SubscriptionRepository.GetAllListAsync(s =>
                                                                    s.PushRequestName == pushRequestName &&
                                                                    s.EntityTypeName == entityTypeName &&
                                                                    s.EntityId == entityId
                                                                );
            }
        }

        [UnitOfWork]
        public virtual async Task<bool> IsSubscribedAsync(IUserIdentifier user, string pushRequestName, string entityTypeName, string entityId)
        {
            using (UnitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                return await SubscriptionRepository.CountAsync(s =>
                                                               s.UserId == user.UserId &&
                                                               s.PushRequestName == pushRequestName &&
                                                               s.EntityTypeName == entityTypeName &&
                                                               s.EntityId == entityId
                                                               ) > 0;
            }
        }

        public virtual async Task DeleteRequestAsync(Guid requestId)
        {
            throw new NotImplementedException();
        }

        public virtual async Task InsertRequestAsync(PushRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<PushRequest> GetRequestOrNullAsync(Guid requestId)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<List<PushRequest>> GetRequestsAsync(IUserIdentifier user, PushRequestPriority? priority = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> GethRequestCountAsync(IUserIdentifier user, PushRequestPriority? priority = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task UpdateAllRequestPrioritiesAsync(IUserIdentifier user, PushRequestPriority priority)
        {
            throw new NotImplementedException();
        }

        public virtual async Task UpdateRequestPriorityAsync(int? tenantId, Guid pushRequestId, PushRequestPriority priority)
        {
            throw new NotImplementedException();
        }
    }
}