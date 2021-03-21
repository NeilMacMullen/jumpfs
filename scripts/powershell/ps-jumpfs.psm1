
### Configuration

# change to true if you want to be explicitly told about invalid bookmark names
$jumpfs_warn = $false
$jumpfs_use_standard_alias = $true


### Standard aliases - feel free to change 
if ($jumpfs_use_standard_alias) {
    Set-Alias -name mark -value jumpfs_mark
    Set-Alias -name go -value jumpfs_go
    Set-Alias -name lst -value jumpfs_list
    Set-Alias -name rmbk -value jumpfs_remove
    Set-Alias -name codego -value jumpfs_code
    Set-Alias -name x -value jumpfs_explorer_folder
    Set-Alias -name xr -value jumpfs_explorer_run
    Set-Alias -name bp -value jumpfs_value
}

## EXPERIMENTAL - expose bookmarks as a virtual drive
function jumpfs_install_drive($publish, $name) {
    #virtual drive only supported on PS 7 and later
    if ($PSVersionTable.PSVersion.Major -ge 7) {
        Import-Module   "$publish\driveProviders\DriveProvider.dll"
        New-PSDrive -Name "$name" -PSProvider "jumpfs" -Root "$($name):\" -Scope Global
    }
}

################################
# Functions: 
###############################

# jumpfs info
function jumpfs_info() {
    jumpfs.exe info 
}

function jumpfs_warn($warning) {
    if ($jumpfs_warn) {
        write-host $warning
    }
}

function jumpfs_do_or_warn_if_empty($path, $action, $warning) {
    if ($path -eq "") {
        jumpfs_warn $warning
    }
    else {
        if ($action -ne "") {
            invoke-expression $action
        }
    }
}

# create a bookmark 
function jumpfs_mark($p, $r, $l, $c) {

    jumpfs.exe mark --name $p --path $r --line $l --column $c
}

#go to a bookmark 
function jumpfs_go($p) {
    $path = (jumpfs.exe find --name $p) ;
    jumpfs_do_or_warn_if_empty $path "set-location $path"  "No bookmark '$p'"
}

# list bookmarks
function jumpfs_list($p) { jumpfs.exe list --match $p }

#open VS Code at a bookmark
function jumpfs_code($p) { 
    $path = (jumpfs.exe find --name $p --format "%p:%l:%c") ;
    jumpfs_do_or_warn_if_empty $path "code --goto $path"  "No bookmark '$p'"
 
}

# open file-explorer at bookmark
function jumpfs_explorer_folder($p) { 
    $path = (jumpfs.exe find --name $p) 
    jumpfs_do_or_warn_if_empty $path "explorer $path" "No bookmark '$p'"
}

# runs the value of a bookmark
function jumpfs_explorer_run($p) { 
    $path = (jumpfs.exe find --name $p --format "%p") ;
    jumpfs_do_or_warn_if_empty $path "explorer $path" "No bookmark '$p'"
}

# get the path of a bookmark - useful for building command lines
function jumpfs_value($p) {
    $path = (jumpfs.exe find --name $p --format "%p" ) ;
    write-host $path 
}

# get the path of a bookmark - useful for building command lines
function jumpfs_remove($p) {
    $path = (jumpfs.exe remove --name $p) ;
    jumpfs_do_or_warn_if_empty $path "" "No bookmark '$p'"
}

### ensure ffunctions and aliases are visible
Export-ModuleMember -alias * -function *

