#################################################################################
#
# The sample scripts are not supported under any Microsoft standard support 
# program or service. The sample scripts are provided AS IS without warranty 
# of any kind. Microsoft further disclaims all implied warranties including, without 
# limitation, any implied warranties of merchantability or of fitness for a particular 
# purpose. The entire risk arising out of the use or performance of the sample scripts 
# and documentation remains with you. In no event shall Microsoft, its authors, or 
# anyone else involved in the creation, production, or delivery of the scripts be liable 
# for any damages whatsoever (including, without limitation, damages for loss of business 
# profits, business interruption, loss of business information, or other pecuniary loss) 
# arising out of the use of or inability to use the sample scripts or documentation, 
# even if Microsoft has been advised of the possibility of such damages.
#
################################################################################# 

############################################################
#
#   Office 365 Fiddler Extension Deployment Script
#
#   v1.0    Jeremy Knight   11/9/20222  Initial version.
#   v1.1    Jeremy Knight   11/10/2022  Re-write.
#   v1.2    Jeremy Knight   11/11/2022  Added upgrade option.
#                                       Consolidated reused code.
#   v1.3    Jeremy Knight   11/14/2022  Manually set deployment folder.
#                                       Complete message.
#   v1.4    Jeremy Knight   5/19/2024   v1.0.78 & v2.0.0.
#   v1.5    Jeremy Knight   7/10/2024   Minor fixes. Coincides with the release of v2.0.3.
#   v1.6    Jeremy Knight   7/12/2024   Updated WebVersionCheck function to read Json data 
#                                       from ExtensionVersion.json in Github repo.
#   v1.7    Jeremy Knight   2/21/2025   Added ability to update ruleset from latest release
#                                       in the Github repository.
# 
############################################################
#

Function SetGlobals {

    # Includes legacy EXO Fiddler Extension files, so these can be processed.
    $Script:InstallFiles = @(
    'Office365FiddlerExtension.dll',
    'Office365FiddlerExtension.dll.config',
    'Office365FiddlerExtension.pdb',
    'Office365FiddlerExtensionRuleset.dll',
    'Office365FiddlerExtensionRuleset.dll.config',
    'Office365FiddlerExtensionRuleset.pdb',
    'Office365FiddlerInspector.dll',
    'Office365FiddlerInspector.dll.config',
    'Office365FiddlerInspector.pdb',
    'Microsoft.ApplicationInsights.AspNetCore.dll',
    'Microsoft.ApplicationInsights.AspNetCore.xml',
    'Microsoft.ApplicationInsights.dll',
    'Microsoft.ApplicationInsights.pdb',
    'Microsoft.ApplicationInsights.xml',
    'EXOFiddlerInspector.dll',
    'EXOFiddlerInspector.dll.config',
    'EXOFiddlerInspector.pdb',
    'SessionClassification.json')

    $Script:RulesetFiles = @(
        'Office365FiddlerExtensionRuleset.dll',
        'Office365FiddlerExtensionRuleset.dll.config',
        'Office365FiddlerExtensionRuleset.pdb'
    )

    $Script:DownloadPath = "$($env:UserProfile)\Downloads\"
    $Script:ZipFileName_v1078 = "Office365FiddlerExtension-v1.0.78.zip"
    $Script:ZipFileName_v2xx = "Office365FiddlerExtension.zip"
    $Script:ZipFileName_Ruleset = "Office365FiddlerExtensionRuleset.zip"

    [bool]$Script:bZipDownload_v1078 = Test-Path "$Script:DownloadPath\$Script:ZipFileName_v1078" -ErrorAction SilentlyContinue
    [bool]$Script:bZipDownload_v2xx = Test-Path "$Script:DownloadPath\$Script:ZipFileName_v2xx" -ErrorAction SilentlyContinue
    
    $Script:URL_JsonUpdate = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/master/Office365FiddlerExtension/ExtensionVersion.json"

    $Script:URL_Repository = "jprknight/Office365FiddlerExtension"

    $Script:URL_Releases = "https://api.github.com/repos/$Script:URL_Repository/releases"
}

Function Download([string]$version) {
    # v1.0.78
    # Only download a new zip file if it doesn't already exist.
    if ($version -eq "1.0.78") {
        if (!(Test-Path "$($env:UserProfile)\Downloads\Office365FiddlerExtension-v1.0.78.zip" -ErrorAction SilentlyContinue)) {
            $tag = (Invoke-WebRequest $Script:URL_Releases | ConvertFrom-Json)[0].tag_name
            $ZipDownload = "https://github.com/$Script:URL_Repository/releases/download/$tag/$Script:ZipFileName_v1078"
            $Script:LocalZipFile_v1078 = "$($env:UserProfile)\Downloads\$Script:ZipFileName_v1078"
    
            $Error.Clear()
            try {
                Invoke-WebRequest $ZipDownload -Out $Script:LocalZipFile_v1078
            }
            catch {
                Write-Host $_
            }
            if ($Error.Count -eq 0) {
                Write-Host ""
                Write-Host "Downloaded $Script:LocalZipFile_v1078." -ForegroundColor Green
            }
            $Error.Clear()
        }
    }
    elseif ($version -eq '2.0.x') {
        # v2.x.x 
        # Only download a new zip file if it doesn't already exist.
        if (!(Test-Path "$($env:UserProfile)\Downloads\$Script:ZipFileName_v2xx" -ErrorAction SilentlyContinue)) {
            $tag = (Invoke-WebRequest $Script:URL_Releases | ConvertFrom-Json)[0].tag_name
            $ZipDownload_2xx = "https://github.com/$Script:URL_Repository/releases/download/$tag/$Script:ZipFileName_v2xx"
            $Script:LocalZipFile_v2xx = "$($env:UserProfile)\Downloads\$Script:ZipFileName_v2xx"

            $Error.Clear()
            try {
                Invoke-WebRequest $ZipDownload_2xx -Out $Script:LocalZipFile_v2xx
            }
            catch {
                Write-Host $_
            }
            if ($Error.Count -eq 0) {
                Write-Host ""
                Write-Host "Downloaded $Script:ZipFileName_v2xx." -ForegroundColor Green
            }
            $Error.Clear()
        }
    }
    elseif ($version -eq 'ruleset') {
        # Download the ruleset zip file.
        $tag = (Invoke-WebRequest $Script:URL_Releases | ConvertFrom-Json)[0].tag_name
            $ZipDownload = "https://github.com/$Script:URL_Repository/releases/download/$tag/$Script:ZipFileName_Ruleset"
            $Script:LocalZipFile_Ruleset = "$($env:UserProfile)\Downloads\$Script:ZipFileName_Ruleset"
    
            $Error.Clear()
            try {
                Invoke-WebRequest $ZipDownload -Out $Script:ZipFileName_Ruleset
            }
            catch {
                Write-Host $_
            }
            if ($Error.Count -eq 0) {
                Write-Host ""
                Write-Host "Downloaded $Script:LocalZipFile_Ruleset." -ForegroundColor Green
            }
            $Error.Clear()
    }
}

Function Install_v1078 {
    Uninstall

    if (!(Test-Path $Script:FiddlerScriptsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerScriptsPath doesn't exist."
        Return
    }

    if (!(Test-Path $Script:FiddlerInspectorsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerInspectorsPath doesn't exist."
        Return
    }

    # If no existing extension files are found, download the latest zip file from the repo, and install.
    $Error.Clear()
    try {
        if (!($Script:bZipDownload_v1078)) {
            Download("1.0.78")
        }
        Expand-Archive -LiteralPath $Script:LocalZipFile_v1078 -DestinationPath $Script:FiddlerScriptsPath
        Expand-Archive -LiteralPath $Script:LocalZipFile_v1078 -DestinationPath $Script:FiddlerInspectorsPath
    }
    catch {
        Write-Host $_
    }
    if ($Erorr.count -eq 0) {
        CleanDownloadFile
        Write-Host ""
        Write-Host "$Script:Operation complete, exiting." -ForegroundColor Green
        Read-Host "Press any key to exit."
        Exit
    }

}

Function Install_v2xx {
    # Run the uninstall first to clear out old extension files.
    Uninstall

    if (!(Test-Path $Script:FiddlerScriptsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerScriptsPath doesn't exist."
        Return
    }

    if (!(Test-Path $Script:FiddlerInspectorsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerInspectorsPath doesn't exist."
        Return
    }

    # Download the latest zip file from the repo, and install.
    $Error.Clear()
    try {
        if (!($Script:bZipDownload)) {
            Download("2.0.x")
        }
        Expand-Archive -LiteralPath $Script:LocalZipFile_v2xx -DestinationPath $Script:FiddlerScriptsPath
        Expand-Archive -LiteralPath $Script:LocalZipFile_v2xx -DestinationPath $Script:FiddlerInspectorsPath
    }
    catch {
        Write-Host $_
    }
    if ($Erorr.count -eq 0) {
        CleanDownloadFile
        Write-Host ""
        Write-Host "$Script:Operation complete, exiting." -ForegroundColor Green
        Read-Host "Press any key to exit."
        Exit
    }
}

Function InstallRulesetFiles {
    # Remove the existing ruleset files first.
    RemoveRulesetFiles

    if (!(Test-Path $Script:FiddlerScriptsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerScriptsPath doesn't exist."
        Return
    }

    if (!(Test-Path $Script:FiddlerInspectorsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerInspectorsPath doesn't exist."
        Return
    }

    # Download the latest zip file from the repo, and install.
    $Error.Clear()
    try {
        if (!($Script:bZipDownload)) {
            Download("ruleset")
        }
        Expand-Archive -LiteralPath $Script:LocalZipFile_Ruleset -DestinationPath $Script:FiddlerScriptsPath
        Expand-Archive -LiteralPath $Script:LocalZipFile_Ruleset -DestinationPath $Script:FiddlerInspectorsPath
    }
    catch {
        Write-Host $_
    }
    if ($Erorr.count -eq 0) {
        CleanDownloadFile
        Write-Host ""
        Write-Host "$Script:Operation complete, exiting." -ForegroundColor Green
        Read-Host "Press any key to exit."
        Exit
    }
}

Function Uninstall {
    if (!(Test-Path $Script:FiddlerScriptsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerScriptsPath doesn't exist."
        Return
    }

    if (!(Test-Path $Script:FiddlerInspectorsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerInspectorsPath doesn't exist."
        Return
    }

    $RemovedFilesCount = 0
    $Folders = @("$Script:FiddlerScriptsPath", "$Script:FiddlerInspectorsPath")

    Write-Host ""
    Write-Host "Running uninstall."
    Write-Host ""

    foreach ($Folder in $Folders) {
        foreach ($File in $Script:InstallFiles) {
            if (Test-Path "$Folder\$File" -ErrorAction SilentlyContinue) {
                $Error.Clear()
                try {
                    Write-Host "Removing: $Folder\$File"
                    Remove-Item "$Folder\$File"
                }
                catch {
                    Write-Host $_
                }
                if ($Error.count -eq 0) {
                    $RemovedFilesCount++
                }
            }
        }
    }
    if ($RemovedFilesCount -eq 0) {
        Write-Host ""
        Write-Host "$Script:Operation removed $RemovedFilesCount files." -ForegroundColor Red
    }
    else {
        Write-Host ""
        Write-Host "$Script:Operation removed $RemovedFilesCount files." -ForegroundColor Green
    }
}

Function RemoveRulesetFiles {
    if (!(Test-Path $Script:FiddlerScriptsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerScriptsPath doesn't exist."
        Return
    }

    if (!(Test-Path $Script:FiddlerInspectorsPath -ErrorAction SilentlyContinue)) {
        Write-Host "Fiddler folder $Script:FiddlerInspectorsPath doesn't exist."
        Return
    }

    $RemovedFilesCount = 0
    $Folders = @("$Script:FiddlerScriptsPath", "$Script:FiddlerInspectorsPath")

    Write-Host ""
    Write-Host "Removing ruleset files."
    Write-Host ""

    foreach ($Folder in $Folders) {
        foreach ($File in $Script:RulesetFiles) {
            if (Test-Path "$Folder\$File" -ErrorAction SilentlyContinue) {
                $Error.Clear()
                try {
                    Write-Host "Removing: $Folder\$File"
                    Remove-Item "$Folder\$File"
                }
                catch {
                    Write-Host $_
                }
                if ($Error.count -eq 0) {
                    $RemovedFilesCount++
                }
            }
        }
    }
    if ($RemovedFilesCount -eq 0) {
        Write-Host ""
        Write-Host "$Script:Operation removed $RemovedFilesCount files." -ForegroundColor Red
    }
    else {
        Write-Host ""
        Write-Host "$Script:Operation removed $RemovedFilesCount files." -ForegroundColor Green
    }
}

Function LocalVersionCheck {
    # Extension Version
    $ExtensionFileName = "Office365FiddlerExtension.dll"
    $ExtScriptsFile = "$Script:FiddlerScriptsPath\$ExtensionFileName"
    $ExtInspectorsFile = "$Script:FiddlerInspectorsPath\$ExtensionFileName"

    # Check for the main dll file in both scripts and inspectors folders to get version number.
    if ((Test-Path $ExtScriptsFile -ErrorAction SilentlyContinue) -AND (Test-Path $ExtInspectorsFile -ErrorAction SilentlyContinue)) {
        $dll = Get-Item "$Script:FiddlerScriptsPath\$ExtensionFileName"
        $Script:ExtensionVersion = $("$($dll.VersionInfo.FileMajorPart).$($dll.VersionInfo.FileMinorPart).$($dll.VersionInfo.FileBuildPart)")
        [bool]$Script:bExtInstalled = 1
    }
    else {
        $Script:ExtensionVersion = "Not Installed"
        [bool]$Script:bExtInstalled = 0
    }

    # Ruleset Version
    $RulesetFilename = "Office365FiddlerExtensionRuleset.dll";
    $RulesetScriptsFile = "$Script:FiddlerScriptsPath\$RulesetFilename"
    $RulesetInspectorsFile = "$Script:FiddlerInspectorsPath\$RulesetFilename"

    # Check for the main dll file in both scripts and inspectors folders to get version number.
    if (Test-Path $RulesetInspectorsFile -ErrorAction SilentlyContinue) {
        $dll = Get-Item $RulesetInspectorsFile
        $Script:RulesetVersion = $("$($dll.VersionInfo.FileMajorPart).$($dll.VersionInfo.FileMinorPart).$($dll.VersionInfo.FileBuildPart)")
        [bool]$Script:bRulesetInstalled = 1
    }
    else {
        $Script:RulesetVersion = "Not Installed"
        [bool]$Script:bRulesetInstalled = 0
    }
}

Function WebVersionCheck {

    # Extension Github Version.
    try {
        $JsonURL = $Script:URL_JsonUpdate
        
        $JsonWebString = Invoke-RestMethod -Uri $JsonURL

        $Json = $JsonWebString | ConvertTo-Json | ConvertFrom-Json

        $WebExtensionMajor = ($Json).ExtensionMajor
        $WebExtensionMinor = ($Json).ExtensionMinor
        $WebExtensionBuild = ($Json).ExtensionBuild

        $Script:LatestWebExtensionVersion = "$WebExtensionMajor.$WebExtensionMinor.$WebExtensionBuild"
    }
    catch {
        $Script:LatestWebExtensionVersion = "Unknown"
    }

    # Ruleset Github Version.
    try {
        $JsonURL = $Script:URL_JsonUpdate
        
        $JsonWebString = Invoke-RestMethod -Uri $JsonURL

        $Json = $JsonWebString | ConvertTo-Json | ConvertFrom-Json

        $WebRulesetMajor = ($Json).RulesetMajor
        $WebRulesetMinor = ($Json).RulesetMinor
        $WebRulesetBuild = ($Json).RulesetBuild

        $Script:LatestWebRulesetVersion = "$WebRulesetMajor.$WebRulesetMinor.$WebRulesetBuild"
    }
    catch {
        $Script:LatestWebRulesetVersion = "Unknown"
    }
}

Function SetFiddlerPaths {
    $Paths = $env:path -split ";"
    foreach ($path in $Paths) {
        if ($Path -like "*Fiddler*") {
            $Script:FiddlerPath = $Path
            $Script:FiddlerScriptsPath = "$Script:FiddlerPath\Scripts"
            $Script:FiddlerInspectorsPath = "$Script:FiddlerPath\Inspectors"
            $PathCount++
        }
    }
    if ($PathCount -eq 0) {
        $Script:FiddlerPath = "Path not found! Reboot if you've just installed Fiddler."
    }
    if ($PathCount -gt 1) {
        $Script:FiddlerPath = "More than one path found! Reboot?"
    }
}

Function ManuallySetFiddlerPath {
    Write-Host "Enter a path to manually override where Fiddler is installed."
    $Path = Read-Host "Path"

    if (Test-Path "$Path\Fiddler.exe") {
        Write-Host ""
        Write-Host "Fiddler.exe found in the path, updating." -ForegroundColor Green
        $Script:FiddlerPath = $Path
    }
    else {
        Write-Host ""
        Write-Host "Fiddler.exe not found in the path." -ForegroundColor Red
    }
}
Function CleanDownloadFile {
    # v1.0.78
    If (Test-Path "$($env:UserProfile)\Downloads\$Script:ZipFileName_v1078" -ErrorAction SilentlyContinue) {
        $Error.Clear()
        try {
            Remove-Item "$($env:UserProfile)\Downloads\$Script:ZipFileName_v1078"
        }
        catch {
            Write-Host $_
        }
        if ($Error.count -eq 0) {
            Write-Host ""
            Write-Host "Removed temporary download zip file $($env:UserProfile)\Downloads\$Script:ZipFileName_v1078" -ForegroundColor Green
        }
    }

    # v2.x.x
    If (Test-Path "$($env:UserProfile)\Downloads\$Script:ZipFileName_v2xx" -ErrorAction SilentlyContinue) {
        $Error.Clear()
        try {
            Remove-Item "$($env:UserProfile)\Downloads\$Script:ZipFileName_v2xx"
        }
        catch {
            Write-Host $_
        }
        if ($Error.count -eq 0) {
            Write-Host ""
            Write-Host "Removed temporary download zip file $($env:UserProfile)\Downloads\$Script:ZipFileName_v2xx" -ForegroundColor Green
        }
    }

    # Ruleset
    If (Test-Path "$($env:UserProfile)\Downloads\$Script:ZipFileName_Ruleset" -ErrorAction SilentlyContinue) {
        $Error.Clear()
        try {
            Remove-Item "$($env:UserProfile)\Downloads\$Script:ZipFileName_Ruleset"
        }
        catch {
            Write-Host $_
        }
        if ($Error.count -eq 0) {
            Write-Host ""
            Write-Host "Removed temporary download zip file $($env:UserProfile)\Downloads\$Script:ZipFileName_Ruleset" -ForegroundColor Green
        }
    }
}

$Menu = {
    LocalVersionCheck
    Write-Host ""
    Write-Host "**********************************************************" -ForegroundColor Cyan
    Write-Host "Office 365 Fiddler Extension Deployment Script" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "https://github.com/jprknight/Office365FiddlerExtension" -ForegroundColor Cyan
    Write-Host ""
    
    if ($Script:bExtInstalled) {
        Write-Host " Extension Installed version:   $Script:ExtensionVersion" -ForegroundColor Green
    }
    else {
        Write-Host " Extension Installed version:   $Script:ExtensionVersion" -ForegroundColor Red
    }

    if ($Script:LatestWebVersion -eq "Unknown") {
        Write-Host " Extension Latest Web Version:  $Script:LatestWebExtensionVersion" -ForegroundColor Red
    }
    else {
        Write-Host " Extension Latest Web Version:  $Script:LatestWebExtensionVersion" -ForegroundColor Green
    }

    if ($Script:bRulesetInstalled) {
        Write-Host " Ruleset Installed version:     $Script:RulesetVersion" -ForegroundColor Green
    }
    else {
        Write-Host " Ruleset Installed version:     $Script:RulesetVersion" -ForegroundColor Red
    }

    if ($Script:LatestWebRulesetVersion -eq "Unknown") {
        Write-Host " Ruleset Latest Web Version:    $Script:LatestWebRulesetVersion" -ForegroundColor Red
    }
    else {
        Write-Host " Ruleset Latest Web Version:    $Script:LatestWebRulesetVersion" -ForegroundColor Green
    }

    if ($Script:FiddlerPath -eq "Path not found!") {
        Write-Host " Fiddler Path:                  $Script:FiddlerPath" -ForegroundColor Red
    }
    elseif ($Script:FiddlerPath -eq "More than one path found! Reboot?") {
        Write-Host " Fiddler Path:                  $Script:FiddlerPath" -ForegroundColor Red
    }
    else {
        Write-Host " Fiddler Path:                  $Script:FiddlerPath" -ForegroundColor Green
    }   

    if (Get-Process Fiddler -ErrorAction SilentlyContinue) {
        Write-Host " Fiddler Running:               True" -ForegroundColor Red
    }
    else {
        Write-Host " Fiddler Running:               False" -ForegroundColor Green
    }

    Write-Host ""
    Write-Host "**********************************************************" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1) Install Extension v2.x.x (Current)" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "2) Install Extension v1.0.78 (Legacy)" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "3) Upgrade Extension" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "4) Update Ruleset" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "5) Uninstall" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "6) Set Fiddler Path" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "7) Exit" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "**********************************************************" -ForegroundColor Cyan
}

Do {
    SetGlobals
    SetFiddlerPaths
    LocalVersionCheck
    WebVersionCheck

    Invoke-Command -ScriptBlock $Menu
    $Selection = Read-Host "Selection"

    Switch ($Selection) {
        1 {
            $Script:Operation = "Install_v2xx"
            Install_v2xx    
        }
        2 {
            $Script:Operation = "Install_v1078"
            Install_v1078
        }
        3 {
            $Script:Operation = "Upgrade"
            Uninstall
            Install_v2xx
            CleanDownloadFile
        }
        4 {
            $Script:Operation = "Update Ruleset"
            RemoveRulesetFiles
            InstallRulesetFiles
            CleanDownloadFile
        }
        5 {
            $Script:Operation = "Uninstall"
            Uninstall
            CleanDownloadFile
        }
        6 {
            ManuallySetFiddlerPath
        }
    }
} While ($Selection -ne 7)