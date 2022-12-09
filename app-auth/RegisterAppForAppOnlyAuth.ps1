# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT license.

# <ScriptBody>
param(
  [Parameter(Mandatory=$true,
  HelpMessage="The friendly name of the app registration")]
  [String]
  $AppName,

  [Parameter(Mandatory=$true,
  HelpMessage="The application permission scopes to configure on the app registration")]
  [String[]]
  $GraphScopes,

  [Parameter(Mandatory=$false)]
  [Switch]
  $StayConnected = $false
)

$graphAppId = "00000003-0000-0000-c000-000000000000"

# Requires an admin
Connect-MgGraph -Scopes "Application.ReadWrite.All User.Read" -UseDeviceAuthentication -ErrorAction Stop

# Get context for access to tenant ID
$context = Get-MgContext -ErrorAction Stop
$authTenant = $context.TenantId

# Create app registration
$appRegistration = New-MgApplication -DisplayName $AppName -SignInAudience "AzureADMyOrg" -ErrorAction Stop
Write-Host -ForegroundColor Cyan "App registration created with app ID" $appRegistration.AppId

# Create corresponding service principal
$appServicePrincipal = New-MgServicePrincipal -AppId $appRegistration.AppId -ErrorAction SilentlyContinue `
  -ErrorVariable SPError
if ($SPError)
{
  Write-Host -ForegroundColor Red "A service principal for the app could not be created."
  Write-Host -ForegroundColor Red $SPError
  Exit
}

Write-Host -ForegroundColor Cyan "Service principal created"

# Lookup available Graph application permissions
$graphServicePrincipal = Get-MgServicePrincipal -Filter ("appId eq '" + $graphAppId + "'") -ErrorAction Stop
$graphAppPermissions = $graphServicePrincipal.AppRoles

$resourceAccess = @()

foreach($scope in $GraphScopes)
{
  $permission = $graphAppPermissions | Where-Object { $_.Value -eq $scope }
  if ($permission)
  {
    $resourceAccess += @{ Id =  $permission.Id; Type = "Role"}
  }
  else
  {
    Write-Host -ForegroundColor Red "Invalid scope:" $scope
    Exit
  }
}

# Add the permissions to required resource access
Update-MgApplication -ApplicationId $appRegistration.Id -RequiredResourceAccess `
 @{ ResourceAppId = $graphAppId; ResourceAccess = $resourceAccess } -ErrorAction Stop
Write-Host -ForegroundColor Cyan "Added application permissions to app registration"

# Add admin consent
foreach ($appRole in $resourceAccess)
{
  $appServicePrincipal
  New-MgServicePrincipalAppRoleAssignment -ServicePrincipalId $appServicePrincipal.Id `
   -PrincipalId $appServicePrincipal.Id -ResourceId $graphServicePrincipal.Id `
   -AppRoleId $appRole.Id -ErrorAction SilentlyContinue -ErrorVariable SPError | Out-Null
  if ($SPError)
  {
    Write-Host -ForegroundColor Red "Admin consent for one of the requested scopes could not be added."
    Write-Host -ForegroundColor Red $SPError
    Exit
  }
}
Write-Host -ForegroundColor Cyan "Added admin consent"

# Add a client secret
$clientSecret = Add-MgApplicationPassword -ApplicationId $appRegistration.Id -PasswordCredential `
 @{ DisplayName = "Added by PowerShell" } -ErrorAction Stop

Write-Host
Write-Host -ForegroundColor Green "SUCCESS"
Write-Host -ForegroundColor Cyan -NoNewline "Client ID: "
Write-Host -ForegroundColor Yellow $appRegistration.AppId
Write-Host -ForegroundColor Cyan -NoNewline "Tenant ID: "
Write-Host -ForegroundColor Yellow $authTenant
Write-Host -ForegroundColor Cyan -NoNewline "Client secret: "
Write-Host -ForegroundColor Yellow $clientSecret.SecretText
Write-Host -ForegroundColor Cyan -NoNewline "Secret expires: "
Write-Host -ForegroundColor Yellow $clientSecret.EndDateTime

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
