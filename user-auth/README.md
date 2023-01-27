# How to run the completed project

## Prerequisites

To run the completed project in this folder, you need the following:

- The [.NET SDK](https://dotnet.microsoft.com/download) installed on your development machine. (**Note:** This tutorial was written with .NET SDK version 7.0.102. The steps in this guide may work with other versions, but that has not been tested.)
- Either a personal Microsoft account with a mailbox on Outlook.com, or a Microsoft work or school account. If you don't have a Microsoft account, there are a couple of options to get a free account:
  - You can [sign up for a new personal Microsoft account](https://signup.live.com/signup?wa=wsignin1.0&rpsnv=12&ct=1454618383&rver=6.4.6456.0&wp=MBI_SSL_SHARED&wreply=https://mail.live.com/default.aspx&id=64855&cbcxt=mai&bk=1454618383&uiflavor=web&uaid=b213a65b4fdc484382b6622b3ecaa547&mkt=E-US&lc=1033&lic=1).
  - You can [sign up for the Microsoft 365 Developer Program](https://developer.microsoft.com/microsoft-365/dev-program) to get a free Microsoft 365 subscription.

## Register an application

You can register an application using the Azure Active Directory admin center, or by using the [Microsoft Graph PowerShell SDK](https://learn.microsoft.com/graph/powershell/get-started).

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

### PowerShell

To use PowerShell, you'll need the Microsoft Graph PowerShell SDK. If you do not have it, see [Install the Microsoft Graph PowerShell SDK](https://learn.microsoft.com/graph/powershell/installation) for installation instructions.

> **NOTE:** The PowerShell script requires a work/school account with the Application administrator, Cloud application administrator, or Global administrator role. If your account has the Application developer role, you can register in the Azure AD admin center.

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

## Configure the sample

1. Open [appsettings.json](./GraphTutorial/appsettings.json) and update the values according to the following table.

    | Setting | Value |
    |---------|-------|
    | `clientId` | The client ID of your app registration |
    | `tenantId` | If you chose the option to only allow users in your organization to sign in, change this value to your tenant ID. Otherwise leave as `common`. |

## Run the sample

In your command-line interface (CLI), navigate to the project directory and run the following commands.

```Shell
dotnet restore
dotnet build
dotnet run
```
