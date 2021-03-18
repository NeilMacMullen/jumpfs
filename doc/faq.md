# FAQ

## Why not just use [ZLocation](https://github.com/vors/ZLocation)  ?

I wasn't aware of this when writing jumpfs. It looks great! If your day-to-day usage is just getting around folders in PowerShell it's probably a better solution.

There are some differences of approach between ZLocation and jumps

- ZLocation requires you to use it as a replacement to cd and stores a searchable history of all the locations you've been.  jumpfs is a  bookmark system.  (A long, long time ago it was a set of Perl scripts to allow me to share Emacs bookmarks with the bash and cmd shells.)

- ZLocation is pure powershell, hence adds nice features such as tab-completion and simple installation.  jumpfs uses a common .net exe 'engine' with simple PS/Bash functions/BAT scripts as the front end, does not (yet) offer tab-completion and requires a bit of profile fiddling for each shell you want to use it in. Jumpfs is conceived as a family of related scripts and utilities; hence it includes simple functions to open a code-editor or file-explorer and may conceivable grow to include a shell-extension and VS Code plugin.

- zlocation is aimed at getting around folders.  jumpfs allows you get around folders and files. I.e you can bookmark a line within a file and either 'go' to it (in which case you cd to the containing folder) or 'codego' to it to open your editor at that line. Jumpfs *may* (no promises) also be able to bookmark other things in future such as URLs. 

- jumpfs is aimed at a problem I suspect many developers and sysadmins face since the introduction of WSL; i.e. being able to get around in both filesystems on the same box and refer to the same items regardless of shell.  I.e. you can bookmark a folder or file in WSL then get to it easily in PowerShell or vice-versa

As far as I can see, you could use both ZLocation and jumps together (and I probably will).

### Wouldn't it be better if it was 100% PowerShell?

Yes, that would certainly be better if I didn't want it to support Bash and Cmd bookmarking.  That requires some kind of common backend (even if just an agreed file format) and being a C# developer it's easiest to do that in C# and keep the script layering thin.  Other functional partitioning would certainly be possible and might even be superior but this is the current one.

### Why do some of the command names clash with things I have installed

*go* and *mark* in particular are popular verbs. Apologies if you are a golang developer.  You're free to edit the scripts to change them to suit yourself !

### How can I find out if there's a newer version with better features?

Run the `jumpfs_info` command  - it will check for new versions.

### Where is the bookmark file?

Run the `jumpfs_info` command  - it will tell you.

### Why did you use JSON for the bookmark file?

Because it's a reasonable compromise between human and machine-readable and .Net has a Json serializer built in. I wanted to be able to hack the file by hand if necessary and to allow (at least, in principle) for other client applications to use the same bookmark file. 

### Why isn't there a way to delete bookmarks
Ooops - forgot to include this in the first release.  Coming soon....
