using System.Collections.Generic;
using UnityEngine;

public sealed class Inventory
{
	private readonly List<IItem> items = new List<IItem>();
	private int selectedIndex;

	public IReadOnlyList<IItem> Items => items;

	public Inventory(IItem hands)
	{
		Add(hands);
		selectedIndex = 0;

		items[0].Activate();
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

		item.Deactivate();
		items.Add(item);

		Debug.Log("Item Added " + item.ItemType);
	}

	public bool Remove(IItem item)
	{
		if (item == null || item == items[0])
		{
			return false;
		}

		int index = items.IndexOf(item);

		if (index < 0)
		{
			return false;
		}

		if (index == selectedIndex)
		{
			item.Deactivate();
		}

		items.RemoveAt(index);

		if (selectedIndex >= items.Count)
		{
			selectedIndex = items.Count - 1;
		}

		Debug.Log("Item Removed " + item.ItemType);

		return true;
	}

	public void SelectNext()
	{
		if (items.Count <= 1)
		{
			return;
		}

		int nextIndex = selectedIndex + 1;

		if (nextIndex >= items.Count)
		{
			nextIndex = 0;
		}

		Select(nextIndex);
	}

	public void SelectPrevious()
	{
		if (items.Count <= 1)
		{
			return;
		}

		int previousIndex = selectedIndex - 1;

		if (previousIndex < 0)
		{
			previousIndex = items.Count - 1;
		}

		Select(previousIndex);
	}

	public void Select(int index)
	{
		if (index < 0 || index >= items.Count || index == selectedIndex)
		{
			return;
		}

		items[selectedIndex].Deactivate();

		selectedIndex = index;

		items[selectedIndex].Activate();

		Debug.Log("Current selected item " + items[selectedIndex].GetType().Name);
	}
}