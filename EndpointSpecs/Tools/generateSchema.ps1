$generatorPath = "C:\src\mseng\AppInsights-Common"
$schemasPath = "C:\src\mseng\DataCollectionSchemas"

$currentDir = $scriptPath = split-path -parent $MyInvocation.MyCommand.Definition


#fix path
$generatorPath = "$generatorPath\..\bin\Debug\BondSchemaGenerator\BondSchemaGenerator"
$schemasPath = "$schemasPath\v2\Bond"

& "$generatorPath\BondSchemaGenerator.exe" -v -i "$schemasPath\AppInsightsTypes.bond" -i "$schemasPath\ContextTagKeys.bond" -o "$currentDir\..\Schemas\Bond\" -e BondLanguage -t BondLayout -n test --flatten false


& "$generatorPath\BondSchemaGenerator.exe" -v -i "$schemasPath\AppInsightsTypes.bond" -i "$schemasPath\ContextTagKeys.bond" -o "$currentDir\..\Schemas\Docs\" -e DocsLanguage -t DocsLayout -n test --flatten false

#mkdir -Force .\obj
#Invoke-WebRequest -o ".\obj\nuget.exe" https://api.nuget.org/downloads/nuget.exe

#& .\obj\nuget install Bond.CSharp -Version 4.2.1 -OutputDirectory .\obj\packages


#dir "$currentDir\..\Schemas\Bond\" | ForEach-Object { 
#    & .\obj\packages\Bond.CSharp.4.2.1\tools\gbc.exe schema -o "$currentDir\..\Schemas\JSONSchema" $_.FullName
#}

#dir "$currentDir\..\Schemas\JSONSchema\" | ForEach-Object { 
#    Get-Content $_.FullName | ConvertFrom-Json | ConvertTo-Json | Set-Content $_.FullName
#}