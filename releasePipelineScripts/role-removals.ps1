Param(
    [string] [Parameter(Mandatory=$true)] $subscription,
    [string] [Parameter(Mandatory=$true)] $assigneeId
)

$RoleRemovals = @(
    @{role="User Access Administrator"; rg="nbs-appts-rg-exp-uks"}
    @{role="Reader"; rg="covid19-booking-rg-dev-uks"}
    @{role="User Access Administrator"; rg="covid19-booking-rg-dev-uks"}
)

$RoleRemovals | ForEach-Object {
    $resourceGroupId = -join ("/subscriptions/$subscription/resourceGroups/", $_.rg)
    az role assignment delete --assignee $assigneeId --role $_.role --scope $resourceGroupId
}