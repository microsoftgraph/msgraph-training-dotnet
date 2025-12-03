// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace GraphAppOnlyTutorial;

public class GraphHelper
{
    // <AppOnlyAuthConfigSnippet>
    // Settings object
    private static Settings? settings;

    // App-ony auth token credential
    private static ClientSecretCredential? clientSecretCredential;

    // Client configured with app-only authentication
    private static GraphServiceClient? appClient;

    public static void InitializeGraphForAppOnlyAuth(Settings settings)
    {
        GraphHelper.settings = settings;

        // Ensure settings isn't null
        _ = settings ??
            throw new NullReferenceException("Settings cannot be null");

        GraphHelper.settings = settings;

        clientSecretCredential ??= new ClientSecretCredential(
                GraphHelper.settings.TenantId, GraphHelper.settings.ClientId, GraphHelper.settings.ClientSecret);

        appClient ??= new GraphServiceClient(
                clientSecretCredential,
                /* Use the default scope, which will request the scopes
                   configured on the app registration */
                ["https://graph.microsoft.com/.default"]);
    }
    // </AppOnlyAuthConfigSnippet>

    // <GetAppOnlyTokenSnippet>
    public static async Task<string> GetAppOnlyTokenAsync()
    {
        // Ensure credential isn't null
        _ = clientSecretCredential ??
            throw new NullReferenceException("Graph has not been initialized for app-only auth");

        // Request token with given scopes
        var context = new TokenRequestContext(["https://graph.microsoft.com/.default"]);
        var response = await clientSecretCredential.GetTokenAsync(context);
        return response.Token;
    }
    // </GetAppOnlyTokenSnippet>

    // <GetUsersSnippet>
    public static Task<UserCollectionResponse?> GetUsersAsync()
    {
        // Ensure client isn't null
        _ = appClient ??
            throw new NullReferenceException("Graph has not been initialized for app-only auth");

        return appClient.Users.GetAsync((config) =>
        {
            /* Only request specific properties */
            config.QueryParameters.Select = ["displayName", "id", "mail"];
            /* Get at most 25 results */
            config.QueryParameters.Top = 25;
            /* Sort by display name */
            config.QueryParameters.Orderby = ["displayName"];
        });
    }
    // </GetUsersSnippet>

#pragma warning disable CS1998
    // <MakeGraphCallSnippet>
    /* This function serves as a playground for testing Graph snippets
       or other code */
    public static async Task MakeGraphCallAsync()
    {
        // INSERT YOUR CODE HERE
    }
    // </MakeGraphCallSnippet>
}
