# Overview
gomark is a simple cross-platform exe and collection of scripts that allow you quickly bookmark locations in your file system, jump between them, or reference them from script commands.

For example:

mark appdata "C:\users\neilm\AppData\Roaming"
go appdata
ls (d appdata)
x appdata
--- show cross dos/ps/linux/explorer operation

It's invaluable if you spend a lot of time in a command shell.

The core is the gomark.exe executable.

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
Download the gomark executable and ensure it is in your path.
Download the gomark-functions.psm1 module.  (Edit aliases to taste)
Run 
 
 import-module gomark-functions.psm1 

##DOS

##WSL

sudo apt-get install dotnet-dev-1.0.1





















