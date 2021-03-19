#!/bin/bash

# configuration

# use standard script aliases.  If you want to set up custom ones
# you can  edit those below
jumpfs_use_standard_alias="1"

### Standard aliases - feel free to change
if [ "$jumpfs_use_standard_alias" -eq "1" ] ;
then
    alias mark="jumpfs_mark"
    alias go="jumpfs_go"
    alias lst="jumpfs_list"
    alias rmbk="jumpfs_remove"
    alias codego="jumpfs_code"
    alias x="jumpfs_explorer_folder"
    alias xr="jumpfs_explorer_run"
    alias bp="jumpfs_value"
fi


# this should already be on the path
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


### Functions #######################


# jumpfs info
jumpfs_info() {
    info=`$JumpFsExe info`
    echo "$info"
}


# create a bookmark
jumpfs_mark() {
    `$JumpFsExe mark --name $1 --path $2 --line $3 --column $4`
}

#go to a bookmark
jumpfs_go() {
    d=`$JumpFsExe find --name $1`
    if [ -n "$d" ]; then
        cd $d
    fi
}

# list bookmarks
jumpfs_list() {
    matches=`$JumpFsExe list --match $1`
    echo "$matches"
}

# list bookmarks
jumpfs_remove() {
    `$JumpFsExe remove --name $1`
}

#open VS Code at a bookmark
jumpfs_code() {
    d=`$JumpFsExe find --name $1 --format %p:%l:%c`
    if [ -n "$d" ]; then
        `code --goto "$d"`
    fi
}

# open file-explorer at bookmark
jumpfs_explorer_folder() {
    d=`$JumpFsExe find --name $1 --winpath`
    if [ -n "$d" ]; then
        explorer.exe "$d"
    fi
}


# open file-explorer at bookmark
jumpfs_explorer_run() {
    d=`$JumpFsExe find --name $1 --format %p --winpath`
    if [ -n "$d" ]; then
        explorer.exe "$d"
    fi
}

# get the path of a bookmark - useful for building command lines
jumpfs_value() {
    d=`$JumpFsExe find --name $1 --format %p`
    echo "$d"
}


