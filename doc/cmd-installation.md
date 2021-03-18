# Installing for DOS/CMD

Ensure you have [.NET 5.0 for Windows](https://dotnet.microsoft.com/download/dotnet/5.0) installed.  You only need the *.NET Runtime* unless you are planning to [build from source](buildFromSource.md).

Next, [download](download.md) or [build](buildFromSource.md) the tool and copy the files to a folder such as `C:\tools\jumpfs`.    

You'll need to ensure that the folder is on the PATH.  Add this to your autoexec.bat file or change it in system settings....

```
@PATH=%PATH%;c:\tools\jumpfs
```

That's it!  Jumpfs should be available when you start your next CMD session.  You can confirm this by running `jumpfs_info`.




