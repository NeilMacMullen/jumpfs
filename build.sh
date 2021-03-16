mkdir publish -Force
rm publish\* -rf
dotnet publish -p:PublishProfile=linux
dotnet publish -p:PublishProfile=windows

rm publish\*.pdb

cp scripts\bash\*.sh publish
cp scripts\powershell\*.ps* publish
cp scripts\cmd\*.bat publish 


echo "Artefacts are in the ./publish folder"
