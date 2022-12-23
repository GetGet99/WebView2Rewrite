# WebView2Rewrite
WebView2 source code from `microsoft-ui-xaml` rewritten in C# and test project

# Modifications
All the code I modified from the original, I'll have commented `// MODIFIED` except one place where I fix horizontal scroll (line 422 on github)

- Fixed horizontal scroll
- Add touchpad scroll (beta, zooming is weird and not yet finished)

# Note
The use of CsWin32 nuget is required (WebView2 calls some native method)

# Warning
WinUI 3 version is not finished yet, do not use it
