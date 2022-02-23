---
ms.localizationpriority: medium
---

<!-- markdownlint-disable MD041 -->

In this section you will add app-only authentication to the application. This section is optional, and requires completion of [Optional: configure app-only authentication](?tutorial-step=2). These steps can only be completed with a work or school account.

> [!div class="nextstepaction"]
> [I don't need app-only, skip to the end](?tutorial-step=10)

## Configure Graph client for app-only authentication

In this section you will use the `ClientSecretCredential` class to request an access token by using the [client credentials flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow).

1. Update the value of `tenantId` in **appsettings.json** (or **appsettings.Development.json**) with your organization's tenant ID.

1. Add your client secret to the [.NET Secret Manager](https://docs.microsoft.com/aspnet/core/security/app-secrets). In your command-line interface, change the directory to the location of **GraphTutorial.csproj** and run the following commands, replacing *&lt;client-secret&gt;* with your client secret.

    ```dotnetcli
    dotnet user-secrets init
    dotnet user-secrets set settings:clientSecret <client-secret>
    ```

    > [!NOTE]
    > The .NET Secret Manager is only available during development. Production apps should store client secrets in a secure store, such as [Azure Key Vault](https://docs.microsoft.com/azure/key-vault/general/overview).

1. Open **./GraphHelper.cs** and add the following code to the **GraphHelper** class.

    :::code language="csharp" source="../demo/GraphTutorial/GraphHelper.cs" id="AppOnyAuthConfigSnippet":::
