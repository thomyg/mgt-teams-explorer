extern alias BetaLib;
using Beta = BetaLib.Microsoft.Graph;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.JSInterop;
using mgt_teams_explorer.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace mgt_teams_explorer.Data
{
    public class MyGraphService
    {
        private  string _accessToken;

        [Inject]
        private IJSRuntime js { get; set; }
        
        private MGTAuthenticationStateProvider AuthProvider { get; set; }

        public MyGraphService(AuthenticationStateProvider authProvider)
        {
            AuthProvider = (MGTAuthenticationStateProvider)authProvider;
        }

        public async Task<List<Team>> GetTeams()
        {            
            var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) => {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("Bearer", AuthProvider.GetAccessTokenFromCurrentUser());

                return Task.FromResult(0);
            }));

            var teams = await graphServiceClient
                .Me
                .JoinedTeams
                .Request()
                .GetAsync();
            
            List<Team> result = (List<Team>)teams.CurrentPage;
            result.Sort((a,b) => a.DisplayName.CompareTo(b.DisplayName));

            return result;
        }

        public async Task<List<CustomTeamMember>> GetMembers(string id)
        {
            Beta.GraphServiceClient graphServiceClient = new Beta.GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) => {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("Bearer", AuthProvider.GetAccessTokenFromCurrentUser());

                return Task.FromResult(0);
            }));
            
            var members = await graphServiceClient
                .Teams[id].Members
                .Request()
                .GetAsync();

            List<CustomTeamMember> result = new List<CustomTeamMember>();

            foreach (Beta.AadUserConversationMember member in members.CurrentPage)
            {
                CustomTeamMember ctm = new CustomTeamMember()
                {
                    DisplayName = member.DisplayName,
                    Email = member.Email,                    
                };

                List<string> roles = (List<string>)member.Roles;

                if (roles.Count > 0)
                {
                    ctm.Roles = roles[0];
                }
                else
                {
                    ctm.Roles = "";
                }
                result.Add(ctm);
            }
            return result;
        }

 

        #region EveryoneHasSecrets
        [JSInvokable("SendDotNetInstanceToJS")]
        private async Task SendDotNetInstanceToJS()
        {
            var dotNetObjRef = DotNetObjectReference.Create(this);
            await js.InvokeVoidAsync("window.GetToken", dotNetObjRef);
        }

        [JSInvokable("SetToken")]
        public void SetToken(string token)
        {
            _accessToken = token;
            Console.WriteLine("Token in setToken: " + _accessToken);
        }
        #endregion
    }

    public class CustomTeamMember
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }

    
}
