

function jumpfs_info() {
    jumpfs.exe env 
}

function go($p) {
    $path = (jumpfs.exe find -name $p) ;
    set-location $path;
    #write-host (Get-Location).Path
}



function bmk($p) {
    $path = (jumpfs.exe find -name $p -format "%p" ) ;
    write-host $path 
}

function mark($p,$r,$l,$c) {

    jumpfs.exe mark -name $p -path $r -line $l -column $c
}


function lst($p) { jumpfs.exe list -match $p }

function codego($p) { 
 $path = (jumpfs.exe find -name $p -format "%p:%l:%c") ;
 code --goto $path
}


function x($p) { 
 $path = (jumpfs.exe find -name $p) ;
 explorer $path
}


Export-ModuleMember -Function *
