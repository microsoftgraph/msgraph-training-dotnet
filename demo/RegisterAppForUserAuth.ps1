# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT license.

# <ScriptBody>
param(
  [Parameter(Mandatory=$true,
  HelpMessage="The friendly name of the app registration")]
  [String]
  $AppName,

  [Parameter(Mandatory=$false,
  HelpMessage="The sign in audience for the app")]
  [ValidateSet("AzureADMyOrg", "AzureADMultipleOrgs", `
  "AzureADandPersonalMicrosoftAccount", "PersonalMicrosoftAccount")]
  [String]
  $SignInAudience = "AzureADandPersonalMicrosoftAccount",

  [Parameter(Mandatory=$false)]
  [Switch]
  $StayConnected = $false
)

# Tenant to use in authentication.
# See https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-device-code#device-authorization-request
$authTenant = switch ($SignInAudience)
{
  "AzureADMyOrg" { "tenantId" }
  "AzureADMultipleOrgs" { "organizations" }
  "AzureADandPersonalMicrosoftAccount" { "common" }
  "PersonalMicrosoftAccount" { "consumers" }
  default { "invalid" }
}

if ($authTenant -eq "invalid")
{
  Write-Host -ForegroundColor Red "Invalid sign in audience:" $SignInAudience
  Exit
}

# Requires an admin
Connect-MgGraph -Scopes "Application.ReadWrite.All User.Read" -UseDeviceAuthentication -ErrorAction Stop

# Get context for access to tenant ID
$context = Get-MgContext -ErrorAction Stop

if ($authTenant -eq "tenantId")
{
  $authTenant = $context.TenantId
}

# Create app registration
$appRegistration = New-MgApplication -DisplayName $AppName -SignInAudience $SignInAudience `
 -IsFallbackPublicClient -ErrorAction Stop
Write-Host -ForegroundColor Cyan "App registration created with app ID" $appRegistration.AppId

# Create corresponding service principal
if ($SignInAudience -ne "PersonalMicrosoftAccount")
{
  New-MgServicePrincipal -AppId $appRegistration.AppId -ErrorAction SilentlyContinue `
   -ErrorVariable SPError | Out-Null
  if ($SPError)
  {
    Write-Host -ForegroundColor Red "A service principal for the app could not be created."
    Write-Host -ForegroundColor Red $SPError
    Exit
  }

  Write-Host -ForegroundColor Cyan "Service principal created"
}

Write-Host
Write-Host -ForegroundColor Green "SUCCESS"
Write-Host -ForegroundColor Cyan -NoNewline "Client ID: "
Write-Host -ForegroundColor Yellow $appRegistration.AppId
Write-Host -ForegroundColor Cyan -NoNewline "Auth tenant: "
Write-Host -ForegroundColor Yellow $authTenant

if ($StayConnected -eq $false)
{
  Disconnect-MgGraph
  Write-Host "Disconnected from Microsoft Graph"
}
else
{
  Write-Host
  Write-Host -ForegroundColor Yellow `
   "The connection to Microsoft Graph is still active. To disconnect, use Disconnect-MgGraph"
}
# </ScriptBody>
