using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbanana.AuthorizationRequirements
{
    public class KanbananaClaim : IAuthorizationRequirement
    {
        public KanbananaClaim(string claim)
        {
            ClaimType = claim;
        }

        public string ClaimType { get; }
    }

    public class KanbananaClaimHandler : AuthorizationHandler<KanbananaClaim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, KanbananaClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);
            if (hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtention
    {
        public static AuthorizationPolicyBuilder RequireKanbananaClaim(
            this AuthorizationPolicyBuilder builder,
            string claimTypes)
        {
            builder.AddRequirements(new KanbananaClaim(claimTypes));
            return builder;
        }
    }
}
