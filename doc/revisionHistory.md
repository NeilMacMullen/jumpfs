# Change log

### v1.5.0
- Migrate to Net8

### v1.4.0
- Migrate to Net7
- Make version checking less intrusive by only checking on 1 in 10 startups

### v1.3.0
- fixes an issue where `get-content jfs:x` would never terminate
- fixed nuspec file

# v1.2.0
- Implemented 'remove'
- powershell and bash modules now better organised
- better handling for missing bookmarks
- If you pass an actual path instead of a bookmark, jumpfs now does the "right thing" in most cases.
- Bookmarks can be exposed via [virtual drive](doc/psdrive.md) (Powershell 7 only)
- It is now possible to bookmark Urls and shell commands

See [full revision history](doc/revisionHistory.md)

# v1.1.0 
- Initial public release 
