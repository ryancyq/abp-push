using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;

namespace Abp.Push.Requests
{
    /// <summary>
    /// Used to store a push request.
    /// This push request is distributed to tenants and users by <see cref="IPushRequestDistributer"/>.
    /// </summary>
    [Serializable]
    [Table("AbpPushRequests")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class PushRequest : CreationAuditedEntity<Guid>
    {
        /// <summary>
        /// Indicates all tenant ids for <see cref="TenantIds"/> property.
        /// Value: "0".
        /// </summary>
        public const string AllTenantIds = "0";

        /// <summary>
        /// Maximum length of <see cref="Name"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxNameLength = 512;

        /// <summary>
        /// Maximum length of <see cref="Data"/> property.
        /// Value: 4,194,304 (4 MB).
        /// </summary>
        public const int MaxDataLength = 4 * 1024 * 1024;

        /// <summary>
        /// Maximum length of <see cref="DataTypeName"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxDataTypeNameLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityTypeName"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxEntityTypeNameLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityTypeAssemblyQualifiedName"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxEntityTypeAssemblyQualifiedNameLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityId"/> property.
        /// Value: 256.
        /// </summary>
        public const int MaxEntityIdLength = 256;

        /// <summary>
        /// Maximum length of <see cref="UserIds"/> property.
        /// Value: 2,097,152 (2 MB).
        /// </summary>
        public const int MaxUserIdsLength = 2 * 1024 * 1024;

        /// <summary>
        /// Maximum length of <see cref="TenantIds"/> property.
        /// Value: 1,048,576 (1 MB).
        /// </summary>
        public const int MaxTenantIdsLength = 1024 * 1024;

        /// <summary>
        /// Maximum length of <see cref="LastExecutionResult"/> property.
        /// Value: 4,194,304 (4 MB).
        /// </summary>
        public const int MaxLastExecutionResultLength = 4 * 1024 * 1024;

        /// <summary>
        /// Unique push request name.
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Request data as JSON string.
        /// </summary>
        [MaxLength(MaxDataLength)]
        public virtual string Data { get; set; }

        /// <summary>
        /// Type of the JSON serialized <see cref="Data"/>.
        /// It's AssemblyQualifiedName of the type.
        /// </summary>
        [MaxLength(MaxDataTypeNameLength)]
        public virtual string DataTypeName { get; set; }

        /// <summary>
        /// Gets/sets entity type name, if this is an entity level push request.
        /// It's FullName of the entity type.
        /// </summary>
        [MaxLength(MaxEntityTypeNameLength)]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// AssemblyQualifiedName of the entity type.
        /// </summary>
        [MaxLength(MaxEntityTypeAssemblyQualifiedNameLength)]
        public virtual string EntityTypeAssemblyQualifiedName { get; set; }

        /// <summary>
        /// Gets/sets primary key of the entity, if this is an entity level push request.
        /// </summary>
        [MaxLength(MaxEntityIdLength)]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// Target users of the request.
        /// If this is set, it overrides subscribed users.
        /// If this is null/empty, then push request is sent to all subscribed users.
        /// </summary>
        [MaxLength(MaxUserIdsLength)]
        public virtual string UserIds { get; set; }

        /// <summary>
        /// Excluded users.
        /// This can be set to exclude some users while publishing requests to subscribed users.
        /// It's not normally used if <see cref="UserIds"/> is not null.
        /// </summary>
        [MaxLength(MaxUserIdsLength)]
        public virtual string ExcludedUserIds { get; set; }

        /// <summary>
        /// Target tenants of the request.
        /// Used to send request to subscribed users of specific tenant(s).
        /// This is valid only if UserIds is null.
        /// If it's "0", then indicates to all tenants.
        /// </summary>
        [MaxLength(MaxTenantIdsLength)]
        public virtual string TenantIds { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public virtual PushRequestPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the expiration time.
        /// </summary>
        /// <value>The expiration time.</value>
        public virtual DateTime? ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets the max failed count.
        /// </summary>
        /// <value>The max failed count.</value>
        public virtual int? MaxFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the failed count.
        /// </summary>
        /// <value>The failed count.</value>
        public virtual int FailedCount { get; set; }

        /// <summary>
        /// Gets or sets the last execution time.
        /// </summary>
        /// <value>The last execution time.</value>
        public virtual DateTime? LastExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the last execution result.
        /// </summary>
        /// <value>The last execution result.</value>
        [MaxLength(MaxLastExecutionResultLength)]
        public virtual string LastExecutionResult { get; set; }

        public PushRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Priority"/> class.
        /// </summary>
        public PushRequest(Guid id)
        {
            Id = id;
            Priority = PushRequestPriority.Normal;
        }
    }
}