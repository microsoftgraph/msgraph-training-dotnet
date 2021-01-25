// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeZoneConverter;

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
                return await graphClient.Me
                    .Request()
                    .Select(u => new{
                        u.DisplayName,
                        u.MailboxSettings
                    })
                    .GetAsync();
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting signed-in user: {ex.Message}");
                return null;
            }
        }

        // <GetEventsSnippet>
        public static async Task<IEnumerable<Event>> GetCurrentWeekCalendarViewAsync(
            DateTime today,
            string timeZone)
        {
            // Configure a calendar view for the current week
            var startOfWeek = GetUtcStartOfWeekInTimeZone(today, timeZone);
            var endOfWeek = startOfWeek.AddDays(7);

            var viewOptions = new List<QueryOption>
            {
                new QueryOption("startDateTime", startOfWeek.ToString("o")),
                new QueryOption("endDateTime", endOfWeek.ToString("o"))
            };

            try
            {
                var events = await graphClient.Me
                    .CalendarView
                    .Request(viewOptions)
                    // Send user time zone in request so date/time in
                    // response will be in preferred time zone
                    .Header("Prefer", $"outlook.timezone=\"{timeZone}\"")
                    // Get max 50 per request
                    .Top(50)
                    // Only return fields app will use
                    .Select(e => new
                    {
                        e.Subject,
                        e.Organizer,
                        e.Start,
                        e.End
                    })
                    // Order results chronologically
                    .OrderBy("start/dateTime")
                    .GetAsync();

                return events.CurrentPage;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting events: {ex.Message}");
                return null;
            }
        }

        private static DateTime GetUtcStartOfWeekInTimeZone(DateTime today, string timeZoneId)
        {
            // Time zone returned by Graph could be Windows or IANA style
            // .NET Core's FindSystemTimeZoneById needs IANA on Linux/MacOS,
            // and needs Windows style on Windows.
            // TimeZoneConverter can handle this for us
            TimeZoneInfo userTimeZone = TZConvert.GetTimeZoneInfo(timeZoneId);

            // Assumes Sunday as first day of week
            int diff = System.DayOfWeek.Sunday - today.DayOfWeek;

            // create date as unspecified kind
            var unspecifiedStart = DateTime.SpecifyKind(today.AddDays(diff), DateTimeKind.Unspecified);

            // convert to UTC
            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedStart, userTimeZone);
        }
        // </GetEventsSnippet>

        // <CreateEventSnippet>
        public static async Task CreateEvent(
            string timeZone,
            string subject,
            DateTime start,
            DateTime end,
            List<string> attendees,
            string body = null)
        {
            // Create a new Event object with required
            // values
            var newEvent = new Event
            {
                Subject = subject,
                Start = new DateTimeTimeZone
                {
                    DateTime = start.ToString("o"),
                    // Set to the user's time zone
                    TimeZone = timeZone
                },
                End = new DateTimeTimeZone
                {
                    DateTime = end.ToString("o"),
                    // Set to the user's time zone
                    TimeZone = timeZone
                }
            };

            // Only add attendees if there are actual
            // values
            if (attendees.Count > 0)
            {
                var requiredAttendees = new List<Attendee>();

                foreach(var email in attendees)
                {
                    requiredAttendees.Add(new Attendee
                    {
                        Type = AttendeeType.Required,
                        EmailAddress = new EmailAddress
                        {
                            Address = email
                        }
                    });
                }

                newEvent.Attendees = requiredAttendees;
            }

            // Only add a body if a body was supplied
            if (!string.IsNullOrEmpty(body))
            {
                newEvent.Body = new ItemBody
                {
                    Content = body,
                    ContentType = BodyType.Text
                };
            }

            try
            {
                // POST /me/events
                await graphClient.Me
                    .Events
                    .Request()
                    .AddAsync(newEvent);

                Console.WriteLine("Event added to calendar.");
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error creating event: {ex.Message}");
            }
        }
        // </CreateEventSnippet>
    }
}