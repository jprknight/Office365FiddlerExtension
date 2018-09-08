# EXOFiddlerInspector
The Exchange Online Fiddler Inspector

Created in Visual Studio 2017 this inspector is intended to help with troubleshooting issues with Outlook and Office 365 / Exchange Online. This inspector is intended to be installed as an addition to Fiddler and used to review traffic while or after reproducing an issue.

Project goals:

* Colourise sessions in session list. Highlight sessions of interest in typical traffic light style.
  * Red -- Something bad, highlighted to draw attention. Examples are HTTP 403 and HTTP 5xx response codes.
  * Orange -- Perhaps something expected, but still drawing attention. An example is HTTP 401 unauthorised response codes. Something expected, though need to be accomanied by a subsequent authentication sucess in a HTTP 200 response.
  * Blue -- Something which could be mis-interpretted as bad, but is a false positive or by design. An example is Autodiscover attempts to Exchange Online endpoints which are not accepted on port 443, by design the same host only responds on port 80, and redirects secure Autodiscover request to another host which does respond on port 443.
  * Green -- Expected to be anything which is given the all clear by the inspector.
  * Gray -- Expected to be anything which is not directly related to Outlook or Exchange.
* Add an inspector tab to show request and response information:
  * Request hostname, URL, type.
  * Response code, status code short description, response server.
  * Request start and end, with duration.
  * Comments/alerts on sessions such as:
    * HTTP 403 Forbidden.
    * HTTP 502 Service Unavailable.
    * HTTP 200 Search for errors lurking in what are OK server responses.
  * Data freshness. Information on how old the trace is being looked at.
  
* More to come.
