# Installing for PowerShell

Ensure you have [.NET 7.0 for Windows](https://dotnet.microsoft.com/download/dotnet/7.0) installed.  You only need the *.NET Runtime* unless you are planning to [build from source](buildFromSource.md).

Next, [download](download.md) or [build](buildFromSource.md) *jumpfs* and copy the files to a folder such as `C:\tools\jumpfs`.    

*You can share a single "installation" of jumpfs between Windows and Linux. If you do this, it's best to use a folder in the Windows file-system.*

You'll need to add a few lines to your profile startup script.  If you're not sure where to find your profile, you can look at the `$profile` variable.  Most likely you'll see something like this.



```
PS> $profile
C:\Users\neilm\Documents\PowerShell\Microsoft.PowerShell_profile.ps1
```

** IMPORTANT ** the ps-jumpfs.psm1 module is not signed so PowerShell will refuse to execute it unless using an Unrestricted execution policy (not advisable!). The easiest workaround is to unblock the file manually (after you've reviewed the contents of course!) ...

```
PS> unblock-file C:\tools\jumpfs\ps-jumpfs.psm1
```

You *may* also have to unblock the *jumpfs/driverProviders/DriveProvider.dll* file although that hasn't been necessary in my testing.

You'll then need to add these lines to the end of the script.  Modify the `jumpfs_path` line to point to the folder you stored the files.

```
# change this line...
$jumpfs_path = "C:\tools\jumpfs"
$env:Path = $env:Path+";$jumpfs_path"
Import-Module "$jumpfs_path\ps-jumpfs.psm1"

# OPTIONAL - install virtual drive (PS 7.x only, has no effect on earlier versions)
jumpfs_install_drive $jumpfs_path "jfs"
```


That's it!  Jumpfs should be available when you start your next PowerShell session.  You can confirm this by running `jumpfs_info`.

IF you chose to install the [virtual drive](doc/../psdrive.md) you can list bookmarks with `ls jfs:`



