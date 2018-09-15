using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Push.Configurations;
using Abp.Push.Providers;

namespace Abp.Push
{
    /// <summary>
    /// Implements <see cref="IPushDefinitionManager"/>.
    /// </summary>
    internal class PushDefinitionManager : IPushDefinitionManager, ISingletonDependency
    {
        private readonly IPushConfiguration _configuration;
        private readonly IocManager _iocManager;

        private readonly IDictionary<string, PushDefinition> _pushDefinitions;

        public PushDefinitionManager(
            IocManager iocManager,
            IPushConfiguration configuration)
        {
            _configuration = configuration;
            _iocManager = iocManager;

            _pushDefinitions = new Dictionary<string, PushDefinition>();
        }

        public void Initialize()
        {
            var context = new PushDefinitionContext(this);

            foreach (var providerType in _configuration.Providers)
            {
                using (var provider = _iocManager.ResolveAsDisposable<PushDefinitionProvider>(providerType))
                {
                    provider.Object.SetDefinitions(context);
                }
            }
        }

        public void Add(PushDefinition pushDefinition)
        {
            if (_pushDefinitions.ContainsKey(pushDefinition.Name))
            {
                throw new AbpInitializationException("There is already a push definition with given name: " + pushDefinition.Name + ". Push names must be unique!");
            }

            _pushDefinitions[pushDefinition.Name] = pushDefinition;
        }

        public PushDefinition Get(string name)
        {
            var definition = GetOrNull(name);
            if (definition == null)
            {
                throw new AbpException("There is no push definition with given name: " + name);
            }

            return definition;
        }

        public PushDefinition GetOrNull(string name)
        {
            return _pushDefinitions.GetOrDefault(name);
        }

        public IReadOnlyList<PushDefinition> GetAll()
        {
            return _pushDefinitions.Values.ToImmutableList();
        }

        public async Task<bool> IsAvailableAsync(string name, UserIdentifier user)
        {
            var pushDefinition = GetOrNull(name);
            if (pushDefinition == null)
            {
                return true;
            }

            if (pushDefinition.FeatureDependency != null)
            {
                using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
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
                using (var permissionDependencyContext = _iocManager.ResolveAsDisposable<PermissionDependencyContext>())
                {
                    permissionDependencyContext.Object.User = user;

                    if (!await pushDefinition.PermissionDependency.IsSatisfiedAsync(permissionDependencyContext.Object))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<IReadOnlyList<PushDefinition>> GetAllAvailableAsync(UserIdentifier user)
        {
            var availableDefinitions = new List<PushDefinition>();

            using (var permissionDependencyContext = _iocManager.ResolveAsDisposable<PermissionDependencyContext>())
            {
                permissionDependencyContext.Object.User = user;

                using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
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