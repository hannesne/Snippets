The WSL backend sometimes loses time when the computer goes to sleep. Use this command on hte host in Powershell to reset it:
`wsl -d docker-desktop hwclock -s`
