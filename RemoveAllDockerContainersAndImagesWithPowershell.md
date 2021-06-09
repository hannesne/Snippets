```powershell
docker ps -a -q | %{docker rm $_}
docker image list -a -q | %{docker image rm $_ -f}
docker image prune
```
