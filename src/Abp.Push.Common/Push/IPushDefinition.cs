using System;
using System.Collections.Generic;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Localization;

namespace Abp.Push
{
    /// <summary>
    /// Interface of a push definition
    /// </summary>
    public interface IPushDefinition
    {
        /// <summary>
        /// Unique name of the push.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Related entity type with this push (optional).
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// Display name of the push.
        /// Optional.
        /// </summary>
        ILocalizableString DisplayName { get; }

        /// <summary>
        /// Description for the push.
        /// Optional.
        /// </summary>
        ILocalizableString Description { get; }

        /// <summary>
        /// A permission dependency. This push will be available to a user if this dependency is satisfied.
        /// Optional.
        /// </summary>
        IPermissionDependency PermissionDependency { get; }

        /// <summary>
        /// A feature dependency. This push will be available to a tenant if this feature is enabled.
        /// Optional.
        /// </summary>
        IFeatureDependency FeatureDependency { get; }

        /// <summary>
        /// Gets/sets arbitrary objects related to this object.
        /// Gets null if given key does not exists.
        /// This is a shortcut for <see cref="Attributes"/> dictionary.
        /// </summary>
        /// <param name="key">Key</param>
        object this[string key] { get; set; }

        /// <summary>
        /// Arbitrary objects related to this object.
        /// These objects must be serializable.
        /// </summary>
        IDictionary<string, object> Attributes { get; }
    }
}
