namespace Abp.Push
{
    internal class PushDefinitionContext : IPushDefinitionContext
    {
        public IPushDefinitionManager Manager { get; private set; }

        public PushDefinitionContext(IPushDefinitionManager manager)
        {
            Manager = manager;
        }
    }
}
