mkdir publish -Force
Remove-Item publish\* -Recurse -Force
dotnet publish -p:PublishProfile=linux
dotnet publish -p:PublishProfile=windows

Remove-Item publish\*.pdb -Recurse

Copy-Item scripts\bash\*.sh publish
Copy-Item scripts\powershell\*.ps* publish
Copy-Item scripts\cmd\*.bat publish 


Write-Host "Artefacts are in the ./publish folder"
