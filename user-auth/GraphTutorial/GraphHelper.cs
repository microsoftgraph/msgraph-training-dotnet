// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Me.SendMail;
using Microsoft.Graph.Models;

namespace GraphTutorial;

public class GraphHelper
{
    // <UserAuthConfigSnippet>
    // Settings object
    private static Settings? settings;

    // User auth token credential
    private static DeviceCodeCredential? deviceCodeCredential;

    // Client configured with user authentication
    private static GraphServiceClient? userClient;

    public static void InitializeGraphForUserAuth(
        Settings settings,
        Func<DeviceCodeInfo, CancellationToken, Task> deviceCodePrompt)
    {
        GraphHelper.settings = settings;

        var options = new DeviceCodeCredentialOptions
        {
            ClientId = settings.ClientId,
            TenantId = settings.TenantId,
            DeviceCodeCallback = deviceCodePrompt,
        };

        deviceCodeCredential = new DeviceCodeCredential(options);

        userClient = new GraphServiceClient(deviceCodeCredential, settings.GraphUserScopes);
    }
    // </UserAuthConfigSnippet>

    // <GetUserTokenSnippet>
    public static async Task<string> GetUserTokenAsync()
    {
        // Ensure credential isn't null
        _ = deviceCodeCredential ??
            throw new NullReferenceException("Graph has not been initialized for user auth");

        // Ensure scopes isn't null
        _ = settings?.GraphUserScopes ?? throw new ArgumentNullException("Argument 'scopes' cannot be null");

        // Request token with given scopes
        var context = new TokenRequestContext(settings.GraphUserScopes);
        var response = await deviceCodeCredential.GetTokenAsync(context);
        return response.Token;
    }
    // </GetUserTokenSnippet>

    // <GetUserSnippet>
    public static Task<User?> GetUserAsync()
    {
        // Ensure client isn't null
        _ = userClient ??
            throw new NullReferenceException("Graph has not been initialized for user auth");

        return userClient.Me.GetAsync((config) =>
        {
            // Only request specific properties
            config.QueryParameters.Select = ["displayName", "mail", "userPrincipalName"];
        });
    }
    // </GetUserSnippet>

    // <GetInboxSnippet>
    public static Task<MessageCollectionResponse?> GetInboxAsync()
    {
        // Ensure client isn't null
        _ = userClient ??
            throw new NullReferenceException("Graph has not been initialized for user auth");

        return userClient.Me
            .MailFolders["Inbox"] // Only messages from Inbox folder
            .Messages
            .GetAsync((config) =>
            {
                /* Only request specific properties */
                config.QueryParameters.Select = ["from", "isRead", "receivedDateTime", "subject"];
                /* Get at most 25 results */
                config.QueryParameters.Top = 25;
                /* Sort by received time, newest first */
                config.QueryParameters.Orderby = ["receivedDateTime DESC"];
            });
    }
    // </GetInboxSnippet>

    // <SendMailSnippet>
    public static async Task SendMailAsync(string subject, string body, string recipient)
    {
        // Ensure client isn't null
        _ = userClient ??
            throw new NullReferenceException("Graph has not been initialized for user auth");

        // Create a new message
        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                Content = body,
                ContentType = BodyType.Text,
            },
            ToRecipients =
            [
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = recipient,
                    },
                },
            ],
        };

        // Send the message
        await userClient.Me
            .SendMail
            .PostAsync(new SendMailPostRequestBody
            {
                Message = message,
            });
    }
    // </SendMailSnippet>

#pragma warning disable CS1998
    // <MakeGraphCallSnippet>
    /* This function serves as a playground for testing Graph snippets */
    /* or other code */
    public static async Task MakeGraphCallAsync()
    {
        // INSERT YOUR CODE HERE
    }
    // </MakeGraphCallSnippet>
}
