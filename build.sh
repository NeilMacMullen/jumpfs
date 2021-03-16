mkdir publish -p
rm publish/* -rf
dotnet publish -p:PublishProfile=linux
echo ""
echo "SORRY - windows exe can't be build under Linux"
echo "Build from PowerShell if you want this"
#dotnet publish -p:PublishProfile=windows

rm publish/*.pdb

cp scripts/bash/*.sh publish
cp scripts/powershell/*.ps* publish
cp scripts/cmd/*.bat publish 

echo "Artefacts are in the ./publish folder"
