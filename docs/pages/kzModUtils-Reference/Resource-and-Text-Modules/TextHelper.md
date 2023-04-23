---
title: TextHelper
---

`TextHelper` is the main Text Module entry point.

This static class allows modders to create new Text entries in the game library.

## Methods

### `void RegisterText(string text, OnTextRegistered callback)`
Registers `text` into TextMaster and calls `callback` with the new id.

**Example:**

```C#
TextData.TextHelper.RegisterText(
	"My Item Name",
	(int id) => { itemConfig.NameId = id; }
);
```

