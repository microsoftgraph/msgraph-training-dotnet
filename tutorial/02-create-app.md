<!-- markdownlint-disable MD002 MD041 -->

Begin by creating a new .NET Core console project using the [.NET Core CLI](/dotnet/core/tools/?tabs=netcore2x).

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

- [Microsoft.Extensions.Configuration](https://github.com/aspnet/Extensions) to read application configuration from a JSON file.
- [Microsoft Authentication Library (MSAL) for .NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) to authenticate the user and acquire access tokens.
- [Microsoft Graph .NET Client Library](https://github.com/microsoftgraph/msgraph-sdk-dotnet) to make calls to the Microsoft Graph.

Run the following commands in your CLI to install the dependencies.

```Shell
dotnet add package Microsoft.Extensions.Configuration --version 3.1.0
dotnet add package Microsoft.Extensions.Configuration.FileExtensions --version 3.1.0
dotnet add package Microsoft.Extensions.Configuration.Json --version 3.1.0
dotnet add package Microsoft.Extensions.Configuration.Binder --version 3.1.0
dotnet add package Microsoft.Identity.Client --version 4.7.1
dotnet add package Microsoft.Graph --version 1.21.0
```

## Design the app

In this section you will create a simple console-based menu.

Open **Program.cs** in a text editor (such as [Visual Studio Code](https://code.visualstudio.com/)) and replace its entire contents with the following code.

[!code-csharp[](../demos/01-create-app/GraphTutorial/Program.cs)]

This implements a basic menu and reads the user's choice from the command line.
