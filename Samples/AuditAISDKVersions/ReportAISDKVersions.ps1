param(
    [string]$subscriptions    
)
$ErrorActionPreference = "Stop"

if(!$subscriptions){
    Write-Host "Provide comma separated string of subscription Ids or names. For instance: .\ReportAISDKVersions.ps1 -subscriptions ""subscription-name-1,subscription-id-2"""
    exit
}

$apiKeyDescription = "Read SDK versions to generate report"
$permissions = @("ReadTelemetry")
$operation = "query"
$query = [uri]::EscapeUriString("?query=requests | summarize argmax(timestamp, sdkVersion) by cloud_RoleName | project strcat(cloud_RoleName, '|', max_timestamp_sdkVersion)")
$reportFileName = "AISDKVersionAuditReport-$(get-date -f yyyy-MM-dd-hh-mm).csv"
$header = "SDKLanguage,SDKVersion,AIResourceName,RoleName,Subscription,AIResourceId,FailureMessage(If any)"
$header | add-content -path $PSScriptRoot\$reportFileName

foreach($sub in $subscriptions.Split(",")){
    Set-AzureRmContext -Subscription $sub 
    $aiResources = Get-AzureRmApplicationInsights
    foreach($ai in $aiResources){
        $appOutString = ""
        $appId = $ai.AppId
        try{
            Write-Host "Processing AI resource: " $ai.name
            $apiKeyResult = New-AzureRmApplicationInsightsApiKey -ResourceId $ai.Id -Description $apiKeyDescription -Permissions $permissions 
            if($apiKeyResult.Id){
                $headers = @{"X-Api-Key" = $apiKeyResult.ApiKey; "Content-Type" = "application/json" }
                $response = Invoke-WebRequest -uri "https://api.applicationinsights.io/v1/apps/$appId/$operation$query" -Headers $headers
                $json = ConvertFrom-Json $response.Content
                
                foreach($roleData in $json.tables.rows){
                    $sdkParts = $roleData.Split("|")
                    
                    $language = "DotNet"
                    $versionParts = $sdkParts[1].Split(":")
                    $langPart = $versionParts[0]
                    $version = $versionParts[1].Split("-")[0]
                    if($langPart.StartsWith("aspnet")){
                        $language = "DotNetCore"
                    }elseif ($langPart.StartsWith("py")) {
                        $language = "Python"
                    }elseif ($langPart -eq "java") {
                        $language = "Java"
                    }elseif ($langPart -eq "node") {
                        $language = "Node"
                    }elseif ($langPart.StartsWith("azurefunctions")) {
                        $language = "Azure Functions"
                    }elseif ($langPart -eq "rb") {
                        $language = "Ruby"
                    }
                    Write-Host $ai.name " - " $sdkParts[0] " is using " $language " SDK version =" $version 
                    $appOutString = $language + "," + $version + "," + $ai.name + "," + $sdkParts[0] + "," + $sub + "," + $ai.Id 
                }
                try{
                    Remove-AzureRmApplicationInsightsApiKey -ResourceId $ai.Id -ApiKeyId $apiKeyResult.Id                      
                } catch {
                    $failureMessage = "Failed to remove the API key:" + $_.Exception.Message
                    Write-Host "Failed to remove the API key for "+$ai.name, $_.Exception.Message
                    $appOutString += $failureMessage
                }
                
            }          
        }
        catch {
            if($appOutString.Contains(",")){
                $appOutString += $failureMessage
            } else {
                $appOutString = "NA,NA,"+$ai.name+","+"NA,"+$sub+","+$ai.Id+","+$_.Exception.Message
            }
            Write-Host $ai.name " failed with "  $_.Exception.Message
        }
        if($appOutString -eq ""){
            $appOutString = "NA,NA,"+$ai.name+","+"NA,"+$sub+","+$ai.Id+","+"No Request data found"
        }
        $appOutString | add-content -path $PSScriptRoot\$reportFileName
    }

}
