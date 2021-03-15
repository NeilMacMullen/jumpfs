# Installing for PowerShell

Ensure you have dotnet 5.0 installed

Edit your profile in $env:ProfileDir to add these lines...

```
#change this line...
$jumpfs_path = "D:\work\jumpfs\publish"

Import-Module "$jumpfs_path\ps-jumpfs.psm1"
$env:Path = $env:Path+";$jumpfs_path"
```

Make sure you edit the `jumpfs_path` variable to point to the folder you extracted to.


That's it!


