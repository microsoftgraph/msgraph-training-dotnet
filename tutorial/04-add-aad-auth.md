---
ms.localizationpriority: medium
---

<!-- markdownlint-disable MD041 -->

In this section you will extend the application from the previous exercise to support authentication with Azure AD. This is required to obtain the necessary OAuth access token to call the Microsoft Graph. In this step you will integrate the [Azure Identity client library for .NET](https://www.nuget.org/packages/Azure.Identity) into the application and configure authentication for the [Microsoft Graph .NET client library](https://github.com/microsoftgraph/msgraph-sdk-dotnet).

The Azure Identity library provides a number of `TokenCredential` classes that implement OAuth2 token flows. The Microsoft Graph client library uses those classes to authenticate calls to Microsoft Graph. In this example, we'll use the following `TokenCredential` classes.

- `DeviceCodeCredential` implements the [device code flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-device-code) for user authentication.
- `ClientSecretCredential` implements the [client credentials flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow) for app-only authentication.

## Configure Graph client for user authentication

In this section you will use the `DeviceCodeCredential` class to request an access token by using the [device code flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-device-code).

1. Create a new file in the **GraphTutorial** directory named **GraphHelper.cs** and add the following code to that file.

    :::code language="csharp" source="../demo/GraphTutorial/GraphHelper.cs" id="GraphHelperSnippet":::

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
