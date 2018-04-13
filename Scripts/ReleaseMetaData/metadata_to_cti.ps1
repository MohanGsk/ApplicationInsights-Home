Param(
    [Parameter(Mandatory=$true,HelpMessage="Path to releaseMetaData.xml")]
    [string]
    $metadataPath
) 
# for release definition:
#$metadataPath = "$(SYSTEM.ARTIFACTSDIRECTORY)";


function Send-CtiEmail([string]$emailBody) {
    #TODO
}


if($metadataPath -notlike "releaseMetaData.xml") {
	Write-Verbose "'releaseMetaData.xml' not part of MetaDataPath: $metadataPath"
	Write-Verbose "Searching..."
    #assume this is Artifact Directory and find the required file
    $items = Get-ChildItem -Path $metadataPath -Recurse -Filter "releaseMetaData.xml"
	$items | ForEach-Object { Write-Verbose "Found: $($_.FullName)" }
    $metadataPath = $items[0].FullName
}
Write-Verbose "MetaDataPath: $metadataPath"
[xml]$metaData = Get-Content $metadataPath

# build email
$sb = [System.Text.StringBuilder]::new();
[void]$sb.AppendFormat("CTI Test of {0} {1}", "_NAME_", $metaData.MetaData.ReleaseName);
[void]$sb.AppendLine("<br/>");
[void]$sb.AppendLine("<br/>Requesting a CTI Test of the following SDKs:");
[void]$sb.AppendLine("<br/>");
[void]$sb.AppendLine("<br/><ul>");
foreach( $package in $metaData.MetaData.Packages.Package) {
    [void]$sb.AppendLine([string]::Format("<li>{0} {1}</li>", $package.Name, $package.MyGetUri));
}
[void]$sb.AppendLine("</ul><br/>");
[void]$sb.AppendLine("<br/>");
[void]$sb.AppendFormat("<br/>If there are any issues, please reach out to {0}", "_NAME_");
$emailBody = $sb.ToString()


Write-Output $emailBody


#Send-CtiEmail $emailBody