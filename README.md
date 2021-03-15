
![GitHub release (latest by SemVer including pre-releases)](https://img.shields.io/github/downloads-pre/NeilMacmullen/jumpfs/total)
[![Coverage Status](https://coveralls.io/repos/github/NeilMacMullen/jumpfs/badge.svg?branch=main&kill_cache=1)](https://coveralls.io/github/NeilMacMullen/jumpfs?branch=main) [![Join the chat at https://gitter.im/jumpfs/community](https://badges.gitter.im/jumpfs/community.svg)](https://gitter.im/jumpfs/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)



# WORK IN PROGRESS - check back in a few days !


## Give a Star! :star:

If you like, or are using this project please give it a star - thanks!
<hr/>

**jumpfs** is a simple cross-platform exe and collection of scripts that allow you bookmark locations in your file system, jump between them, or reference them from commands.  If you spent much time at the command line you owe it to your TAB key to use this!

It's easiest to demonstrate with a picture..

![jumpfs in action](img/jumpfs.gif)

## I want it! 

Great! You can get prebuilt binaries from ... or build it yourself from source. 


Follow the instructions to
- [install for PowerShell](doc/powershell-installation.md)
- [install for WSL or Linux](doc/linux-installation.md)
- [install for DOS/CMD](doc/cmd-installation.md)

## Basic use

The main commands are listed below but you can create your own quite easily by reading the [advanced usage](doc/advanced.md) guide.

### mark - create a bookmark
**mark** can take up to 4 arguments.  The first is the name you want to use for the bookmark.  The remainder are *path*, *line-number* and *column-number*.  The latter two are useful when specifying a bookmark to a particular position within a file.

 - `mark name` creates a bookmark at the current working directory
 - `mark name path` creates a bookmark at the supplied folder or file
 - `mark name path 10 5` creates a bookmark with line number and column

### go - go to a bookmark
**go** only takes a single argument which is the name of the bookmark.  Note that if you *go* to a file, you will actually be taken to the folder that contains it.

### lst - list bookmarks
If no arguments are supplied, **lst** will display all stored bookmarks.  If an argument is given, it is used to search within the bookmark names and paths and only those that match are returned.

### codego - open Visual Studio Code at the bookmark

If the bookmark supplied to **codego** is a file and has a line and column associated with it the file will be opened at that position.

### x - open Windows File Explorer at the bookmark

Opens Windows File Explorer at the location of the bookmark (or the containing folder if the bookmark is a file).

## Manually hacking bookmarks 
Bookmarks are stored in a fairly self-explanatory JSON format.  The easiest way to find it is to just run 
```jumpfs.exe env```




## Contributions
PRs are welcome.  Particularly to documentation and Linux-side scripts!  Please read the
[contributors guide](doc/Contributions.md)

Shell extension
VS Code extension
Visual bookmark editor


















