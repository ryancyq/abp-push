using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using Abp.Linq.Extensions;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Implements <see cref="IPushRequestStore"/> using repositories.
    /// </summary>
    public class AbpPersistentPushRequestStore : AbpServiceBase, IPushRequestStore
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        protected readonly IRepository<PushRequest, Guid> RequestRepository;
        protected readonly IRepository<PushRequestSubscription, Guid> SubscriptionRepository;

        public AbpPersistentPushRequestStore(
            IRepository<PushRequest, Guid> pushRequestRepository,
            IRepository<PushRequestSubscription, Guid> pushSubscriptionRepository
        )
        {
            RequestRepository = pushRequestRepository;
            SubscriptionRepository = pushSubscriptionRepository;

            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        #region Subscriptions

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
        public virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(string pushRequestName, string entityTypeName, string entityId, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var query = SubscriptionRepository.GetAll()
                                                  .Where(s =>
                                                         s.PushRequestName == pushRequestName &&
                                                         s.EntityTypeName == entityTypeName &&
                                                         s.EntityId == entityId
                                                         );
                query = query.PageBy(skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int?[] tenantIds, string pushRequestName, string entityTypeName, string entityId, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            var tenantIdsList = new List<int?>();
            if (tenantIds != null && tenantIds.Length > 0)
            {
                tenantIdsList.AddRange(tenantIds);
            }

            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var query = SubscriptionRepository.GetAll()
                                                  .Where(s =>
                                                         tenantIdsList.Contains(s.TenantId) &&
                                                         s.PushRequestName == pushRequestName &&
                                                         s.EntityTypeName == entityTypeName &&
                                                         s.EntityId == entityId
                                                         );
                query = query.PageBy(skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(IUserIdentifier user, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            using (UnitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                var query = SubscriptionRepository.GetAll()
                                                  .Where(s => s.UserId == user.UserId);
                query = query.PageBy(skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        [UnitOfWork]
        protected virtual async Task<List<PushRequestSubscription>> GetSubscriptionsAsync(int? tenantId, string pushRequestName, string entityTypeName, string entityId, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var query = SubscriptionRepository.GetAll()
                                                  .Where(s =>
                                                         s.PushRequestName == pushRequestName &&
                                                         s.EntityTypeName == entityTypeName &&
                                                         s.EntityId == entityId
                                                        );
                query = query.PageBy(skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
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

        #endregion

        [UnitOfWork]
        public virtual async Task DeleteRequestAsync(Guid requestId)
        {
            // push request only defined on Host side
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                await RequestRepository.DeleteAsync(requestId);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task InsertRequestAsync(PushRequest request)
        {
            // push request only defined on Host side
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                await RequestRepository.DeleteAsync(request);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task<PushRequest> GetRequestOrNullAsync(Guid requestId)
        {
            // push request only defined on Host side
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                return await RequestRepository.FirstOrDefaultAsync(requestId);
            }
        }

        public virtual async Task<List<PushRequest>> GetRequestsAsync(PushRequestPriority? priority = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            // push request only defined on Host side
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                var query = RequestRepository.GetAll()
                                             .WhereIf(priority.HasValue, pr => pr.Priority == priority.Value);
                query = query.PageBy(skipCount, maxResultCount);

                return await AsyncQueryableExecuter.ToListAsync(query);
            }
        }

        public virtual async Task<int> GethRequestCountAsync(PushRequestPriority? priority = null)
        {
            // push request only defined on Host side
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                var query = RequestRepository.GetAll()
                                             .WhereIf(priority.HasValue, pr => pr.Priority == priority.Value);

                return await AsyncQueryableExecuter.CountAsync(query);
            }
        }

        [UnitOfWork]
        public virtual async Task UpdateRequestPriorityAsync(Guid pushRequestId, PushRequestPriority priority)
        {
            // push request only defined on Host side
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                var pushRequest = await RequestRepository.FirstOrDefaultAsync(pushRequestId);
                if (pushRequest == null)
                {
                    return;
                }

                pushRequest.Priority = priority;
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
        }
    }
}