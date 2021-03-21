# PowerShell virtual drive (PowerShell 7.x only)

jumpfs comes with a virtual drive which is installed with the `jumpfs_install_drive $jumpfs_path "jfs"` command in your profile script.  You are free to change the name to something other than "jfs" - a single letter such as "b" makes it a little quicker to get around!

It can be used in two different ways.

## As a bookmark container

```

# list bookmarks 
ls jfs: 

# get all the paths 
ls jfs: | % Path

# a clumsy way of performing 'go example'
cd (get-item jfs:example).Path
```
## As a content container

By using the `$` variable prefix, the drive can be coerced to return the Path member for a bookmark

```
# show contents of the file pointed to by the bookmark 'myfile'
get-content $jfs:myfile

```
## Drive features

Tab-completion  and standard PS wild-cards (`*,?,[]`) are supported.

## Restrictions
It is possible to `remove-item` a bookmark via the drive but not yet to add or edit items
