Run an executable whose path has spaces in PowerShell
====

Running an executable from a directory whose path has spaces in it is not straightforward in PowerShell.
For example, the command below will not work since PowerShell thinks that it is a string because it is quoted:

```powershell
"C:\Program Files (x86)\DjVuZone\DjVuLibre\djvm.exe"
```

To run this executable, PowerShell needs to be instructed explicitly to execute the string that it is given. This is done using the function call operator (&):

```powershell
& "C:\Program Files (x86)\DjVuZone\DjVuLibre\djvm.exe"
```
