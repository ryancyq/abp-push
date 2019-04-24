using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Push.Configuration;

namespace Abp.Push
{
    /// <summary>
    /// Implements <see cref="IPushDefinitionManager"/>.
    /// </summary>
    public class PushDefinitionManager : AbpServiceBase, IPushDefinitionManager, ISingletonDependency
    {
        protected readonly IAbpPushConfiguration Configuration;
        protected readonly IIocResolver IocResolver;

        private readonly IDictionary<string, PushDefinition> _pushDefinitions;

        public PushDefinitionManager(
            IIocResolver iocResolver,
            IAbpPushConfiguration configuration)
        {
            Configuration = configuration;
            IocResolver = iocResolver;

            _pushDefinitions = new Dictionary<string, PushDefinition>();
        }

        public virtual void Initialize()
        {
            var context = new PushDefinitionContext(this);

            foreach (var providerType in Configuration.Providers)
            {
                using (var provider = IocResolver.ResolveAsDisposable<PushDefinitionProvider>(providerType))
                {
                    provider.Object.SetDefinitions(context);
                }
            }
        }

        public virtual void Add(PushDefinition pushDefinition)
        {
            if (_pushDefinitions.ContainsKey(pushDefinition.Name))
            {
                throw new AbpInitializationException("There is already a push definition with given name: " + pushDefinition.Name + ". Push names must be unique!");
            }

            _pushDefinitions[pushDefinition.Name] = pushDefinition;
        }

        public virtual PushDefinition Get(string name)
        {
            var definition = GetOrNull(name);
            if (definition == null)
            {
                throw new AbpException("There is no push definition with given name: " + name);
            }

            return definition;
        }

        public virtual PushDefinition GetOrNull(string name)
        {
            return _pushDefinitions.GetOrDefault(name);
        }

        public virtual IReadOnlyList<PushDefinition> GetAll()
        {
            return _pushDefinitions.Values.ToImmutableList();
        }

        public virtual async Task<bool> IsAvailableAsync(string name, IUserIdentifier user)
        {
            var pushDefinition = GetOrNull(name);
            if (pushDefinition == null)
            {
                return true;
            }

            if (pushDefinition.FeatureDependency != null)
            {
                using (var featureDependencyContext = IocResolver.ResolveAsDisposable<FeatureDependencyContext>())
                {
                    featureDependencyContext.Object.TenantId = user.TenantId;

                    if (!await pushDefinition.FeatureDependency.IsSatisfiedAsync(featureDependencyContext.Object))
                    {
                        return false;
                    }
                }
            }

            if (pushDefinition.PermissionDependency != null)
            {
                using (var permissionDependencyContext = IocResolver.ResolveAsDisposable<PermissionDependencyContext>())
                {
                    permissionDependencyContext.Object.User = user.ToUserIdentifier();

                    if (!await pushDefinition.PermissionDependency.IsSatisfiedAsync(permissionDependencyContext.Object))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public virtual async Task<IReadOnlyList<PushDefinition>> GetAllAvailableAsync(IUserIdentifier user)
        {
            var availableDefinitions = new List<PushDefinition>();

            using (var permissionDependencyContext = IocResolver.ResolveAsDisposable<PermissionDependencyContext>())
            {
                permissionDependencyContext.Object.User = user.ToUserIdentifier();

                using (var featureDependencyContext = IocResolver.ResolveAsDisposable<FeatureDependencyContext>())
                {
                    featureDependencyContext.Object.TenantId = user.TenantId;

                    foreach (var pushDefinition in GetAll())
                    {
                        if (pushDefinition.PermissionDependency != null &&
                            !await pushDefinition.PermissionDependency.IsSatisfiedAsync(permissionDependencyContext.Object))
                        {
                            continue;
                        }

                        if (user.TenantId.HasValue &&
                            pushDefinition.FeatureDependency != null &&
                            !await pushDefinition.FeatureDependency.IsSatisfiedAsync(featureDependencyContext.Object))
                        {
                            continue;
                        }

                        availableDefinitions.Add(pushDefinition);
                    }
                }
            }

            return availableDefinitions.ToImmutableList();
        }
    }
}