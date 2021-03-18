# Installing for WSL 
Ensure you have [.NET 5.0 for Linux](https://dotnet.microsoft.com/download/dotnet/5.0) installed.  You only need the *.NET Runtime* unless you are planning to [build from source](doc/buildFromSource.md).

Next, [download](doc/download.md) or [build](buildFromSource.md) the tool and copy the files to a folder such as `/mnt/c/tools/jumpfs`.   

*You can share a single "installation" of jumpfs between Windows and Linux. If you do this, it's best to use a folder in the Windows file-system. **/mnt/c/** is the root of the Windows **C:** drive.

Assuming you are using Bash

Edit **~/.profile**  to include these lines...

```
#add JumpFs to path..
jumpfs_path="/mnt/c/work/tools/jumpfs"
PATH="$PATH:$jumpfs_path"
. "$jumpfs_path/bash-jumpfs.sh"
```

That's it!  Jumpfs should be available when you start your next PowerShell session.  You can confirm this by running `jumpfs_info`.


## Faster startup

When running under WSL, jumpfs needs to be told how to find the Windows APPLOCALDATA folder so that it can share the bookmarks file.  It uses the **wslvar** command to do this which appear to be quite slow and which can add to shell startup time.  

Since the value is constant, you can get slightly faster startup by determining this value on your system and hardcoding it at the beginning of the **bash-jumpfs.sh** script.




