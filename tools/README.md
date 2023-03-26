Tools for working with this repo.

## md2nexus
Converts a markdown file to a BBCode to be used in Nexus Mods.

```SH
npm run md2nexus <md path>
```

Example:
```
npm run md2nexus ../EnhancementsAndTweaks/README.md
```

The output will be on stdout

### Special considerations
1. It is only able to read real markdown (e.g. images using `<p><img>` won't work)
2. It supports some special HTML-like comments to conditionally render the BBCode
3. It converts relative paths repository URLs

### Special controls
As publishing in nexus required a few changes to the README, there are a few hacks in this tool to support conditionally adding content for each side.

Using HTML comments it is possible to define some special rules for the tool.

#### Enable/disable parts of the text

```
Some text here
<!-- nexus-disable -->
Another text here
<!-- nexus-enable -->
Yet another text here
```

Will produce the following BBCode:
```
Some text here
Yet another text here
```

Everything between `<!-- nexus-disable -->` and `<!-- nexus-enable -->` is ignored. Note that sometimes this doesn't work very well if the lexer picks it in the middle of other constructs.

#### Nexus exclusive text
Using:
```
<!-- nexus-only
BBCode you want to include in Nexus version
-->
```

Will make `BBCode you want to include in Nexus version` be included in this script result, but will be hidden in GitHubs markdown (because it is a comment :) ).
The line break after `nexus-only` and the line break before `-->` is mandatory, the code will consider the lines in between.
