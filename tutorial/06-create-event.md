<!-- markdownlint-disable MD002 MD041 -->

In this section you will add the ability to create events on the user's calendar.

1. Open **./Graph/GraphHelper.cs** and add the following function to the **GraphHelper** class.

    :::code language="csharp" source="../demo/GraphTutorial/Graph/GraphHelper.cs" id="CreateEventSnippet":::

    This code initializes an **Event** object and uses the Graph SDK to add it to the user's calendar.

1. Open **./Program.cs** and add the following functions to the **Program** class.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="UserInputSnippet":::

    These functions are helper functions for reading user input.

1. Add the following function to the **Program** class.

    :::code language="csharp" source="../demo/GraphTutorial/Program.cs" id="CreateEventSnippet":::

    This function prompts the user for subject, attendees, start, end, and body, then uses those values to call `GraphHelper.CreateEvent`.

1. Save all of your changes and run the app. Choose the **Add an event** option. Respond to the prompts to create a new event on the user's calendar.
