using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ProductService.Api.Authorize;
internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return await Task.FromResult(policy.Build());
        }
        return await FallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => await FallbackPolicyProvider.GetDefaultPolicyAsync();
}