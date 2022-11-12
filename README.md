## Download

### Deployment Script
The best way to get the Office 365 Fiddler Extension is via the deployment script. Run the below in PowerShell on your computer: 

`Invoke-Expression (New-Object Net.WebClient).DownloadString('https://aka.ms/Deploy-Office365FiddlerExtension')`

## Extension Purpose
This Fiddler Extension is an Office 365 centric parser to efficiently troubleshoot Office 365 client application connectivity and functionality.

## How To Use The Extension
1. Reproduce an issue / behaviour: Use Fiddler Classic, FiddlerCap, or FiddlerAnywhere to collect a trace (decrypt traffic) on the computer where the issue is seen. Save the result as a SAZ file, and transfer to your own computer.
2. Review the result (SAZ) file: On your own computer install Fiddler Classic, install the extension, and open the SAZ file.

## How The Extension Alters The Standard Fiddler UI

* Colourisation of sessions.
* Add column 'Elapsed Time'.
* Add column 'Response Server'.
* Add column 'Session Type'.
* Add column 'Host IP'.
* Add column 'Authentication'.
* Add an 'Office 365' response inspector tab. - Look for Session Analysis, for helpful information on any given session.
* Add an 'Office 365' menu to turn off/on extension and extension features.

## Known limitations
* Extension only alters, enhances sessions when loaded from a SAZ file.
* Extension does not act on live captured sessions. In this scenario, session analysis can be inaccurate.
* Extension does not act on import, such as importing a HTTP archive. However, a HAR file can be imported, saved as a SAZ file, then loaded for the extension to work.

## Project Links

Wiki: <a href="https://aka.ms/O365FiddlerExtensionWiki" target="_blank">https://aka.ms/O365FiddlerExtensionWiki</a>

Issues: <a href="https://aka.ms/O365FiddlerExtensionIssues" target="_blank">https://aka.ms/O365FiddlerExtensionIssues</a>

Download: <a href="https://aka.ms/O365FiddlerExtension" target="_blank">https://aka.ms/O365FiddlerExtension</a>

EHLO Technet blog post: <a href="https://techcommunity.microsoft.com/t5/exchange-team-blog/introducing-the-exchange-online-fiddler-extension/ba-p/608788" target="_blank">EHLO Blog Article</a>
