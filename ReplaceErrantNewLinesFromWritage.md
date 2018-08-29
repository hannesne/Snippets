Using the [writage](http://www.writage.com/) plugin for Microsoft Word for editing your markdown is super awesome. There’s one problem for me: it introduces line breaks in your document in the middle of paragraphs to limit the width of a paragraph. Most editors just use word wrap, but with these line breaks, your markdown won’t flow properly. It’s not too much of an issue, but it annoys me.

To fix this, open the doc in VS Code, then open the find and replace dialog.
Switch it to match regex. Then enter in the find string:

    ([a-zA-Z])[\n\r]([a-zA-Z])

And enter in the replace screen:

    $1 $2

The parenthesis marks the contents as a Capturing group, and the \$ references that Capturing group.

Thanks to this [Stack Overflow answer](https://stackoverflow.com/questions/14458160/while-replacing-using-regex-how-to-keep-a-part-of-matched-string)
and [Regex 101](https://regex101.com/)

