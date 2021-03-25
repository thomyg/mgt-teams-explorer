using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace mgt_teams_explorer.Auth
{
    public class MGTAuthenticationStateProvider : AuthenticationStateProvider
    {

        User CurrentUser;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        public void MarkUserAsAuthenticated(User currentUser)
        {
            CurrentUser = currentUser;

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, currentUser.Name),
                new Claim(ClaimTypes.Email, currentUser.Email),
                new Claim(ClaimTypes.Sid, currentUser.UserId)
            }, "apiauth_tpye");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public string GetAccessTokenFromCurrentUser()
        {
            if (CurrentUser is not null)
            {
                return CurrentUser.AccessToken;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
