## Extension Purpose

This Fiddler Extension is an Office 365 / Microsoft 365 centric parser to efficiently troubleshoot Office 365 client application connectivity and functionality.

## Deployment Script

The best way to get the Office 365 Fiddler Extension is via the deployment script. Run the below in PowerShell on your computer: 

`Invoke-Expression (New-Object Net.WebClient).DownloadString('https://aka.ms/Deploy-Office365FiddlerExtension')`

Don't want to use the aka.ms short link, or not working? Use this link instead, it pulls directly from this Github repository:

`Invoke-Expression (New-Object Net.WebClient).DownloadString('https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/master/Office365FiddlerExtension/Deploy-Office365FiddlerExtension.ps1')`

![Office 365 Fiddler Extension Deployment Script](https://github.com/jprknight/Office365FiddlerExtension/blob/master/docs/Office365FiddlerExtensionDeploymentScript.png)

## How To Use The Extension

1. **Reproduce an issue** from the computer where it is seen:
* Use Fiddler Classic, FiddlerCap, or FiddlerAnywhere to collect a trace with <a href="https://docs.telerik.com/fiddler/configure-fiddler/tasks/decrypthttps">decrypt HTTPS traffic</a> enabled.
* Use F12 Developer tools and save a HAR file from a browser session.
* Use Network Log Export (edge://net-export/ in Edge browser). Note, with this option you'll need the <a href="https://github.com/ericlaw1979/FiddlerImportNetlog/releases/latest">Fiddler Import Netlog</a> plugin, to have the "NetLog JSON" import option in Fiddler.

2. **Review the result** on your own computer by installing Fiddler Classic, installing the extension, and...
* Open / double clicking the SAZ file. Loaded sessions are automatically analysed.
* Importing the HTTP archive. Click File, Import Sessions, "HTTPArchive" and choose your HAR file. Once loaded click The Office 365 menu item, click 'Analyse All Sessions'.
* Importing the JSON browser net trace. Click File, Import Sessions, "NetLog JSON" and choose your JSON file. Once loaded click The Office 365 menu item, click 'Analyse All Sessions'.

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
* Pink -- Something went wrong with the extension logic. This is the default fallback color. Look for errors in the Fiddler log. Open an issue on the 'Issues' tab to contact the extension author.

### User Interface

* **Response Inspector Tab** - Look for Session Analysis, for helpful information on any given session.
* **Office 365 Menu**
  * Turn extension off and on.
  * Analyse all sessions -- Analyse all sessions in the view in Fiddler. This will also fill in any sessions which do not already have session analysis.
  * Clear All Session Analysis -- Clear all session analysis values on all sessions.
  * Create Consolidated Analysis Report -- Creates a HTML report, highlighting the most interesting sessions and other statistical information from the sessions in the view in Fiddler.
  * Check IP Address -- Manually check if an IP address is in a private, public, or Microsoft 365 subnet.
* **Context Menu** - Additional options for processing sessions.
  * Analyse Selected Sessions -- Analyse selected sessions in the view in Fiddler.
  * Clear Selected Sessions -- Clear session analysis values on selected sessions in the view in Fiddler.
  * Set Session Severity -- Manually set severity or recalculate session severity with extension, on selected sessions.
  * Create Consolidated Analysis Report -- Creates a HTML report, highlighting the most interesting sessions and other statistical information from the sessions in the view in Fiddler.
 
### Session Columns

Columns are added into the session view on the left side of Fiddler, scroll the view to the right if you don't immediately see them. Re-order the columns to your preferences.

* **Severity** - Session Severity has a scale of 0 – 60. The scale of Session Severity correlates to the colourisation of sessions the extension provides:
  * 00 Pink (Something went wrong in the extension)
  * 10 Grey (Uninteresting)
  * 20 Bue (False Positive)
  * 30 Green (Normal)
  * 40 Orange (Warning)
  * 50 Black (Concerning)
  * 60 Red (Severe)
* **Elapsed Time** - The roundtrip time for the request/response.
* **Response Server** - What kind of device / server responded to the request.
* **Session Type** - What kind of session was detected.
* **Host IP** - IP address of the device / server which responded.
Assuming you don't have never web call enabled, the extension pulls from the Microsoft URLs and IPs Web Service, to tell you if a host IP is:
  * A private LAN IP address. Host IP will show similar to "LAN:10.0.0.1".
  * A public IP address. Host IP will show similar to "PUB:8.8.8.8".
  * A Microsoft 365 IP address. Host IP will show similar to "M365:40.99.10.34".
* **Authentication** - Authentication details detected in the session.

### Other Information

- The **ruleset is now contained within its own DLL file**. This means any ruleset updates can be delivered more frequently, extension updates can have a different release cadence.
- **Many ruleset updates** applied, which have accumulated since the last release in Winter 2022.
- **Error handling greatly improved**. Errors are typically logged to the Fiddler log within the application rather than throwing popup boxes.
- **Extensive use of Json** for update notifications, session information, version information, URLs, and for minor rule updates from the Github repo, all of which are automated updates the extension runs periodically.
  * URLs -- Extension URLs can be updated in the Github repo, and the extension downloads the updates: https://github.com/jprknight/Office365FiddlerExtension/blob/master/Office365FiddlerExtension/ExtensionURLs.json
  * Version -- Version information can be updated in the Github and the extension notifies of the updates: https://github.com/jprknight/Office365FiddlerExtension/blob/master/Office365FiddlerExtension/ExtensionVersion.json
  * Session Classification -- Colorisation of sessions can be updated within the Github and consumed by the extension on next update: https://github.com/jprknight/Office365FiddlerExtension/blob/master/Office365FiddlerExtension/SessionClassification.json
- **Improved performance** in the ruleset logic. Lots of coding to ensure session logic only runs once, and compute intensive code is exited from as soon as possible when not needed.
- **Session analysis is stored within flags** inside sessions. Loading a Saz file previously saved with the extension enabled will process exceptionally fast. In this scenario instead of running through the ruleset, the stored values are used.
- The extension can still be set to **never web call** for isolated environments, if it's important for you to turn these features off. -- Make sure to have SessionClassification.json in your \Fiddler\Inspectors\ folder if you want to do this. Just note you won't see any update notices and you get to use the expanded Host IP features.

## Project Links

**Wiki:** <a href="https://aka.ms/O365FiddlerExtensionWiki" target="_blank">https://aka.ms/O365FiddlerExtensionWiki</a>

**Issues:** <a href="https://aka.ms/O365FiddlerExtensionIssues" target="_blank">https://aka.ms/O365FiddlerExtensionIssues</a>

**Download:** <a href="https://aka.ms/O365FiddlerExtension" target="_blank">https://aka.ms/O365FiddlerExtension</a>

**EHLO team blog post:** <a href="https://techcommunity.microsoft.com/t5/exchange-team-blog/introducing-the-exchange-online-fiddler-extension/ba-p/608788" target="_blank">EHLO Blog Article</a>
