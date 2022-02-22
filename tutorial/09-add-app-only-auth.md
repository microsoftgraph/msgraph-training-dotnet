---
ms.localizationpriority: medium
---

<!-- markdownlint-disable MD041 -->

In this section you will add app-only authentication to the application. This section is optional, and requires completion of [Optional: configure app-only authentication](?tutorial-step=2). These steps can only be completed with a work or school account.

> [!div class="nextstepaction"]
> [I don't need app-only, skip to the end](?tutorial-step=10)

## Configure Graph client for app-only authentication

In this section you will use the `ClientSecretCredential` class to request an access token by using the [client credentials flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow).

Open **./Graph/GraphHelper.cs** and add the following code to the **GraphHelper** class.

:::code language="csharp" source="../demo/GraphTutorial/GraphHelper.cs" id="AppOnyAuthConfigSnippet":::
