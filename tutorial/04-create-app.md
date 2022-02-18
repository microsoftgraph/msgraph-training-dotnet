---
ms.localizationpriority: medium
---

<!-- markdownlint-disable MD002 MD041 -->

Begin by creating a new .NET Core console project using the [.NET Core CLI](/dotnet/core/tools/).

1. Open your command-line interface (CLI) in a directory where you want to create the project. Run the following command.

    ```dotnetcli
    dotnet new console -o GraphTutorial
    ```

1. Once the project is created, verify that it works by changing the current directory to the **GraphTutorial** directory and running the following command in your CLI.

    ```dotnetcli
    dotnet run
    ```

    If it works, the app should output `Hello, World!`.

## Install dependencies

Before moving on, add some additional dependencies that you will use later.

- [.NET configuration packages](https://docs.microsoft.com/dotnet/core/extensions/configuration) to read application configuration from **appsettings.json**.
- [Azure Identity client library for .NET](https://www.nuget.org/packages/Azure.Identity)  to authenticate the user and acquire access tokens.
- [Microsoft Graph .NET client library](https://github.com/microsoftgraph/msgraph-sdk-dotnet) to make calls to the Microsoft Graph.

Run the following commands in your CLI to install the dependencies.

```Shell
dotnet add package Microsoft.Extensions.Configuration.Binder --version 6.0.0
dotnet add package Microsoft.Extensions.Configuration.Json --version 6.0.0
dotnet add package Azure.Identity --version 1.5.0
dotnet add package Microsoft.Graph --version 4.18.0
```

## Load application settings

In this section you'll add the details of your app registration to the project.

1. Create a file in the **GraphTutorial** directory named **appsettings.json** and add the following code.

    :::code language="json" source="../demo/GraphTutorial/appsettings.json":::

1. Update the values according to the following table.

    | Setting | Value |
    |---------|-------|
    | `clientId` | The client ID of your app registration |
    | `clientSecret' | The client secret from your app registration (only if you configured app-only authentication) |
    | `authTenant` | If you chose the option to only allow users in your organization to sign in, change this value to your tenant ID. Otherwise leave as `common`. |

    > [!TIP]
    > Optionally, you can set these values in a separate file named **appsettings.Development.json**, or in the [.NET Secret Manager](https://docs.microsoft.com/aspnet/core/security/app-secrets).

1. Update **GraphTutorial.csproj** to copy **appsettings.json** to the output directory. Add the following code between the `<Project>` and `</Project>` lines.

    ```xml
    <ItemGroup>
      <None Include="appsettings*.json">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
      </None>
    </ItemGroup>
    ```

1. Create a file in the **GraphTutorial** directory named **Settings.cs** and add the following code.

    :::code language="csharp" source="../demo/GraphTutorial/Settings.cs" id="SettingsSnippet":::

## Design the app

In this section you will create a simple console-based menu.

1. Open **./Program.cs** and replace its entire contents with the following code.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="ProgramSnippet":::

1. Add the following placeholder methods at the end of the file. You'll implement them in later steps.

    ```csharp
    void InitializeGraph()
    {
        // TODO
    }

    async Task GreetUserAsync()
    {
        // TODO
    }

    async Task DisplayAccessTokenAsync(string[]? userScopes)
    {
        // TODO
    }

    async Task ListInboxAsync()
    {
        // TODO
    }

    async Task SendMailAsync()
    {
        // TODO
    }

    async Task ListUsersAsync()
    {
        // TODO
    }
    ```

This implements a basic menu and reads the user's choice from the command line.
