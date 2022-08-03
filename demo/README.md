# How to run the completed project

## Prerequisites

To run the completed project in this folder, you need the following:

- The [.NET SDK](https://dotnet.microsoft.com/download) installed on your development machine. (**Note:** This tutorial was written with .NET SDK version 6.0.102. The steps in this guide may work with other versions, but that has not been tested.)
- A Microsoft work or school account.

If you don't have a Microsoft account, you can [sign up for the Microsoft 365 Developer Program](https://developer.microsoft.com/microsoft-365/dev-program) to get a free Microsoft 365 subscription.

## Configure the sample for user authentication

Instructions for user authentication configuration are located in [BASIC-CONFIG.md](BASIC-CONFIG.md).

## Enable app-only authentication

By default this sample disables the app-only authentication portion. This is because this portion requires access to a work/school account with administrative privileges. For instructions to configure and enable app-only authentication, see [APP-ONLY-CONFIG.md](APP-ONLY-CONFIG.md).

## Build and run the sample

In your command-line interface (CLI), navigate to the project directory and run the following commands.

```Shell
dotnet restore
dotnet build
dotnet run
```
