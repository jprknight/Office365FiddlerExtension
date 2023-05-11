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
#   v1.4    Jeremy Knight   5/11/2023   Amendment for output files update.
# 
Function Download { 
    # Only download a new zip file if it doesn't already exist.
    if (!(Test-Path "$($env:UserProfile)\Downloads\$Script:ZipFileName" -ErrorAction SilentlyContinue)) {
        $repo = "jprknight/Office365FiddlerExtension"
        $releases = "https://api.github.com/repos/$repo/releases"
        $tag = (Invoke-WebRequest $releases | ConvertFrom-Json)[0].tag_name
        $ZipDownload = "https://github.com/$repo/releases/download/$tag/$Script:ZipFileName"
        $Script:LocalZipFile = "$($env:UserProfile)\Downloads\$Script:ZipFileName"

        $Error.Clear()
        try {
            Invoke-WebRequest $ZipDownload -Out $LocalZipFile
        }
        catch {
            Write-Host $_
        }
        if ($Error.Count -eq 0) {
            Write-Host ""
            Write-Host "Downloaded $Script:ZipFileName." -ForegroundColor Green
        }
        $Error.Clear()
    }
}

Function Install {
    $FileCount = 0
    $Folders = @("$Script:FiddlerScriptsPath","$Script:FiddlerInspectorsPath")
    $InstallFiles = $Script:InstallFiles

    # Count any existing extension files in scripts/inspectors folders.
    foreach ($Folder in $Folders) {
        foreach ($File in $InstallFiles) {
            if (Test-Path "$Folder\$File" -ErrorAction SilentlyContinue) {
                Write-Host "$Folder\$File"
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
                    Download
                }
                Expand-Archive -LiteralPath $Script:LocalZipFile -DestinationPath $Script:FiddlerScriptsPath
                Expand-Archive -LiteralPath $Script:LocalZipFile -DestinationPath $Script:FiddlerInspectorsPath
            }
            catch {
                Write-Host $_
            }
            if ($Erorr.count -eq 0) {
                CleanDownloadFile
                Write-Host ""
                Write-Host "$Script:Operation complete, exiting." -ForegroundColor Green
                Exit
            }
        }
    }
    else {
        # This is the only dead end in the script. 
        # Didn't want to create the possibility of an infinite loop by calling install function from within itself here.
        Write-Host ""
        Write-Host "$Script:Operation detected $FileCount existing extension files, run the uninstall process first." -ForegroundColor Red
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

$Menu = {
    Write-Host ""
    Write-Host "**********************************************************" -ForegroundColor Cyan
    Write-Host "Office 365 Fiddler Extension Deployment Script" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "https://github.com/jprknight/Office365FiddlerExtension" -ForegroundColor Cyan
    Write-Host ""
    
    if ($Script:bExtInstalled) {
        Write-Host " Local Installed version: $Script:Version" -ForegroundColor Green    
    }
    else {
        Write-Host " Local Installed version: $Script:Version" -ForegroundColor Red
    }
    
    if ($Script:LatestWebVersion -eq "Unknown") {
        Write-Host " Latest Web Version:      $LatestWebVersion" -ForegroundColor Red
    }
    else {
        Write-Host " Latest Web Version:      $LatestWebVersion" -ForegroundColor Green
    }

    if ($Script:FiddlerPath -eq "Path not found!") {
        Write-Host " Fiddler Path:            $Script:FiddlerPath" -ForegroundColor Red
    }
    elseif ($Script:FiddlerPath -eq "More than one path found! Reboot?") {
        Write-Host " Fiddler Path:            $Script:FiddlerPath" -ForegroundColor Red
    }
    else {
        Write-Host " Fiddler Path:            $Script:FiddlerPath" -ForegroundColor Green
    }   

    if (Get-Process Fiddler -ErrorAction SilentlyContinue) {
        Write-Host " Fiddler Running:         True" -ForegroundColor Red
    }
    else {
        Write-Host " Fiddler Running:         False" -ForegroundColor Green
    }

    Write-Host ""
    Write-Host "**********************************************************" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1) Install" -ForegroundColor Cyan
    Write-Host "2) Upgrade" -ForegroundColor Cyan
    Write-Host "3) Uninstall" -ForegroundColor Cyan
    Write-Host "4) Set Fiddler Path" -ForegroundColor Cyan
    Write-Host "5) Exit" -ForegroundColor Cyan
    Write-Host ""
}

Function LocalVersionCheck {
    $ExtScriptsFile = "$Script:FiddlerScriptsPath\Office365FiddlerInspector.dll"
    $ExtInspectorsFile = "$Script:FiddlerInspectorsPath\Office365FiddlerInspector.dll"

    # Check for the main dll file in both scripts and inspectors folders to get version number.
    if ((Test-Path $ExtScriptsFile -ErrorAction SilentlyContinue) -AND (Test-Path $ExtInspectorsFile -ErrorAction SilentlyContinue)) {
        $dll = Get-Item "$Script:FiddlerScriptsPath\Office365FiddlerInspector.dll"
        $Script:Version = $("$($dll.VersionInfo.FileMajorPart).$($dll.VersionInfo.FileMinorPart).$($dll.VersionInfo.FileBuildPart)")
        [bool]$Script:bExtInstalled = 1
    }
    else {
        $Script:Version = "Not Installed"
        [bool]$Script:bExtInstalled = 0
    }
}

Function WebVersionCheck {
    try {
        $xml = New-Object System.Xml.XmlDocument
        $xml.Load("https://aka.ms/Office365FiddlerExtensionUpdateCheckUrl")
        # Legacy XML name used EXOFiddlerInspector in XML file in the repo, as to not create errors for old extension versions.
        $Script:LatestWebVersion = $($xml.EXOFiddlerInspector.version)
    }
    catch {
        $Script:LatestWebVersion = "Unknown"
    }
    
}

Function SetGlobals {

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

    $Script:DownloadPath = "$($env:UserProfile)\Downloads\"
    $Script:ZipFileName = "Office365FiddlerExtension.zip"
    [bool]$Script:bZipDownload = Test-Path "$Script:DownloadPath\$Script:ZipFileName" -ErrorAction SilentlyContinue
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
Function CleanDownloadFile {
    If (Test-Path "$($env:UserProfile)\Downloads\$Script:ZipFileName" -ErrorAction SilentlyContinue) {
        $Error.Clear()
        try {
            Remove-Item "$($env:UserProfile)\Downloads\$Script:ZipFileName"
        }
        catch {
            Write-Host $_
        }
        if ($Error.count -eq 0) {
            Write-Host ""
            Write-Host "Removed temporary download zip file $($env:UserProfile)\Downloads\$Script:ZipFileName" -ForegroundColor Green
        }
    }
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
            $Script:Operation = "Install"
            Install
        }
        2 {
            $Script:Operation = "Upgrade"
            Uninstall
            Install
            CleanDownloadFile
        }
        3 {
            $Script:Operation = "Uninstall"
            Uninstall
            CleanDownloadFile
        }
        4 {
            ManuallySetFiddlerPath
        }
    }
} While ($Selection -ne 5)