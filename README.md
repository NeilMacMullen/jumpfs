
![GitHub release (latest by SemVer including pre-releases)](https://img.shields.io/github/downloads-pre/NeilMacmullen/jumpfs/total)
[![Coverage Status](https://coveralls.io/repos/github/NeilMacMullen/jumpfs/badge.svg?branch=main&kill_cache=1)](https://coveralls.io/github/NeilMacMullen/jumpfs?branch=main) [![Join the chat at https://gitter.im/jumpfs/community](https://badges.gitter.im/jumpfs/community.svg)](https://gitter.im/jumpfs/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)



# WORK IN PROGRESS - check back in a few days !


## Give a Star! :star:

If you like or are using this project please give it a star - thanks!

jumpfs is a simple cross-platform exe and collection of scripts that allow you quickly bookmark locations in your file system, jump between them, or reference them from script commands.

It's easiest to demonstration with picture..

![jumpfs in action](img/jumpfs.gif)

..Term with WSL, PowerShell, DOS

PS: mark pshome
DOS: go pshome
WSL: go pshome
WSL cd ~
    mark wslhome
DOS:lst ubu
x wslhome
PS mark  scr \jumpfs\publish\....psm 10 10
WSL: codeat scr
     go scr






For example:

mark appdata "C:\users\neilm\AppData\Roaming"
go appdata
ls (d appdata)
x appdata
--- show cross dos/ps/linux/explorer operation

It's invaluable if you spend a lot of time in a command shell.

The core is the jumpfs.exe executable.

Wrapper scripts are provided for...
- Powershell
- DOS 
- BASH

Basic Usage

## mark name [path] 
Create or overwrite a bookmark at the specified path.  If the path is omitted, the current working directory is used.

## go name 
Go to the path bookmarked by the specified name.

## lst match
List all bookmarks where either the name or path contain the provided match string

## x name
(Windows) opens a file-explorer window at the bookmark.

##gol name [parent]
Finds the "best" match for a bookmark. For example if you have bookmarked ....

##edit name
Opens Visual Studio Code


#Data
The Bookmarks file is stored as JSON at .....


#Installation

##PowerShell
Download the jumpfs executable and ensure it is in your path.
Download the jumpfs-functions.psm1 module.  (Edit aliases to taste)
Run 
 
 import-module jumpfs-functions.psm1 

##DOS

##WSL

sudo apt-get install dotnet-dev-1.0.1

##Contributions
Contributions are welcome.  Particularly to documentation and Linux-side scripts!  Please read the
[contributors guide](doc/Contributions.md)

Shell extension
VS Code extension

Bookmark format is open - easy to add your own editors or client apps



















