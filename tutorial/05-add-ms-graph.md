<!-- markdownlint-disable MD002 MD041 -->

In this exercise you will incorporate the Microsoft Graph into the application. For this application, you will use the [Microsoft Graph .NET Client Library](https://github.com/microsoftgraph/msgraph-sdk-dotnet) to make calls to Microsoft Graph.

## Get user details

1. Create a new directory in the **GraphTutorial** directory named **Graph**.
1. Create a new file in the **Graph** directory named **GraphHelper.cs** and add the following code to that file.

    ```csharp
    using Microsoft.Graph;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace GraphTutorial
    {
        public class GraphHelper
        {
            private static GraphServiceClient graphClient;
            public static void Initialize(IAuthenticationProvider authProvider)
            {
                graphClient = new GraphServiceClient(authProvider);
            }

            public static async Task<User> GetMeAsync()
            {
                try
                {
                    // GET /me
                    return await graphClient.Me.Request().GetAsync();
                }
                catch (ServiceException ex)
                {
                    Console.WriteLine($"Error getting signed-in user: {ex.Message}");
                    return null;
                }
            }
        }
    }
    ```

1. Add the following code in `Main` in **Program.cs** just after the `GetAccessToken` call to get the user and output the user's display name.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="GetUserSnippet":::

If you run the app now, after you log in the app welcomes you by name.

## Get calendar events from Outlook

Add the following function to the `GraphHelper` class to get events from the user's calendar.

:::code language="csharp" source="../demo/GraphTutorial/Graph/GraphHelper.cs" id="GetEventsSnippet":::

Consider what this code is doing.

- The URL that will be called is `/me/events`.
- The `Select` function limits the fields returned for each event to just those the app will actually use.
- The `OrderBy` function sorts the results by the date and time they were created, with the most recent item being first.

## Display the results

1. Add the following function to the `Program` class to format the [dateTimeTimeZone](/graph/api/resources/datetimetimezone?view=graph-rest-1.0) properties from Microsoft Graph into a user-friendly format.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="FormatDateSnippet":::

1. Add the following function to the `Program` class to get the user's events and output them to the console.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="ListEventsSnippet":::

1. Add the following just after the `// List the calendar` comment in the `Main` function.

    ```csharp
    ListCalendarEvents();
    ```

Save all of your changes and run the app. Choose the **List calendar events** option to see a list of the user's events.

```Shell
Welcome Adele Vance

Please choose one of the following options:
0. Exit
1. Display access token
2. List calendar events
2
Events:
Subject: Team meeting
  Organizer: Adele Vance
  Start: 5/22/19, 3:00 PM
  End: 5/22/19, 4:00 PM
Subject: Team Lunch
  Organizer: Adele Vance
  Start: 5/24/19, 6:30 PM
  End: 5/24/19, 8:00 PM
Subject: Flight to Redmond
  Organizer: Adele Vance
  Start: 5/26/19, 4:30 PM
  End: 5/26/19, 7:00 PM
Subject: Let's meet to discuss strategy
  Organizer: Patti Fernandez
  Start: 5/27/19, 10:00 PM
  End: 5/27/19, 10:30 PM
Subject: All-hands meeting
  Organizer: Adele Vance
  Start: 5/28/19, 3:30 PM
  End: 5/28/19, 5:00 PM
```
