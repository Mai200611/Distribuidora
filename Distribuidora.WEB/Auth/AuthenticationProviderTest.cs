using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Distribuidora.WEB.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonymous = new ClaimsIdentity();
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
        }
    }
}