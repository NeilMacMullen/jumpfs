# Installing for PowerShell

Ensure you have [.NET 5.0 for Windows](https://dotnet.microsoft.com/download/dotnet/5.0) installed.  You only need the *.NET Runtime* unless you are planning to [build from source](doc/buildFromSource.md).

Next, [download](doc/download.md) or [build](doc/buildFromSource.md) *jumpfs* and copy the files to a folder such as `C:\tools\jumpfs`.    

*You can share a single "installation" of jumpfs between Windows and Linux. If you do this, it's best to use a folder in the Windows file-system.*

You'll need to add a few lines to your profile startup script.  If you're not sure where to find your profile, you can look at the `$profile` variable.  Most likely you'll see something like this.

```
PS>$profile
C:\Users\neilm\Documents\PowerShell\Microsoft.PowerShell_profile.ps1
```
You'll then need to add these lines to the end of the script.  Modify the `jumpfs_path` line to point to the folder you stored the files.

```
#change this line...
$jumpfs_path = "C:\tools\jumpfs"

Import-Module "$jumpfs_path\ps-jumpfs.psm1"
$env:Path = $env:Path+";$jumpfs_path"
```


That's it!  Jumpfs should be available when you start your next PowerShell session.  You can confirm this by running `jumpfs_info`.


