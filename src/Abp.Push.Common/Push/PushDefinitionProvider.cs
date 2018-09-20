using Abp.Dependency;

namespace Abp.Push
{
    /// <summary>
    /// This class should be implemented in order to define pushes.
    /// </summary>
    public abstract class PushDefinitionProvider : ITransientDependency
    {
        /// <summary>
        /// Used to add/manipulate push definitions.
        /// </summary>
        /// <param name="context">Context</param>
        public abstract void SetDefinitions(IPushDefinitionContext context);
    }
}