Write-Host "Setting up event log"
$log = "Application"
$source = "DockerFixer"
if ([System.Diagnostics.EventLog]::SourceExists($source) -eq $false) {
    Write-Host "Creating event source $source on event log $log"
    [System.Diagnostics.EventLog]::CreateEventSource($source, $log)
    Write-Host -foregroundcolor green "Event source $source created"
}

Write-Host "toggling time sync for wsl: 'wsl -d docker-desktop hwclock -s'"
# Get-VMIntegrationService -VMName DockerDesktopVM -Name "Time Synchronization" | Disable-VMIntegrationService
# Get-VMIntegrationService -VMName DockerDesktopVM -Name "Time Synchronization" | Enable-VMIntegrationService

wsl -d docker-desktop hwclock -s

if ($?) {
    Write-EventLog -LogName $log -Source $source -EventID 1 -EntryType Information -Message "Toggled time sync on wsl." -Category 1
}
else {
    $message = "Failed to toggle time sync on docker vm: " + $Error[0].Exception.Message
    Write-EventLog -LogName $log -Source $source -EventID 1 -EntryType Error -Message $message -Category 1
    Write-Host $message
    pause
}