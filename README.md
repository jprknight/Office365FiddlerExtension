## Download

### Deployment Script
The best way to get the Office 365 Fiddler Extension is via the deployment script. Run the below in PowerShell on your computer: 

`Invoke-Expression (New-Object Net.WebClient).DownloadString('https://aka.ms/Deploy-Office365FiddlerExtension')`

## Extension Purpose
This Fiddler Extension is an Office 365 centric parser to efficiently troubleshoot Office 365 client application connectivity and functionality.

## How To Use The Extension
1. Reproduce an issue / behaviour: Use Fiddler Classic, FiddlerCap, or FiddlerAnywhere to collect a trace (decrypt traffic) on the computer where the issue is seen. Save the result as a SAZ file, and transfer to your own computer.
2. Review the result (SAZ) file: On your own computer install Fiddler Classic, install the extension, and open the SAZ file.

## Functionality Breakdown

* Colourisation of sessions -- Think traffic lights, with some extras.
** Red -- Something red is really broken, start here first.
** Black -- Something might be broken, but these may not be the underlying cause.
** Orange -- Something which may be a concern, see Session Analysis on the response inspector for details.
** Blue -- False positive detected, most prominiently HTTP 502's, see Session Analysis on the response inspector for details.
** Green -- Nothing bad detected.

* Add an 'Office 365' response inspector tab. - Look for Session Analysis, for helpful information on any given session.
* Add an 'Office 365' menu to turn off/on extension and extension features.

* Add column 'Elapsed Time' -- The roundtrip time for the request/response.
* Add column 'Response Server' -- What kind of device / server responded to the request.
* Add column 'Session Type' -- What kind of session was detected.
* Add column 'Host IP' -- IP address of the device / server which responded.
* Add column 'Authentication' -- Authentication details detected in the session.

## Known limitations
* Extension only alters, enhances sessions when loaded from a SAZ file.
* Extension does not act on live captured sessions. In this scenario, session analysis can be inaccurate.
* Extension does not act on import, such as importing a HTTP archive. However, a HAR file can be imported, saved as a SAZ file, then loaded for the extension to work.

## Project Links

Wiki: <a href="https://aka.ms/O365FiddlerExtensionWiki" target="_blank">https://aka.ms/O365FiddlerExtensionWiki</a>

Issues: <a href="https://aka.ms/O365FiddlerExtensionIssues" target="_blank">https://aka.ms/O365FiddlerExtensionIssues</a>

Download: <a href="https://aka.ms/O365FiddlerExtension" target="_blank">https://aka.ms/O365FiddlerExtension</a>

EHLO Technet blog post: <a href="https://techcommunity.microsoft.com/t5/exchange-team-blog/introducing-the-exchange-online-fiddler-extension/ba-p/608788" target="_blank">EHLO Blog Article</a>
