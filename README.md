## Extension Purpose

This Fiddler Extension is an Office 365 centric parser to efficiently troubleshoot Office 365 client application connectivity and functionality.

## How To Use The Extension

1. Reproduce an issue / behaviour: Use Fiddler Classic, FiddlerCap, or FiddlerAnywhere to collect a trace (decrypt traffic) on the computer where the issue is seen. Save the result as a SAZ file, and transfer to your own computer.
2. Review the result (SAZ) file: On your own computer install Fiddler Classic, install the extension, and open the SAZ file.

## Deployment Script

The best way to get the Office 365 Fiddler Extension is via the deployment script. Run the below in PowerShell on your computer: 

`Invoke-Expression (New-Object Net.WebClient).DownloadString('https://aka.ms/Deploy-Office365FiddlerExtension')`

![Office 365 Fiddler Extension Deployment Script](https://github.com/jprknight/Office365FiddlerExtension/blob/master/docs/Office365FiddlerExtensionDeploymentScript.png)

## Functionality Breakdown

### Colourisation of sessions
The extension enhances the default experience of Fiddler by colouring sessions in line with the session analysis performed.

Think traffic lights, with some extras.

* Red -- Something red is really broken, start here first.
* Black -- Something might be broken, but these may not be the underlying cause.
* Orange -- Something which may be a concern, see Session Analysis on the response inspector for details.
* Blue -- False positive detected, most prominiently HTTP 502's, see Session Analysis on the response inspector for details.
* Green -- Nothing bad detected.

### User Interface

* **Response Inspector Tab** - Look for Session Analysis, for helpful information on any given session.
* **Office 365 Menu** - Turn off/on extension features.

### Session Columns

Columns are added into the session view on the left side of Fiddler, scroll the view to the right if you don't immediately see them. Re-order the columns to your preferences.

* **Elapsed Time** - The roundtrip time for the request/response.
* **Response Server** - What kind of device / server responded to the request.
* **Session Type** - What kind of session was detected.
* **Host IP** - IP address of the device / server which responded.
* **Authentication** - Authentication details detected in the session.

## Known limitations

* Extension only alters, enhances sessions when loaded from a SAZ file.
* Extension does not act on live captured sessions. In this scenario, session analysis can be inaccurate.
* Extension does not act on import, such as importing a HTTP archive. However, a HAR file can be imported, saved as a SAZ file, then loaded for the extension to work.

## Project Links

**Wiki:** <a href="https://aka.ms/O365FiddlerExtensionWiki" target="_blank">https://aka.ms/O365FiddlerExtensionWiki</a>

**Issues:** <a href="https://aka.ms/O365FiddlerExtensionIssues" target="_blank">https://aka.ms/O365FiddlerExtensionIssues</a>

**Download:** <a href="https://aka.ms/O365FiddlerExtension" target="_blank">https://aka.ms/O365FiddlerExtension</a>

**EHLO Technet blog post:** <a href="https://techcommunity.microsoft.com/t5/exchange-team-blog/introducing-the-exchange-online-fiddler-extension/ba-p/608788" target="_blank">EHLO Blog Article</a>
