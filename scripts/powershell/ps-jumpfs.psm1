

# jumpfs info
function jumpfs_info() {
    jumpfs.exe info 
}

# create a bookmark 
function mark($p,$r,$l,$c) {

    jumpfs.exe mark --name $p --path $r --line $l --column $c
}

#go to a bookmark 
function go($p) {
    $path = (jumpfs.exe find --name $p) ;
    set-location $path;
    #write-host (Get-Location).Path
}

# list bookmarks
function lst($p) { jumpfs.exe list --match $p }

#open VS Code at a bookmark
function codego($p) { 
 $path = (jumpfs.exe find --name $p --format "%p:%l:%c") ;
 code --goto $path
}

# open file-explorer at bookmark
function x($p) { 
 $path = (jumpfs.exe find --name $p) ;
 explorer $path
}

# get the path of a bookmark - useful for building command lines
function bp($p) {
    $path = (jumpfs.exe find --name $p --format "%p" ) ;
    write-host $path 
}

Export-ModuleMember -Function *
