using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Push
{
    public interface IPushDefinitionManager : IPushDefinitionManager<PushDefinition>
    {
    }

    /// <summary>
    /// Used to manage push definitions.
    /// </summary>
    public interface IPushDefinitionManager<TDefinition>
        where TDefinition : IPushDefinition
    {
        /// <summary>
        /// Adds the specified push definition.
        /// </summary>
        void Add(TDefinition pushDefinition);

        /// <summary>
        /// Gets a push definition by name.
        /// Throws exception if there is no push definition with given name.
        /// </summary>
        TDefinition Get(string name);

        /// <summary>
        /// Gets a push definition by name.
        /// Returns null if there is no push definition with given name.
        /// </summary>
        TDefinition GetOrNull(string name);

        /// <summary>
        /// Gets all push definitions.
        /// </summary>
        IReadOnlyList<TDefinition> GetAll();

        /// <summary>
        /// Checks if given push definition by name is available for given user.
        /// </summary>
        /// <param name="name">Definition Name.</param>
        /// <param name="user">User.</param>
        Task<bool> IsAvailableAsync(string name, IUserIdentifier user);

        /// <summary>
        /// Gets all available push definitions for given user.
        /// </summary>
        /// <param name="user">User.</param>
        Task<IReadOnlyList<TDefinition>> GetAllAvailableAsync(IUserIdentifier user);
    }
}