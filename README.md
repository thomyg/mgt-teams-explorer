# mgt-teams-explorer

Small sample that uses the Microsoft Grapht Toolkit at its authentication providers to authenticate against AAD either in Teams or as a stand alone Blazor Server application. 

Implemented a custom AuthenticationState provider in Blazor based on this tutorial: https://www.youtube.com/watch?v=BmAnSNfFGsc

There is a small "trick" to get the token from the JavaSript mgt-login component to Blazor. Adding and EventListener to "loginCompleted" of the mgt-login component just to virtually click a helper button. That helper button sits on the login page and is wired to the C# part of the solution. User will never see that button, but it does the need trick to get the information into C#.

The calls to Microsoft Graph are just a simple example of getting all Teams and listing members of the selected team.

Second "trick" -> we are using the GA and the Beta version of Graph. Have a close look at the .csproj file and find the alias definition of the Beta package.

If you are interessted on the mgt-login part of the solution have a look at https://www.youtube.com/watch?v=v_jLXCR1fzE to get an overview.
