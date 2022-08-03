# Configure app-only authentication

## Update app registration

You can update your existing application registration using the Azure Active Directory admin center, or by using the [Microsoft Graph PowerShell SDK](https://docs.microsoft.com/graph/powershell/get-started).

> **NOTE**
> The PowerShell script requires a work/school account with the Application administrator, Cloud application administrator, or Global administrator role. If your account has the Application developer role, you can register in the Azure AD admin center.

### Configure app-only auth (AAD admin center)

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

### Configure app-only auth (PowerShell)

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
    | `tenantId` | The tenant ID of your organization (only needed if doing app-only) |

1. Initialize the [.NET development secret store](https://docs.microsoft.com/aspnet/core/security/app-secrets) by opening your CLI in the directory that contains **GraphTutorial.csproj** and running the following command.

    ```Shell
    dotnet user-secrets init
    ```

1. Add your client secret to the secret store using the following command, replacing *&lt;client-secret&gt;* with your client secret.

    ```Shell
    dotnet user-secrets set settings:clientSecret <client-secret>
    ```

    > **Note:** The .NET Secret Manager is only available during development. Production apps should store client secrets in a secure store, such as [Azure Key Vault](https://docs.microsoft.com/azure/key-vault/general/overview).
