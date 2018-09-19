using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Json;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Used to store a push request subscription.
    /// </summary>
    [Table("AbpPushRequestSubscriptions")]
    public class PushRequestSubscription : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// Tenant id of the subscribed user.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Push request unique name.
        /// </summary>
        [MaxLength(PushRequest.MaxNameLength)]
        public virtual string PushRequestName { get; set; }

        /// <summary>
        /// Gets/sets entity type name, if this is an entity level push request.
        /// It's FullName of the entity type.
        /// </summary>
        [MaxLength(PushRequest.MaxEntityTypeNameLength)]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// AssemblyQualifiedName of the entity type.
        /// </summary>
        [MaxLength(PushRequest.MaxEntityTypeAssemblyQualifiedNameLength)]
        public virtual string EntityTypeAssemblyQualifiedName { get; set; }

        /// <summary>
        /// Gets/sets primary key of the entity, if this is an entity level push request.
        /// </summary>
        [MaxLength(PushRequest.MaxEntityIdLength)]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PushRequestSubscription"/> class.
        /// </summary>
        public PushRequestSubscription()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PushRequestSubscription"/> class.
        /// </summary>
        public PushRequestSubscription(Guid id, int? tenantId, long userId, string pushRequestName, EntityIdentifier entityIdentifier = null)
        {
            Id = id;
            TenantId = tenantId;
            PushRequestName = pushRequestName;
            UserId = userId;
            EntityTypeName = entityIdentifier == null ? null : entityIdentifier.Type.FullName;
            EntityTypeAssemblyQualifiedName = entityIdentifier == null ? null : entityIdentifier.Type.AssemblyQualifiedName;
            EntityId = entityIdentifier == null ? null : entityIdentifier.Id.ToJsonString();
        }
    }
}