// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// <SettingsSnippet>
using Microsoft.Extensions.Configuration;

public class AppOnlySettings
{
    public bool EnableAppOnly { get; set; }
    public string? ClientSecret { get; set; }
    public string? TenantId { get; set; }
}

public class Settings
{
    public string? ClientId { get; set; }
    public string? AuthTenant { get; set; }
    public string[]? GraphUserScopes { get; set; }
    public AppOnlySettings? AppOnly { get; set; }

    public static Settings LoadSettings()
    {
        // Load settings
        IConfiguration config = new ConfigurationBuilder()
            // appsettings.json is required
            .AddJsonFile("appsettings.json", optional: false)
            // appsettings.Development.json" is optional, values override appsettings.json
            .AddJsonFile($"appsettings.Development.json", optional: true)
            // User secrets are optional, values override both JSON files
            .AddUserSecrets<Program>()
            .Build();

        return config.GetRequiredSection("Settings").Get<Settings>();
    }
}
// </SettingsSnippet>
