// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Configuration;
using System;

namespace GraphTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".NET Core Graph Tutorial\n");

            // <InitializationSnippet>
            var appConfig = LoadAppSettings();

            if (appConfig == null)
            {
                Console.WriteLine("Missing or invalid appsettings.json...exiting");
                return;
            }

            var appId = appConfig["appId"];
            var scopesString = appConfig["scopes"];
            var scopes = scopesString.Split(';');

            // Initialize the auth provider with values from appsettings.json
            var authProvider = new DeviceCodeAuthProvider(appId, scopes);

            // Request a token to sign in the user
            var accessToken = authProvider.GetAccessToken().Result;
            // </InitializationSnippet>

            // <GetUserSnippet>
            // Initialize Graph client
            GraphHelper.Initialize(authProvider);

            // Get signed in user
            var user = GraphHelper.GetMeAsync().Result;
            Console.WriteLine($"Welcome {user.DisplayName}!\n");
            // </GetUserSnippet>

            int choice = -1;

            while (choice != 0) {
                Console.WriteLine("Please choose one of the following options:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Display access token");
                Console.WriteLine("2. View this week's calendar");
                Console.WriteLine("3. Add an event");

                try
                {
                    choice = int.Parse(Console.ReadLine());
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
                        Console.WriteLine($"Access token: {accessToken}\n");
                        break;
                    case 2:
                        // List the calendar
                        ListCalendarEvents(
                            user.MailboxSettings.TimeZone,
                            $"{user.MailboxSettings.DateFormat} {user.MailboxSettings.TimeFormat}"
                        );
                        break;
                    case 3:
                        // Create a new event
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        // <ListEventsSnippet>
        static void ListCalendarEvents(string userTimeZone, string dateTimeFormat)
        {
            var events = GraphHelper
                .GetCurrentWeekCalendarViewAsync(DateTime.Today, userTimeZone)
                .Result;

            Console.WriteLine("Events:");

            foreach (var calendarEvent in events)
            {
                Console.WriteLine($"Subject: {calendarEvent.Subject}");
                Console.WriteLine($"  Organizer: {calendarEvent.Organizer.EmailAddress.Name}");
                Console.WriteLine($"  Start: {FormatDateTimeTimeZone(calendarEvent.Start, dateTimeFormat)}");
                Console.WriteLine($"  End: {FormatDateTimeTimeZone(calendarEvent.End, dateTimeFormat)}");
            }
        }
        // </ListEventsSnippet>

        // <FormatDateSnippet>
        static string FormatDateTimeTimeZone(
            Microsoft.Graph.DateTimeTimeZone value,
            string dateTimeFormat)
        {
            // Parse the date/time string from Graph into a DateTime
            var dateTime = DateTime.Parse(value.DateTime);

            return dateTime.ToString(dateTimeFormat);
        }
        // </FormatDateSnippet>

        // <LoadAppSettingsSnippet>
        static IConfigurationRoot LoadAppSettings()
        {
            var appConfig = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            // Check for required settings
            if (string.IsNullOrEmpty(appConfig["appId"]) ||
                string.IsNullOrEmpty(appConfig["scopes"]))
            {
                return null;
            }

            return appConfig;
        }
        // </LoadAppSettingsSnippet>
    }
}
