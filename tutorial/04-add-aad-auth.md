<!-- markdownlint-disable MD002 MD041 -->

In this exercise you will extend the application from the previous exercise to support authentication with Azure AD. This is required to obtain the necessary OAuth access token to call the Microsoft Graph. In this step you will integrate the [Microsoft Authentication Library (MSAL) for .NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) into the application.

Create a new file in the **GraphTutorial** directory named **appsettings.json**. Add the following text in that file.

```json
{
  "appId": "YOUR_APP_ID_HERE",
  "scopes": [
    "User.Read",
    "Calendars.Read"
  ]
}
```

Replace `YOUR_APP_ID_HERE` with the application ID you created in the Azure portal.

> [!IMPORTANT]
> If you're using source control such as git, now would be a good time to exclude the **appsettings.json** file from source control to avoid inadvertently leaking your app ID.

## Implement sign-in

In this section you will create an authentication provider that can be used with the Graph SDK and can also be used to explicitly request an access token by using the [device code flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-device-code).

### Create an authentication provider

1. Create a new directory in the **GraphTutorial** directory named **Authentication**.
1. Create a new file in the **Authentication** directory named **DeviceCodeAuthProvider.cs** and add the following code to that file.

    ```csharp
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    namespace GraphTutorial
    {
        public class DeviceCodeAuthProvider : IAuthenticationProvider
        {
            private IPublicClientApplication _msalClient;
            private string[] _scopes;
            private IAccount _userAccount;

            public DeviceCodeAuthProvider(string appId, string[] scopes)
            {
                _scopes = scopes;

                _msalClient = PublicClientApplicationBuilder
                    .Create(appId)
                    // Set the tenant ID to "organizations" to disable personal accounts
                    // Azure OAuth does not support device code flow for personal accounts
                    // See https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-device-code
                    .WithTenantId("organizations")
                    .Build();
            }

            public async Task<string> GetAccessToken()
            {
                // If there is no saved user account, the user must sign-in
                if (_userAccount == null)
                {
                    try
                    {
                        // Invoke device code flow so user can sign-in with a browser
                        var result = await _msalClient.AcquireTokenWithDeviceCode(_scopes, callback => {
                            Console.WriteLine(callback.Message);
                            return Task.FromResult(0);
                        }).ExecuteAsync();

                        _userAccount = result.Account;
                        return result.AccessToken;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"Error getting access token: {exception.Message}");
                        return null;
                    }
                }
                else
                {
                    // If there is an account, call AcquireTokenSilent
                    // By doing this, MSAL will refresh the token automatically if
                    // it is expired. Otherwise it returns the cached token.

                        var result = await _msalClient
                            .AcquireTokenSilent(_scopes, _userAccount)
                            .ExecuteAsync();

                       return result.AccessToken;
                }
            }

            // This is the required function to implement IAuthenticationProvider
            // The Graph SDK will call this function each time it makes a Graph
            // call.
            public async Task AuthenticateRequestAsync(HttpRequestMessage requestMessage)
            {
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("bearer", await GetAccessToken());
            }
        }
    }
    ```

Consider what this code does.

- It uses the MSAL `IPublicClientApplication` implementation to request and manage tokens.
- The `GetAccessToken` function:
  - Signs in the user if they are not already signed in using the device code flow.
  - Ensures that the token returned is always fresh by using the `AcquireTokenSilent` function, which returns the cached token if it not expired, and refreshes the token if it is expired.
- It implements the `IAuthenticationProvider` interface so that the Graph SDK can use the class to authenticate Graph calls.

## Sign in and display the access token

In this section you will update the application to call the `GetAccessToken` function, which will sign in the user. You will also add code to display the token.

1. Open **Program.cs** and add the following `using` statements to the top of the file.

    ```csharp
    using Microsoft.Extensions.Configuration;
    using System.IO;
    ```

1. Add the following function to the `Program` class.

    ```csharp
    static IConfigurationRoot LoadAppSettings()
    {
        var appConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        // Check for required settings
        if (string.IsNullOrEmpty(appConfig["appId"]) ||
            // Make sure there's at least one value in the scopes array
            string.IsNullOrEmpty(appConfig["scopes:0"]))
        {
            return null;
        }

        return appConfig;
    }
    ```

1. Add the following code to the `Main` function immediately after the `Console.WriteLine(".NET Core Graph Tutorial\n");` line.

    ```csharp
    var appConfig = LoadAppSettings();

    if (appConfig == null)
    {
        Console.WriteLine("Missing or invalid appsettings.json...exiting");
        return;
    }

    var appId = appConfig["appId"];
    var scopes = appConfig.GetSection("scopes").Get<string[]>();

    // Initialize the auth provider with values from appsettings.json
    var authProvider = new DeviceCodeAuthProvider(appId, scopes);

    // Request a token to sign in the user
    var accessToken = authProvider.GetAccessToken().Result;
    ```

1. Add the following code to the `Main` function immediately after the `// Display access token` line.

    ```csharp
    Console.WriteLine($"Access token: {accessToken}\n");
    ```

1. Build and run the app. The application displays a URL and device code.

    ```Shell
    .NET Core Graph Tutorial

    To sign in, use a web browser to open the page https://microsoft.com/devicelogin and enter the code F7CG945YZ to authenticate.
    ```

1. Open a browser and browse to the URL displayed. Enter the provided code and sign in. Once completed, return to the application and choose the **1. Display access token** option to display the access token.
