// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

// <GraphHelperSnippet>
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;

class GraphHelper
{
    // User auth token credential
    private static DeviceCodeCredential? _deviceCodeCredential;
    // Client configured with user authentication
    private static GraphServiceClient? _userClient;
    // </GraphHelperSnippet>

    // App-ony auth token credential
    private static ClientSecretCredential? _clientSecretCredential;
    // Client configured with app-only authentication
    private static GraphServiceClient? _appClient;

    public static void InitializeGraphForUserAuth(Settings settings,
        Func<DeviceCodeInfo, CancellationToken, Task> deviceCodePrompt)
    {
        _deviceCodeCredential = new DeviceCodeCredential(deviceCodePrompt,
            settings.AuthTenant, settings.ClientId);

        _userClient = new GraphServiceClient(_deviceCodeCredential, settings.GraphUserScopes);
    }

    public static async Task<string> GetUserTokenAsync(string[]? scopes)
    {
        // Ensure credential isn't null
        _ = _deviceCodeCredential ??
            throw new System.NullReferenceException("Graph has not been initialized for user auth");

        // Ensure scopes isn't null
        _ = scopes ?? throw new System.ArgumentNullException("Argument 'scopes' cannot be null");

        // Request token with given scopes
        var context = new TokenRequestContext(scopes);
        var response = await _deviceCodeCredential.GetTokenAsync(context);
        return response.Token;
    }

    public static Task<User> GetUserAsync()
    {
        // Ensure client isn't null
        _ = _userClient ??
            throw new System.NullReferenceException("Graph has not been initialized for user auth");

        return _userClient.Me
            .Request()
            .Select(u => new
            {
                // Only request the user's display name
                u.DisplayName
            })
            .GetAsync();
    }
// <GraphHelperSnippet>
}
// </GraphHelperSnippet>
