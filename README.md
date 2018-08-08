# EXOFiddlerInspector
The Exchange Online Fiddler Inspector

Created in Visual Studio 2017 this inspector is intended to help with troubleshooting issues with Outlook and Office 365 / Exchange Online.

Project goals:

* Colourise sessions in session list. Highlight sessions of interest in typical traffic light style.
  * Red -- Typically will be HTTP 403 and HTTP 5xx response codes.
  * Orange -- Typically will be HTTP 401 response codes. Something expected, though need to be accomanied by a subsequent authentication sucess.
  * Green -- Typically will be anything which could be a false positive.
  * Non-colourised -- Sessions not directly related to Outlook or Exchange; De-emphasis these.
* Add an inspector tab to show request and response information:
  * Request hostname, URL, type.
  * Response code, status code short description.
  * Request start and end, with duration.
  * Comments on sessions such as:
    * HTTP 403 Forbidden.
    * HTTP 502 Service Unavailable.
    * HTTP 200 Search for errors lurking in what are OK server responses.
* More to come.

Installer for the inspector to come, for now if you are interested in the project build by cloning the project.
