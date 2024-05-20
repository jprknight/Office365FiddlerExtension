## Extension Purpose

This Fiddler Extension is an Office 365 centric parser to efficiently troubleshoot Office 365 client application connectivity and functionality.

## How To Use The Extension

1. Reproduce an issue / behaviour: Use Fiddler Classic, FiddlerCap, or FiddlerAnywhere to collect a trace (decrypt traffic) on the computer where the issue is seen. Save the result as a SAZ file, and transfer to your own computer.
2. Review the result (SAZ) file: On your own computer install Fiddler Classic, install the extension, and open the SAZ file, HTTP archive, or Json browser net trace.

## Deployment Script

The best way to get the Office 365 Fiddler Extension is via the deployment script. Run the below in PowerShell on your computer: 

`Invoke-Expression (New-Object Net.WebClient).DownloadString('https://aka.ms/Deploy-Office365FiddlerExtension')`

Don't want to use the aka.ms short link, or not working? Use this instead:

`Invoke-Expression (New-Object Net.WebClient).DownloadString('https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/master/Office365FiddlerExtension/Deploy-Office365FiddlerExtension.ps1')`

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
* Gray -- Unlikely to be of interest.

### User Interface

* **Response Inspector Tab** - Look for Session Analysis, for helpful information on any given session.
* **Office 365 Menu** - Turn off/on extension features.
* **Context Menu** - Additional options for processing sessions.

### Session Columns

Columns are added into the session view on the left side of Fiddler, scroll the view to the right if you don't immediately see them. Re-order the columns to your preferences.

* **Severity** - Numnerical value given the severity of the session (0 - 60).
* **Elapsed Time** - The roundtrip time for the request/response.
* **Response Server** - What kind of device / server responded to the request.
* **Session Type** - What kind of session was detected.
* **Host IP** - IP address of the device / server which responded.
* **Authentication** - Authentication details detected in the session.

### Extension v2 Update Notes
The extension has had a complete rewrite giving it the ability to update from the web via Json files, adding new features, and improving the code structure.

* The ruleset is now contained within its own DLL file. This means any ruleset updates can be delivered more frequently, extension updates can have a different release cadence.
* Many ruleset updates added which have accumulated since the last release in Winter 2022.
* Extensive use of Json for update notifications, session information, version information, URLs, and for minor rule updates from the Github repo, all of which are automated updates the extension runs periodically.
* Improved performance in the ruleset logic. Session analysis is also stored within flags inside sessions. Loading a Saz file previously saved with the extension enabled will process exceptionally fast upon reloading. In this scenario instead of running through the ruleset, the stored values are used.
* The extension can still be set to never web call for isolated environments, if it's important for you to turn these features off. -- Make sure to have SessionClassification.json in your \Fiddler\Inspectors\ folder if you want to do this.
* Session Severity added to the list of attributes stamped onto sessions by the extension. -- These directly correlate to the colors the extension uses on sessions.
* Session Severity has a scale of 0 – 60. As shown below the scale of Session Severity correlates to the colourisation of sessions the extension provides:
** 10 Grey (Uninteresting)
** 20 Bue (False Positive)
** 30 Green (Normal)
** 40 Orange (Warning)
** 50 Black (Concerning)
** 60 Red (Severe)

## Project Links

**Wiki:** <a href="https://aka.ms/O365FiddlerExtensionWiki" target="_blank">https://aka.ms/O365FiddlerExtensionWiki</a>

**Issues:** <a href="https://aka.ms/O365FiddlerExtensionIssues" target="_blank">https://aka.ms/O365FiddlerExtensionIssues</a>

**Download:** <a href="https://aka.ms/O365FiddlerExtension" target="_blank">https://aka.ms/O365FiddlerExtension</a>

**EHLO team blog post:** <a href="https://techcommunity.microsoft.com/t5/exchange-team-blog/introducing-the-exchange-online-fiddler-extension/ba-p/608788" target="_blank">EHLO Blog Article</a>
