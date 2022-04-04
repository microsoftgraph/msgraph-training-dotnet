# How to run the completed project

## Prerequisites

To run the completed project in this folder, you need the following:

- The [.NET SDK](https://dotnet.microsoft.com/download) installed on your development machine. (**Note:** This tutorial was written with .NET SDK version 6.0.102. The steps in this guide may work with other versions, but that has not been tested.)
- A Microsoft work or school account.

If you don't have a Microsoft account, you can [sign up for the Microsoft 365 Developer Program](https://developer.microsoft.com/microsoft-365/dev-program) to get a free Microsoft 365 subscription.

## Register an application

You can register an application using the Azure Active Directory admin center, or by using the [Microsoft Graph PowerShell SDK](https://docs.microsoft.com/graph/powershell/get-started).

### Azure Active Directory admin center

1. Open a browser and navigate to the [Azure Active Directory admin center](https://aad.portal.azure.com) and login using a **personal account** (aka: Microsoft Account) or **Work or School Account**.

1. Select **Azure Active Directory** in the left-hand navigation, then select **App registrations** under **Manage**.

1. Select **New registration**. Enter a name for your application, for example, `.NET Graph Tutorial`.

1. Set **Supported account types** as desired. The options are:

    | Option | Who can sign in? |
    |--------|------------------|
    | **Accounts in this organizational directory only** | Only users in your Microsoft 365 organization |
    | **Accounts in any organizational directory** | Users in any Microsoft 365 organization (work or school accounts) |
    | **Accounts in any organizational directory ... and personal Microsoft accounts** | Users in any Microsoft 365 organization (work or school accounts) and personal Microsoft accounts |

1. Leave **Redirect URI** empty.

1. Select **Register**. On the application's **Overview** page, copy the value of the **Application (client) ID** and save it, you will need it in the next step. If you chose **Accounts in this organizational directory only** for **Supported account types**, also copy the **Directory (tenant) ID** and save it.

1. Select **Authentication** under **Manage**. Locate the **Advanced settings** section and change the **Allow public client flows** toggle to **Yes**, then choose **Save**.

#### Configure app-only auth (AAD admin center)

> **Note:** This section requires a work/school account with the Global administrator role. You only need to complete these steps if you plan on using the app-only portions of this sample.

1. Select **API permissions** under **Manage**.

1. Remove the default **User.Read** permission under **Configured permissions** by selecting the ellipses (**...**) in its row and selecting **Remove permission**.

1. Select **Add a permission**, then **Microsoft Graph**.

1. Select **Application permissions**.

1. Select **User.Read.All**, then select **Add permissions**.

1. Select **Grant admin consent for...**, then select **Yes** to provide admin consent for the selected permission.

1. Select **Certificates and secrets** under **Manage**, then select **New client secret**.

1. Enter a description, choose a duration, and select **Add**.

1. Copy the secret from the **Value** column, you will need it in the next steps.

### PowerShell

To use PowerShell, you'll need the Microsoft Graph PowerShell SDK. If you do not have it, see [Install the Microsoft Graph PowerShell SDK](https://docs.microsoft.com/graph/powershell/installation) for installation instructions.

1. Open PowerShell and run the [RegisterAppForUserAuth.ps1](RegisterAppForUserAuth.ps1) file with the following command, replacing *&lt;audience-value&gt;* with the desired value (see table below).

    > **Note:** The RegisterAppForUserAuth.ps1 script requires a work/school account with the Application administrator, Cloud application administrator, or Global administrator role.

    ```powershell
    .\RegisterAppForUserAuth.ps1 -AppName ".NET Graph Tutorial" -SignInAudience <audience-value>
    ```

    | SignInAudience value | Who can sign in? |
    |----------------------|------------------|
    | `AzureADMyOrg` | Only users in your Microsoft 365 organization |
    | `AzureADMultipleOrgs` | Users in any Microsoft 365 organization (work or school accounts) |
    | `AzureADandPersonalMicrosoftAccount` | Users in any Microsoft 365 organization (work or school accounts) and personal Microsoft accounts |
    | `PersonalMicrosoftAccount` | Only personal Microsoft accounts |

1. Copy the **Client ID** and **Auth tenant** values from the script output. You will need these values in the next step.

    ```powershell
    SUCCESS
    Client ID: 2fb1652f-a9a0-4db9-b220-b224b8d9d38b
    Auth tenant: common
    ```

#### Configure app-only auth (PowerShell)

> **Note:** This section requires a work/school account with the Global administrator role. You only need to complete these steps if you plan on using the app-only portions of this sample.

1. Run the [UpdateAppForAppOnlyAuth.ps1](UpdateAppForAppOnlyAuth.ps1) file with the following command, replacing *&lt;your-client-id&gt;* with your client ID.

    ```powershell
    .\UpdateAppForAppOnlyAuth.ps1 -AppId <your-client-id> -GraphScopes "User.Read.All"
    ```

1. Copy the **Tenant ID** and **Client secret** values from the script output. You will need these values in the next step.

    ```powershell
    SUCCESS
    Tenant ID: a795ad0f-7d82-4a3b-a2c0-0713ec72ade7
    Client secret: 2jv7Q~8eiOd_QafJ.....
    Secret expires: 2/16/2024 9:32:09 PM
    ```

## Configure the sample

1. Open [appsettings.json](./GraphTutorial/appsettings.json) and update the values according to the following table.

    | Setting | Value |
    |---------|-------|
    | `clientId` | The client ID of your app registration |
    | `tenantId` | The tenant ID of your organization (only needed if doing app-only) |
    | `authTenant` | If you chose the option to only allow users in your organization to sign in, change this value to your tenant ID. Otherwise leave as `common`. |

1. Initialize the [.NET development secret store](https://docs.microsoft.com/aspnet/core/security/app-secrets) by opening your CLI in the directory that contains **GraphTutorial.csproj** and running the following command.

    ```Shell
    dotnet user-secrets init
    ```

1. Add your client secret to the secret store using the following command, replacing *&lt;client-secret&gt;* with your client secret.

    ```Shell
    dotnet user-secrets set settings:clientSecret <client-secret>
    ```

    > **Note:** The .NET Secret Manager is only available during development. Production apps should store client secrets in a secure store, such as [Azure Key Vault](https://docs.microsoft.com/azure/key-vault/general/overview).

## Build and run the sample

In your command-line interface (CLI), navigate to the project directory and run the following commands.

```Shell
dotnet restore
dotnet build
dotnet run
```
