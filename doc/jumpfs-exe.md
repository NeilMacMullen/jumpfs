# Jumpfs commands and parameters

Jumpfs is normally called via scripts.  If you want to add a custom command you'll need to know about the commands and parameters.

The basic syntax is 

```
jumpfs *command* [--*parameter* [*value*]] 
```

It's generally ok to omit the value for a parameter - this makes it easier to write scripts.  For example, this is a perfectly acceptable way of creating a bookmark in the current directory

```
jumpfs mark --name h --path --line --column
```

## mark 
The mark command creates or replaces a bookmark.
### --name
Required.  The name of the bookmark
### --path 
Optional path to the bookmark (defaults to current working directory)
### --line 
Optional line (defaults to 0)
### --columns 
Optional column (defaults to 0)

## find
The find command locates a bookmark and returns its value in a specified format
### --name
Required.  The name of the bookmark
### --winpath 
A flag which forces the path to be returned in Windows format which can be useful if trying to pass paths to Windows executables from within WSL.
### --format 
Optional format descriptor (defaults to "%f")
- %f expands to the bookmark value when a folder or the containing folder when a file
- %p - expands to the full bookmark path
  %l - expands to the bookmark line number 
  %c - expands to the bookmark column number
  %N - expands to the a newline
  %D - expands to the drive letter for a windows path
Format specifiers can be combined. E.g. `--format "%p@(%l,%c)"`

## list
The list command lists all bookmarks that match the specified criteria
### --match
Optional match string.  If supplied, all bookmarks whose name or path contain the substring will be displayed, otherwise all stored bookmarks will be displayed

## info
The info command displays version and environmental information and will check for updates.

## showargs
The showargs command simply echoes back a list of arguments that have been supplied. This can be useful when debugging script interpolation issues.




