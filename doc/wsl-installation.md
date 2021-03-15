# Installing for WSL

Ensure you have dotnet 5.0 installed

Assuming you are using Bash

Edit your .profile in  to include these lines...

```
#add JumpFs to path..
jumpfs_path="/mnt/d/work/jumpfs/publish"
PATH="$PATH:$jumpfs_path"
. "$jumpfs_path/bash-jumpfs.sh"
```

*Optionally* 
You may wish to change the function names


That's it!


