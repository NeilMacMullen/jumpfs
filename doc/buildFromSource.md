# Building from source

You can build jumpfs from either Windows or Linux; if you are intending to use it from both environments on the same machine, you only need to build it once.


First, ensure that you have the [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) installed.


If you want to build the *bleeding-edge* checkout *main*

`git checkout main`

If you want to build one of the stable releases, checkout one of the release tags

`git checkout tags/v1.2.0`

## PowerShell
Run the **build.ps** script in the top *jumps* folder.

Note that if you already have the [virtual drive](psdrive.md) installed, the build will fail. You will need to close down all sessions that have the virtual drive loaded and then build from a shell that doesn't load it at startup. This may include VS-Code terminal sessions.

In practice, the easiest way to do this is to build from an older shell such as PowerShell 5.

## Linux
Run the **build.sh** script in the top *jumps* folder.
*Note that it is currently not possible to build the Windows executable from Linux.  If you use both PowerShell and Linux, you should build from PowerShell* 


## Artefacts

Binaries and scripts are available in the *publish* folder.



