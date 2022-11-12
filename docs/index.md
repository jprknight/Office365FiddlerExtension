## Office 365 Fiddler Extension

### What Does it do?

This Fiddler Extension is an Office 365 centric parser to efficiently troubleshoot Office 365 client application connectivity and functionality.

### Functionality Breakdown
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

### How to Use the Extension?

1. Reproduce an issue / behaviour: Use Fiddler Classic, FiddlerCap, or FiddlerAnywhere to collect a trace (decrypt traffic) on the computer where the issue is seen. Save the result as a SAZ file, and transfer to your own computer.
2. Review the result (SAZ) file: On your own computer install Fiddler Classic, install the extension, and open the SAZ file.

### How to Get the Extension

`Invoke-Expression (New-Object Net.WebClient).DownloadString('https://aka.ms/Deploy-Office365FiddlerExtension')`

You can use the [editor on GitHub](https://github.com/jprknight/Office365FiddlerExtension/edit/master/docs/index.md) to maintain and preview the content for your website in Markdown files.

Whenever you commit to this repository, GitHub Pages will run [Jekyll](https://jekyllrb.com/) to rebuild the pages in your site, from the content in your Markdown files.

### Markdown

Markdown is a lightweight and easy-to-use syntax for styling your writing. It includes conventions for

```markdown
Syntax highlighted code block

# Header 1
## Header 2
### Header 3

- Bulleted
- List

1. Numbered
2. List

**Bold** and _Italic_ and `Code` text

[Link](url) and ![Image](src)
```

For more details see [GitHub Flavored Markdown](https://guides.github.com/features/mastering-markdown/).

### Jekyll Themes

Your Pages site will use the layout and styles from the Jekyll theme you have selected in your [repository settings](https://github.com/jprknight/Office365FiddlerExtension/settings/pages). The name of this theme is saved in the Jekyll `_config.yml` configuration file.

### Support or Contact

Having trouble with Pages? Check out our [documentation](https://docs.github.com/categories/github-pages-basics/) or [contact support](https://support.github.com/contact) and we’ll help you sort it out.
