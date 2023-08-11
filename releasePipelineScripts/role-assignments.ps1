Param(
    [string] [Parameter(Mandatory=$true)] $subscriptionId,
    [string] [Parameter(Mandatory=$true)] $assigneeId,
    [string] [Parameter(Mandatory=$true)] $environment
)

$RoleAssignments = @(
    @{role="User Access Administrator"; rg="nbs-appts-rg-$environment-uks"}
    @{role="Reader"; rg="covid19-booking-rg-dev-uks"}
    @{role="User Access Administrator"; rg="covid19-booking-rg-dev-uks"}
)

$RoleAssignments | ForEach-Object {
    $resourceGroupId = -join ("/subscriptions/$subscriptionId/resourceGroups/", $_.rg)
    az role assignment create --assignee $assigneeId --role $_.role --scope $resourceGroupId
}