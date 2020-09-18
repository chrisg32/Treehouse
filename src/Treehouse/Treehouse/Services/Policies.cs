using Microsoft.AspNetCore.Authorization;

namespace TreeHouse.Services
{
    public static class Policies
    {
        private const string AuthScheme = "Bearer";
        public const string IsAdmin = "IsAdmin";

        public static void Configure(AuthorizationOptions options)
        {
            options.DefaultPolicy = DefaultPolicy();
            options.AddPolicy(IsAdmin, IsAdminPolicy());
        }

        private static AuthorizationPolicy DefaultPolicy()
        {
            return new AuthorizationPolicyBuilder(AuthScheme).RequireAuthenticatedUser()
                .RequireClaim(Claims.UserId)
                .Build();
        }
        private static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder(AuthScheme).RequireAuthenticatedUser()
                .RequireClaim(Claims.UserId)
                .RequireClaim(Claims.IsParent, "true", "True")
                .Build();
        }

    }
}
