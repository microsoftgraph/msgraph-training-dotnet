---
ms.localizationpriority: medium
---

<!-- markdownlint-disable MD002 MD041 -->

In this exercise you will extend the application from the previous exercise to support authentication with Azure AD. This is required to obtain the necessary OAuth access token to call the Microsoft Graph. In this step you will integrate the [Microsoft Authentication Library (MSAL) for .NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) into the application.

1. Initialize the [.NET development secret store](/aspnet/core/security/app-secrets) by opening your CLI in the directory that contains **GraphTutorial.csproj** and running the following command.

    ```Shell
    dotnet user-secrets init
    ```

1. Add your application ID and a list of required scopes to the secret store using the following commands. Replace `YOUR_APP_ID_HERE` with the application ID you created in the Azure portal.

    ```Shell
    dotnet user-secrets set appId "YOUR_APP_ID_HERE"
    dotnet user-secrets set scopes "User.Read;MailboxSettings.Read;Calendars.ReadWrite"
    ```

    Let's look at the permission scopes you just set.

    - **User.Read** will allow the app to read the signed-in user's profile to get information such as display name and email address.
    - **MailboxSettings.Read** will allow the app to read the user's preferred time zone, date format, and time format.
    - **Calendars.ReadWrite** will allow the app to read the existing events on the user's calendar and add new events.

## Implement sign-in

In this section you will use the `DeviceCodeCredential` class to request an access token by using the [device code flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-device-code).

1. Create a new directory in the **GraphTutorial** directory named **Graph**.
1. Create a new file in the **Graph** directory named **GraphHelper.cs** and add the following code to that file.

    ```csharp
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Core;
    using Azure.Identity;
    using Microsoft.Graph;
    using TimeZoneConverter;

    namespace GraphTutorial
    {
        public class GraphHelper
        {
            private static DeviceCodeCredential tokenCredential;
            private static GraphServiceClient graphClient;

            public static void Initialize(string clientId,
                                          string[] scopes,
                                          Func<DeviceCodeInfo, CancellationToken, Task> callBack)
            {
                tokenCredential = new DeviceCodeCredential(callBack, clientId);
                graphClient = new GraphServiceClient(tokenCredential, scopes);
            }

            public static async Task<string> GetAccessTokenAsync(string[] scopes)
            {
                var context = new TokenRequestContext(scopes);
                var response = await tokenCredential.GetTokenAsync(context);
                return response.Token;
            }
        }
    }
    ```

1. Add the following function to the `Program` class.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="LoadAppSettingsSnippet":::

1. Add the following code to the `Main` function immediately after the `Console.WriteLine(".NET Core Graph Tutorial\n");` line.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="InitializationSnippet":::

1. Add the following code to the `Main` function immediately after the `// Display access token` line.

    ```csharp
    Console.WriteLine($"Access token: {accessToken}\n");
    ```

1. Build and run the app. The application displays a URL and device code.

    ```Shell
    PS C:\Source\GraphTutorial> dotnet run
    .NET Core Graph Tutorial

    To sign in, use a web browser to open the page https://microsoft.com/devicelogin and enter the code F7CG945YZ to authenticate.
    ```

    > [!TIP]
    > If you encounter errors, compare your **Program.cs** with the [example on GitHub](https://github.com/microsoftgraph/msgraph-training-dotnet-core/blob/master/demo/GraphTutorial/Program.cs).

1. Open a browser and browse to the URL displayed. Enter the provided code and sign in. Once completed, return to the application and choose the **1. Display access token** option to display the access token.
