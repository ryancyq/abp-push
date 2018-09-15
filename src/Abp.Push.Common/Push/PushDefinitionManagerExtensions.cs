using System.Collections.Generic;
using Abp.Threading;

namespace Abp.Push
{
    /// <summary>
    /// Extension methods for <see cref="IPushDefinitionManager"/>.
    /// </summary>
    public static class PushDefinitionManagerExtensions
    {
        /// <summary>
        /// Gets all available push definitions for given user.
        /// </summary>
        /// <param name="pushDefinitionManager">Push definition manager</param>
        /// <param name="user">User</param>
        public static IReadOnlyList<PushDefinition> GetAllAvailable(this IPushDefinitionManager pushDefinitionManager, UserIdentifier user)
        {
            return AsyncHelper.RunSync(() => pushDefinitionManager.GetAllAvailableAsync(user));
        }
    }
}
