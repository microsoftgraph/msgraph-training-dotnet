<!-- markdownlint-disable MD002 MD041 -->

Begin by creating a new .NET Core console project using the [.NET Core CLI](/dotnet/core/tools/).

1. Open your command-line interface (CLI) in a directory where you want to create the project. Run the following command.

    ```Shell
    dotnet new console -o GraphTutorial
    ```

1. Once the project is created, verify that it works by changing the current directory to the **GraphTutorial** directory and running the following command in your CLI.

    ```Shell
    dotnet run
    ```

    If it works, the app should output `Hello World!`.

## Install dependencies

Before moving on, add some additional dependencies that you will use later.

- [Microsoft.Extensions.Configuration.UserSecrets](https://github.com/aspnet/extensions) to read application configuration from the [.NET development secret store](https://docs.microsoft.com/aspnet/core/security/app-secrets).
- [Microsoft Authentication Library (MSAL) for .NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) to authenticate the user and acquire access tokens.
- [Microsoft Graph .NET Client Library](https://github.com/microsoftgraph/msgraph-sdk-dotnet) to make calls to the Microsoft Graph.

Run the following commands in your CLI to install the dependencies.

```Shell
dotnet add package Microsoft.Extensions.Configuration.UserSecrets --version 3.1.2
dotnet add package Microsoft.Identity.Client --version 4.10.0
dotnet add package Microsoft.Graph --version 3.0.1
```

## Design the app

In this section you will create a simple console-based menu.

Open **Program.cs** in a text editor (such as [Visual Studio Code](https://code.visualstudio.com/)) and replace its entire contents with the following code.

:::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="ProgramSnippet":::

This implements a basic menu and reads the user's choice from the command line.
