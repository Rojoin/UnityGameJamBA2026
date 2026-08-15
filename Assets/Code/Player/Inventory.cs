using System.Collections.Generic;

public sealed class Inventory
{
	private readonly List<IItem> items = new List<IItem>();
	private int selectedIndex;

	public IReadOnlyList<IItem> Items => items;

	public Inventory(IItem hands)
	{
		items.Add(hands);
		selectedIndex = 0;
	}

	public IItem GetSelectedItem()
	{
		return items[selectedIndex];
	}

	public void Add(IItem item)
	{
		if (item == null)
		{
			return;
		}

		items.Add(item);
	}

	public bool Remove(IItem item)
	{
		if (item == null || item == items[0])
		{
			return false;
		}

		return items.Remove(item);
	}

	public void SelectNext()
	{
		if (items.Count <= 1)
		{
			return;
		}

		selectedIndex++;

		if (selectedIndex >= items.Count)
		{
			selectedIndex = 0;
		}
	}

	public void SelectPrevious()
	{
		if (items.Count <= 1)
		{
			return;
		}

		selectedIndex--;

		if (selectedIndex < 0)
		{
			selectedIndex = items.Count - 1;
		}
	}
	
	public void Select(int index)
	{
		if (index < 0 || index >= items.Count)
		{
			return;
		}

		selectedIndex = index;
	}
}