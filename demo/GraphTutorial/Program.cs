// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

// <ProgramSnippet>
Console.WriteLine(".NET Core Graph Tutorial\n");

var settings = Settings.LoadSettings();

// Initialize Graph
InitializeGraph();

// Greet the user by name
GreetUser();

int choice = -1;

while (choice != 0)
{
    Console.WriteLine("Please choose one of the following options:");
    Console.WriteLine("0. Exit");
    Console.WriteLine("1. Display access token");
    Console.WriteLine("2. View this week's calendar");
    Console.WriteLine("3. Add an event");

    try
    {
        choice = int.Parse(Console.ReadLine() ?? string.Empty);
    }
    catch (System.FormatException)
    {
        // Set to invalid value
        choice = -1;
    }

    switch(choice)
    {
        case 0:
            // Exit the program
            Console.WriteLine("Goodbye...");
            break;
        case 1:
            // Display access token
            DisplayAccessToken();
            break;
        case 2:
            // List emails from user's inbox
            ListInbox();
            break;
        case 3:
            // Send an email message
            SendMail();
            break;
        default:
            Console.WriteLine("Invalid choice! Please try again.");
            break;
    }
}
// </ProgramSnippet>

void InitializeGraph()
{
    // TODO
}

void GreetUser()
{
    // TODO
}

void DisplayAccessToken()
{
    // TODO
}

void ListInbox()
{
    // TODO
}

void SendMail()
{
    // TODO
}
