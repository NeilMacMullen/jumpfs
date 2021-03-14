

function default_to_current_location($r)
{
if ($r -eq $null)
{
    $r = (Get-Location).Path
}
$r
}

function go($p) {
    $path = (jumpfs.exe find -name $p) ;
    set-location $path;
    write-host (Get-Location).Path
}

function mark($p,$r) {

    jumpfs.exe mark -name $p -path (default_to_current_location $r)
}


function lst($p) { jumpfs.exe list -name $p }

function gol($p,$r) { jumpfs.exe findrelative -path $p -container (default_to_current_location $r) }

function edit($p) { 
 $path = (jumpfs.exe find -name $p) ;
 code --new-window -g $path
}


function x($p) { 
 $path = (jumpfs.exe find -name $p) ;
 explorer $path
}


Export-ModuleMember -Function *