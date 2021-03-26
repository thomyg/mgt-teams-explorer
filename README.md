# mgt-teams-explorer

Small sample that uses the Microsoft Grapht Toolkit at its authentication providers to authenticate against AAD either in Teams or as a stand alone Blazor Server application. 


## Authentication
Implemented a custom AuthenticationState provider in Blazor based on this tutorial: https://www.youtube.com/watch?v=BmAnSNfFGsc

There is a small "trick" to get the token from the JavaSript mgt-login component to Blazor. Adding and EventListener to "loginCompleted" of the mgt-login component just to virtually click a helper button. That helper button sits on the login page and is wired to the C# part of the solution. User will never see that button, but it does the need trick to get the information into C#.

```html
<mgt-teams-provider client-id="f676f828-bee1-4fe5-ac7e-db0b1dac0fb4"
                    auth-popup-url="https://localhost:44392/auth"
                    authority="https://login.microsoftonline.com/thomy.onmicrosoft.com">
</mgt-teams-provider>
<mgt-msal-provider client-id="f74ae1c5-5a03-482f-8fca-d105431e90b5"
                   authority="https://login.microsoftonline.com/thomy.onmicrosoft.com"
                   scopes="user.read,people.read,user.readbasic.all,contacts.read,calendars.read,TeamMember.Read.All"
                   login-type="popup"
                   depends-on="mgt-teams-provider">
</mgt-msal-provider>
<mgt-login id="login"></mgt-login>
<button type="button" id="helperBtn" class="btn btn-primary" @onclick="LoginCompleted" style="display:none">
        I'm only here to help...
</button>
```
```javascript
(function () {
    let loginBtn = document.getElementById("login");
    loginBtn.addEventListener("loginCompleted", () => {
        let helperBtn = document.getElementById("helperBtn");
        helperBtn.click();
    });
})();
```

```c#
public async Task LoginCompleted()
{
   UserAuthenticated();

}
``` 

## Calling Microsoft Graph

The calls to Microsoft Graph are just a simple example of getting all Teams and listing members of the selected team.

```c#
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
 ```

Second "trick" -> we are using the GA and the Beta version of Graph. Have a close look at the .csproj file and find the alias definition of the Beta package.

```xml
<Target Name="ChangeAliasesOfStrongNameAssemblies" BeforeTargets="FindReferenceAssembliesForReferences;ResolveReferences">
    <ItemGroup>
      <ReferencePath Condition="'%(FileName)' == 'Microsoft.Graph.Beta'">
        <Aliases>BetaLib</Aliases>
      </ReferencePath>
    </ItemGroup>
  </Target>
```

If you are interessted on the mgt-login part of the solution have a look at https://www.youtube.com/watch?v=v_jLXCR1fzE to get an overview.
