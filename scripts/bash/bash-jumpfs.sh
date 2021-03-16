#!/bin/bash

#this should already be on the path
JumpFsExe="jumpfs"

# JumpFs needs to know where to look for the bookmarks file
# wslvar appears to be quite slow so you can speed up the startup
# by hardcoding the result rather than looking it up on every startup
# For 'bare' LINUX, you should just set this to a folder that will be used 
# to contain the bookmark file
loc="$(wslvar LOCALAPPDATA)"
JUMPFS_FOLDER="$(wslpath $loc)"
export JUMPFS_FOLDER

# JumpFs needs to know the UNC path for the root of this WSL installation
# to allow access from windows.  This is a fast operation and not worth
# skipping
# For 'bare' LINUX you should just set this to "/"
JUMPFS_WSL_ROOT="$(wslpath -w /)"
export JUMPFS_WSL_ROOT

#Feel free to change the names of these to suit....


mark() {
      `$JumpFsExe mark --name $1 --path $2 --line $3 --column $4`
}

go() {
    d=`$JumpFsExe find --name $1`
    cd $d
}


lst() {
    matches=`$JumpFsExe list --match $1`
    echo "$matches"
}



jumpfs_info() {
    info=`$JumpFsExe info`
    echo "$info"
}


codego() {
    d=`$JumpFsExe find --name $1 --format %p:%l:%c`
    `code --goto "$d"`
}



bmk() {
   d=`$JumpFsExe find --name $1 --format %p`
   echo "$d"
}


x() {
   d=`$JumpFsExe find --name $1 --winpath`
   explorer.exe "$d"
}

