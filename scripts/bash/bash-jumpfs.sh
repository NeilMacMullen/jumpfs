#!/bin/bash

# Either ensure that jumpfs is on your path or modify this 

JumpFsExe="/mnt/d/work/jumpfs/publish/jumpfs"
# JumpFs needs to know where to look for the bookmarks file
loc="$(wslvar LOCALAPPDATA)"
JUMPFS_FOLDER="$(wslpath $loc)"
export JUMPFS_FOLDER

# JumpFs needs to know the UNC path for the root of this WSL installation
# to allow access from windows
JUMPFS_WSL_ROOT="$(wslpath -w /)"
export JUMPFS_WSL_ROOT

#functions feel free to change the names of these to suit....
go() {
    d=`$JumpFsExe find -name $1`
    cd $d
}


codego() {
    d=`$JumpFsExe find -name $1 -format %p:%l:%c`
    `code --goto "$d"`
}

mark() {
      `$JumpFsExe mark -name $1 -path $2 -line $3 -column $4`
}

lst() {
    matches=`$JumpFsExe list -match $1`
    echo "$matches"
}

bmk() {
   d=`$JumpFsExe find -name $1 -format %p`
   echo "$d"
}


x() {
   d=`$JumpFsExe find -name $1 -winpath`
   explorer.exe "$d"
}



jpenv() {
    jumpfsenv=`$JumpFsExe env`
    echo "$jumpfsenv"
}

