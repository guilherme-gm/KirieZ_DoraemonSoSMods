---
title: Creating Items
---

New items may be created using [ItemHelper](../../kzModUtils-Reference/ItemModule/ItemHelper.md).
We have to supply it with a `CustomItemConfig` instance defining
our new item.

For example:

```C#
public static CustomItem FishbookItem;

private void Awake()
{
	var bookSprite = Assets.LoadAsset<Sprite>("Fishbook_Icon");

	var fishbook = new CustomItemConfig() {
		ModItemID = "fishbook.book",
		Name = "Fish book",
		Description = "A book containing details about fishes you have catched.",
		IsImportant = true,
		Category = Define.Item.CategoryEnum.ConsumptionTool,
		Sprite = ResourceUtils.RegisterSprite(bookSprite),
		ResourceId = 0,
	};

	ItemHelper.Instance.RegisterItem(fishbook, (item) => {
		FishbookItem = item;
	});
}
```

This will register a item identified by `fishbook.book`, and once registered into
the game's logic, `CustomItem FishbookItem` will contain a reference to this item
model.

Several places in the game's logic requires a `ItemMasterModel`, and `CustomItem`
is a child class of `ItemMasterModel`.
