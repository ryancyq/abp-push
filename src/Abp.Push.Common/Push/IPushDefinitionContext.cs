namespace Abp.Push
{
    /// <summary>
    /// Used as a context while defining pushes.
    /// </summary>
    public interface IPushDefinitionContext
    {
        /// <summary>
        /// Gets the push definition manager.
        /// </summary>
        IPushDefinitionManager Manager { get; }
    }
}
