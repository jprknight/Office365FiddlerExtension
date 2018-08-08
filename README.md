# EXOFiddlerInspector
The Exchange Online Fiddler Inspector

Created in Visual Studio 2017 this inspector is intended to help with troubleshooting issues with Outlook and Office 365 / Exchange Online. Typically this inspector will be installed into Fidler and used to review traffic after reproducing an issue.

Project goals:

* Colourise sessions in session list. Highlight sessions of interest in typical traffic light style.
  * Red -- Typically will be HTTP 403 and HTTP 5xx response codes.
  * Orange -- Typically will be HTTP 401 unauthorised response codes. Something expected, though need to be accomanied by a subsequent authentication sucess in a HTTP 200 response.
  * Green -- Typically will be anything which could be a false positive.
  * Non-colourised -- Sessions not directly related to Outlook or Exchange. Not looking to colourise every session.
* Add an inspector tab to show request and response information:
  * Request hostname, URL, type.
  * Response code, status code short description.
  * Request start and end, with duration.
  * Comments/alerts on sessions such as:
    * HTTP 403 Forbidden.
    * HTTP 502 Service Unavailable.
    * HTTP 200 Search for errors lurking in what are OK server responses.
  * Data freshness. Information on how old the trace is being looked at.
  
* More to come.

Installer for the inspector to come, most likely when the inspector is ready for prime time. For now if you are interested, build by cloning the project.
