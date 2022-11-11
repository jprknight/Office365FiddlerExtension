## Download
Download with the PowerShell deployment script: Invoke-Expression (New-Object Net.WebClient).DownloadString('https://aka.ms/Deploy-Office365FiddlerExtension')

<a href="https://aka.ms/O365FiddlerExtensionUpdateUrl" target="_blank">Download the latest release</a> of the Office 365 Fiddler Extension.

## Extension Purpose
This Fiddler Extension is an Office 365 centric parser to efficiently troubleshoot Office 365 client application connectivity and functionality.

## How To Use The Extension
1. Reproduce an issue / behaviour: Use Fiddler Classic, FiddlerCap, or FiddlerAnywhere to collect a trace (decrypt traffic) on the computer where the issue is seen. Save the result as a SAZ file.
2. Review the result (SAZ) file: On your own computer install Fiddler Classic, install the extension, and open the SAZ file.

## How The Extension Alters The Standard Fiddler UI

* Colourisation of sessions.
* Add column 'Elapsed Time'.
* Add column 'Response Server'.
* Add column 'Session Type'.
* Add column 'Host IP'.
* Add column 'Authentication'.
* Add an 'Office 365' response inspector tab.
* Add an 'Office 365' menu to turn off/on extension and extension features.

## Project Links

Wiki: <a href="https://aka.ms/O365FiddlerExtensionWiki" target="_blank">https://aka.ms/O365FiddlerExtensionWiki</a>

Issues: <a href="https://aka.ms/O365FiddlerExtensionIssues" target="_blank">https://aka.ms/O365FiddlerExtensionIssues</a>

Download: <a href="https://aka.ms/O365FiddlerExtension" target="_blank">https://aka.ms/O365FiddlerExtension</a>

EHLO Technet blog post: <a href="https://techcommunity.microsoft.com/t5/exchange-team-blog/introducing-the-exchange-online-fiddler-extension/ba-p/608788" target="_blank">EHLO Blog Article</a>
