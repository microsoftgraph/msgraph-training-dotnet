---
ms.localizationpriority: medium
---

<!-- markdownlint-disable MD041 -->

In this section you will incorporate the Microsoft Graph into the application. For this application, you will use the [Microsoft Graph .NET Client Library](https://github.com/microsoftgraph/msgraph-sdk-dotnet) to make calls to Microsoft Graph.

## Get user details

1. Open **./GraphHelper.cs** and add the following function to the **GraphHelper** class.

    :::code language="csharp" source="../demo/GraphTutorial/GraphHelper.cs" id="GetUserSnippet":::

1. Replace the empty `DisplayAccessTokenAsync` function in **Program.cs** with the following.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="GreetUserSnippet":::

If you run the app now, after you log in the app welcomes you by name.

```Shell
Hello, Megan Bowen!
Email: MeganB@contoso.com
```

## Code explained

Consider the code in the `GetUserAsync` function. It's only a few lines, but there are some key details to notice.

### Accessing 'me'

The function uses the `_userClient.Me` request builder, which builds a request to the [Get user](https://docs.microsoft.com/graph/api/user-get) API. This API is accessible two ways:

```http
GET /me
GET /users/{user-id}
```

In this case, the code will call the `GET /me` API endpoint. This is a shortcut method to get the authenticated user without knowing their user ID.

> [!NOTE]
> Because the `GET /me` API endpoint gets the authenticated user, it is only availabe to apps that use user authentication. App-only authentication apps cannot access this endpoint.

### Requesting specific properties

The function uses the `Select` method on the request to specify the set of properties it needs. This adds the [$select query parameter](https://docs.microsoft.com/graph/query-parameters#select-parameter) to the API call.

### Strongly-typed return type

The function returns a `Microsoft.Graph.User` object deserialized from the JSON response from the API. Because the code uses `Select`, only the requested properties will have values in the returned `User` object. All other properties will have default values.
