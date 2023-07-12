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
#   v1.0    Jeremy Knight   Nov,  9th 2022  Initial version.
#   v1.1    Jeremy Knight   Nov, 10th 2022  Re-write.
#   v1.2    Jeremy Knight   Nov, 11th 2022  Added upgrade option.
#                                           Consolidated reused code.
#   v1.3    Jeremy Knight   Nov, 14th 2022  Manually set deployment folder.
#                                           Complete message.
#   v1.4    Jeremy Knight   May, 11th 2023  Amendment for output files update.
#   v1.5    Jeremy Knight   Jul,  5th 2023  Additions for ruleset updates.
# 

Function Install {
    $FileCount = 0
    $Folders = @("$Script:FiddlerScriptsPath","$Script:FiddlerInspectorsPath")
    $InstallFiles = $Script:InstallFiles

    # Count any existing extension files in scripts/inspectors folders.
    foreach ($Folder in $Folders) {
        foreach ($File in $InstallFiles) {
            if (Test-Path "$Folder\$File" -ErrorAction SilentlyContinue) {
                WriteToLog "$Folder\$File"
                $FileCount++
            }
        }
    }

    # If no existing extension files are found, download the latest zip file from the repo, and install.
    if ($FileCount -eq 0) {
        if ((Test-Path $Script:FiddlerScriptsPath -ErrorAction SilentlyContinue) -AND (Test-Path $Script:FiddlerInspectorsPath -ErrorAction SilentlyContinue)) {
            $Error.Clear()
            try {
                if (!($Script:bZipDownload)) {
                    DownloadExtensionZipFile
                }
                Expand-Archive -LiteralPath $Script:LocalZipFile -DestinationPath $Script:FiddlerScriptsPath
                Expand-Archive -LiteralPath $Script:LocalZipFile -DestinationPath $Script:FiddlerInspectorsPath
            }
            catch {
                Write-Host $_
            }
            if ($Erorr.count -eq 0) {
                CleanExtensionDownloadFile
                Write-Host ""
                Write-Host "$Script:Operation complete, exiting." -ForegroundColor Green
                WriteToLog "$Script:Operation complete, exiting."
                Exit
            }
        }
    }
    else {
        # This is the only dead end in the script. 
        # Didn't want to create the possibility of an infinite loop by calling install function from within itself here.
        Write-Host ""
        Write-Host "$Script:Operation detected $FileCount existing extension files, run the uninstall process first." -ForegroundColor Red
        WriteToLog "$Script:Operation detected $FileCount existing extension files, run the uninstall process first."
    }
}

Function Uninstall {
    $RemovedFilesCount = 0
    $Folders = @("$Script:FiddlerScriptsPath", "$Script:FiddlerInspectorsPath")

    foreach ($Folder in $Folders) {
        foreach ($File in $Script:InstallFiles) {
            if (Test-Path "$Folder\$File" -ErrorAction SilentlyContinue) {
                $Error.Clear()
                try {
                    Write-Host "$Folder\$File"
                    WriteToLog "$Folder\$File"
                    Remove-Item "$Folder\$File"
                }
                catch {
                    WriteToLog $_
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
        WriteToLog "$Script:Operation removed $RemovedFilesCount files."
    }
    else {
        Write-Host ""
        Write-Host "$Script:Operation removed $RemovedFilesCount files." -ForegroundColor Green
        WriteToLog "$Script:Operation removed $RemovedFilesCount files."
    }
}

Function UpdateRulesetFiles {
    # If the extension major version is 1.x.x return, updating the ruleset dll does nothing for this version of the extension.
    # Only version 2.x.x calls the external ruleset dll.
    if ($Script:ExtensionLocalDLL.VersionInfo.FileMajorPart -eq 1) {
        return
    }

    if ($Script:RulesetLocalVersion -lt $Script:RulesetWebVersion) {
        WriteToLog "Ruleset local version older than web version. Attempting update."
        DownloadRulesetFiles

        try {
            WriteToLog "Attempting to expand ruleset files from downloaded zip file."
            Expand-Archive -LiteralPath $Script:LocalRulesetZipFile -DestinationPath $Script:FiddlerInspectorsPath
        } catch {
            WriteToLog $_
        }
    }
}

Function DownloadExtensionZipFile { 
    # Only download a new zip file if it doesn't already exist.
    if (!(Test-Path "$($env:UserProfile)\Downloads\$Script:ExtensionZipFileName" -ErrorAction SilentlyContinue)) {
        try {
            $Json = (Invoke-WebRequest $Script:GithubRepoBaseURL/ExtensionURLs.json).Content | ConvertFrom-Json
            $Script:ExtensionZipFileName = $Json.Extension

            $repo = "jprknight/Office365FiddlerExtension"
            $releases = "https://api.github.com/repos/$repo/releases"
            $tag = (Invoke-WebRequest $releases | ConvertFrom-Json)[0].tag_name
            $ExtensionZipDownload = "https://github.com/$repo/releases/download/$tag/$Script:ExtensionZipFileName"
            $Script:LocalZipFile = "$($env:UserProfile)\Downloads\$Script:ExtensionZipFileName"

            WriteToLog "Extenion Download URL: $Script:ExtensionZipDownload"
        } catch {
            WriteToLog $_
        }

        $Error.Clear()
        try {
            Invoke-WebRequest $ExtensionZipDownload -Out $LocalZipFile
        }
        catch {
            WriteToLog $_
        }
        if ($Error.Count -eq 0) {
            WriteToLog "Downloaded $Script:ExtensionZipDownload."
        }
        $Error.Clear()
    }
}

Function DownloadRulesetFiles {
    # Only download a new zip file if it doesn't already exist.
    if (!(Test-Path "$($env:UserProfile)\Downloads\$Script:RulesetZipFileName" -ErrorAction SilentlyContinue)) {
        $Json = (Invoke-WebRequest $Script:GithubRepoBaseURL/ExtensionURLs.json).Content | ConvertFrom-Json
        $Script:RulesetZipFileName = $Json.Ruleset

        $repo = "jprknight/Office365FiddlerExtension"
        $releases = "https://api.github.com/repos/$repo/releases"
        $tag = (Invoke-WebRequest $releases | ConvertFrom-Json)[0].tag_name
        $RulesetZipFileName = "https://github.com/$repo/releases/download/$tag/$Script:RulesetZipFileName"
        $Script:LocalRulesetZipFile = "$($env:UserProfile)\Downloads\$Script:RulesetZipFileName"

        try {
            $Json = (Invoke-WebRequest $Script:GithubRepoBaseURL/ExtensionURLs.json).Content | ConvertFrom-Json
            $Script:RulesetZipDownload = $Json.Ruleset
            WriteToLog "Extenion Download URL: $Script:RulesetZipDownload"
        } catch {
            WriteToLog $_
        }

        $Error.Clear()
        try {
            Invoke-WebRequest $Script:RulesetZipDownload -Out $LocalZipFile
        }
        catch {
            WriteToLog $_
        }
        if ($Error.Count -eq 0) {
            Write-Host ""
            Write-Host "Downloaded $Script:RulesetZipFileName." -ForegroundColor Green
            WriteToLog "Downloaded $Script:RulesetZipFileName."
        }
        $Error.Clear()
    }
}

$Menu = {
    Write-Host ""
    Write-Host "**************************************************************************" -ForegroundColor Cyan
    Write-Host "Office 365 Fiddler Extension Deployment Script" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "https://github.com/jprknight/Office365FiddlerExtension" -ForegroundColor Cyan
    Write-Host ""
    
    ###############################
    # FIDDLER RUNNING

    if (Get-Process Fiddler -ErrorAction SilentlyContinue) {
        Write-Host " Fiddler Running:           True" -ForegroundColor Red
        WriteToLog "Fiddler is running."
    }
    else {
        Write-Host " Fiddler Running:           False" -ForegroundColor Green
        WriteToLog "Fiddler is not running."
    }

    ###############################
    # FIDDLER PATH

    if ($Script:FiddlerPath -eq "Path not found!") {
        Write-Host " Fiddler Path:              $Script:FiddlerPath" -ForegroundColor Red
        WriteToLog "Fiddler path: Not found."
    }
    elseif ($Script:FiddlerPath -eq "More than one path found! Reboot?") {
        Write-Host " Fiddler Path:              $Script:FiddlerPath" -ForegroundColor Red
        WriteToLog "Fiddler path: More than one found."
    }
    else {
        Write-Host " Fiddler Path:              $Script:FiddlerPath" -ForegroundColor Green
        WriteToLog "Fiddler Scripts Path: $Script:FiddlerPath\Scripts"
        WriteToLog "Fiddler Inspectors Path: $Script:FiddlerPath\Inspectors"
    }

    Write-Host ""

    ###############################
    # EXTENSION LOCAL VERSION
    if ($Script:ExtensionLocalVersion -lt $Script:ExtensionWebVersion) {
        Write-Host " Extension Local Version:   $Script:ExtensionLocalVersion" -ForegroundColor Yellow
    }
    else {
        Write-Host " Extension Local Version:   $Script:ExtensionLocalVersion" -ForegroundColor Green
    }
    WriteToLog "Extension Local Version: $Script:ExtensionLocalVersion"
    
    ###############################
    # EXTENSION WEB VERSION
    if ($Script:ExtensionWebVersion -eq "Unknown") {
        Write-Host " Extension Web Version:     $ExtensionWebVersion" -ForegroundColor Red
    }
    else {
        Write-Host " Extension Web Version:     $ExtensionWebVersion" -ForegroundColor Green
    }
    WriteToLog "Extension Web Version: $ExtensionWebVersion"

    Write-Host ""

    ###############################
    # RULESET LOCAL VERSION
    if ($Script:RulesetLocalVersion -eq "Unknown" -OR $Script:RulesetFileCount -gt 3 -OR $null -eq $Script:LatestFile) {
        Write-Host " Ruleset Local Version:     $Script:RulesetLocalVersion" -ForegroundColor Red
    }

    if ($Script:RulesetLocalVersion -lt $Script:RulesetWebVersion) {
        Write-Host " Ruleset Local Version:     $Script:RulesetLocalVersion" -ForegroundColor Yellow
    }
    else {
        Write-Host " Ruleset Local Version:     $Script:RulesetLocalVersion" -ForegroundColor Green
    }
    WriteToLog "Ruleset Local Version: $Script:RulesetLocalVersion"
    
    ###############################
    # RULESET WEB VERSION
    if ($Script:RulesetWebVersion -eq "Unknown") {
        Write-Host " Ruleset Web Version:       $Script:RulesetWebVersion" -ForegroundColor Red
    }
    else {
        Write-Host " Ruleset Web Version:       $Script:RulesetWebVersion" -ForegroundColor Green
    }
    WriteToLog "Ruleset Web Version: $Script:RulesetWebVersion"

    Write-Host ""
    Write-Host "**************************************************************************" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1) Install" -ForegroundColor Cyan
    Write-Host "2) Upgrade" -ForegroundColor Cyan
    Write-Host "3) Uninstall" -ForegroundColor Cyan
    Write-Host "4) Set (override) Fiddler Path" -ForegroundColor Cyan
    Write-Host "5) Update Ruleset Files" -ForegroundColor Cyan
    Write-Host "6) Exit" -ForegroundColor Cyan
    Write-Host ""
}

Function ExtensionLocalVersionCheck {
    $ExtScriptsFile = "$Script:FiddlerScriptsPath\Office365FiddlerExtension.dll"
    $ExtInspectorsFile = "$Script:FiddlerInspectorsPath\Office365FiddlerExtension.dll"

    $InsScriptsFile = "$Script:FiddlerScriptsPath\Office365FiddlerInspector.dll"
    $InsInspectorsFile = "$Script:FiddlerInspectorsPath\Office365FiddlerInspector.dll"

    # Check for the main dll file in both scripts and inspectors folders to get version number.
    if ((Test-Path $ExtScriptsFile -ErrorAction SilentlyContinue) -AND (Test-Path $ExtInspectorsFile -ErrorAction SilentlyContinue)) {
        $Script:ExtensionLocalDLL = Get-Item $ExtInspectorsFile
        $Script:ExtensionLocalVersion = $("$($ExtensionLocalDLL.VersionInfo.FileMajorPart).$($ExtensionLocalDLL.VersionInfo.FileMinorPart).$($ExtensionLocalDLL.VersionInfo.FileBuildPart)")
        [bool]$Script:bExtInstalled = 1
    }
    elseif ((Test-Path $InsScriptsFile -ErrorAction SilentlyContinue) -AND (Test-Path $InsInspectorsFile -ErrorAction SilentlyContinue)) {
        $Script:ExtensionLocalDLL = Get-Item $InsInspectorsFile
        $Script:ExtensionLocalVersion = $("$($ExtensionLocalDLL.VersionInfo.FileMajorPart).$($ExtensionLocalDLL.VersionInfo.FileMinorPart).$($ExtensionLocalDLL.VersionInfo.FileBuildPart)")
        [bool]$Script:bExtInstalled = 1
    }
    else {
        $Script:ExtensionLocalVersion = "Not Installed"
        [bool]$Script:bExtInstalled = 0
    }
}

Function ExtensionWebVersionCheck {
    try {
        $Json = (Invoke-WebRequest $Script:GithubRepoBaseURL/ExtensionVersion.json).Content
        $VersionInfo = $Json | ConvertFrom-Json

        $Script:ExtensionWebVersion = "$($VersionInfo.ExtensionMajor).$($VersionInfo.ExtensionMinor).$($VersionInfo.ExtensionBuild)"
    }
    catch {
        $Script:ExtensionWebVersion = "Unknown"
        WriteToLog $_
    }   
}

Function RulesetLocalVersionCheck {
    $Script:LatestFile = ""
    $Script:RulesetFileCount = 0
    
    try {
        $Script:LatestRulesetLocalFile = Get-ChildItem $Script:FiddlerInspectorsPath -Filter $Script:RulesetDLLFilePattern | Sort-Object LastWriteTime | Select-Object -first 1

        if ($null -eq $Script:LatestFile) {
            $Script:RulesetLocalVersion = "No ruleset files found. Run ruleset download."
            return
        }

        foreach ($RulesetFilePattern in $Script:RulesetFilePatterns) {
            $Script:RulesetFileCount += (Get-ChildItem $Script:FiddlerInspectorsPath -Filter $RulesetFilePattern).count
        }

        if ($Script:RulesetFileCount -gt 3) {
            $Script:RulesetLocalVersion = "Too many ruleset files found. Run ruleset file download to clean up old ruleset files."
        }
        else {
            $Script:RulesetLocalVersion = $("$($Script:LatestRulesetLocalFile.VersionInfo.FileMajorPart).$($Script:LatestRulesetLocalFile.VersionInfo.FileMinorPart).$($Script:LatestRulesetLocalFile.VersionInfo.FileBuildPart)")
        }       
    }
    catch {
        $Script:RulesetLocalVersion = "Unknown"
        WriteToLog $_
    }
}

Function RulesetWebVersionCheck {
    try {
        $Json = (Invoke-WebRequest $Script:GithubRepoBaseURL/ExtensionVersion.json).Content
        $RulesetVersionInfo = $Json | ConvertFrom-Json

        $Script:RulesetWebVersion = "$($RulesetVersionInfo.RulesetMajor).$($RulesetVersionInfo.RulesetMinor).$($RulesetVersionInfo.RulesetBuild)"
        WriteToLog "Ruleset Web Version: $($RulesetVersionInfo.RulesetMajor).$($RulesetVersionInfo.RulesetMinor).$($RulesetVersionInfo.RulesetBuild)"
    }
    catch {
        $Script:RulesetWebVersion = "Unknown"
        WriteToLog $_
    }   
}

Function WriteToLog($LogText) {
    $Content = "$Script:LogFileTimestamp : $LogText"
    Add-Content -Path $Script:LogFile -Value $Content
}

Function SetGlobals {

    $Script:GithubRepoBaseURL = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension"

    # Includes legacy EXO Fiddler Extension files, so these can be processed.
    $Script:InstallFiles = @('Office365FiddlerInspector.dll',
    'Office365FiddlerInspector.dll.config',
    'Office365FiddlerInspector.pdb',
    'Microsoft.ApplicationInsights.AspNetCore.dll',
    'Microsoft.ApplicationInsights.AspNetCore.xml',
    'Microsoft.ApplicationInsights.dll',
    'Microsoft.ApplicationInsights.xml',
    'EXOFiddlerInspector.dll',
    'EXOFiddlerInspector.dll.config',
    'EXOFiddlerInspector.pdb',    
    'Office365FiddlerExtension.dll',
    'Office365FiddlerExtension.dll.config',
    'Office365FiddlerExtension.pdb')

    $Script:RulesetDLLFilePattern = "Office365FiddlerExtensionRuleset*.dll"

    $Script:RulesetFilePatterns = @("Office365FiddlerExtensionRuleset*.dll",
    "Office365FiddlerExtensionRuleset*.pdb",
    "Office365FiddlerExtensionRuleset*.dll.config")

    $Script:DownloadPath = "$($env:UserProfile)\Downloads\"

    
    $Script:RulesetZipFileName = "Office365FiddlerExtensionRuleset.zip"

    ###################################
    # Log file.

    $LogFileNameTimestamp = Get-Date -Format "yyyy-MM-dd_HH.mm.ss"
    $LogFileName = "Deploy-Office365FiddlerExtension-$LogFileNameTimestamp.log"
    $UserProfile = $($env:UserProfile)
    $Script:LogFile = "$UserProfile\Documents\$LogFileName"
    $Script:LogFileTimestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

    ###################################
    # Downloads.

    [bool]$Script:bZipDownload = Test-Path "$Script:DownloadPath\$Script:ExtensionZipFileName" -ErrorAction SilentlyContinue

    # Ruleset.
    try {
        $Json = (Invoke-WebRequest $Script:GithubRepoBaseURL/ExtensionURLs.json).Content | ConvertFrom-Json
        $Script:RulesetDLLURL = $Json.Ruleset 
    } catch {
        WriteToLog $_
    }

}

# Fiddler path needs to be set to where Fiddler.exe resides.
Function SetFiddlerPaths {
    $PathCount = 0
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
        $Script:FiddlerPath = "Path not found!"
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
Function CleanExtensionDownloadFile {
    If (Test-Path "$($env:UserProfile)\Downloads\$Script:ExtensionZipFileName" -ErrorAction SilentlyContinue) {
        $Error.Clear()
        try {
            Remove-Item "$($env:UserProfile)\Downloads\$Script:ExtensionZipFileName"
        }
        catch {
            Write-Host $_
        }
        if ($Error.count -eq 0) {
            Write-Host ""
            Write-Host "Removed temporary download zip file $($env:UserProfile)\Downloads\$Script:ExtensionZipFileName" -ForegroundColor Green
        }
    }
}

Function CleanRulesetDownloadFile {
    If (Test-Path "$($env:UserProfile)\Downloads\$Script:RulesetZipFileName" -ErrorAction SilentlyContinue) {
        $Error.Clear()
        try {
            Remove-Item "$($env:UserProfile)\Downloads\$Script:RulesetZipFileName"
        }
        catch {
            Write-Host $_
        }
        if ($Error.count -eq 0) {
            Write-Host ""
            Write-Host "Removed temporary download zip file $($env:UserProfile)\Downloads\$Script:RulesetZipFileName" -ForegroundColor Green
        }
    }
}

Do {
    SetGlobals
    SetFiddlerPaths
    ExtensionLocalVersionCheck
    ExtensionWebVersionCheck
    RulesetLocalVersionCheck
    RulesetWebVersionCheck

    Invoke-Command -ScriptBlock $Menu
    $Selection = Read-Host "Selection"

    Switch ($Selection) {
        1 {
            $Script:Operation = "Install"
            Install
        }
        2 {
            $Script:Operation = "Upgrade"
            Uninstall
            Install
            CleanExtensionDownloadFile
        }
        3 {
            $Script:Operation = "Uninstall"
            Uninstall
            CleanExtensionDownloadFile
        }
        4 {
            ManuallySetFiddlerPath
        }
        5 {
            UpdateRulesetFiles
        }
    }
} While ($Selection -ne 6)