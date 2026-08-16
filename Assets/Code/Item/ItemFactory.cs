using System;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
	private static ItemFactory instance;

	[SerializeField] private Rock rock;
	[SerializeField] private Pistol pistol;
	[SerializeField] private Scissors scissors;
	[SerializeField] private Shield shield;
	[SerializeField] private Paper paper;
	[SerializeField] private Box box;

	public static ItemFactory Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindAnyObjectByType<ItemFactory>();
			}

			return instance;
		}
	}

	public Item CreateItem(ItemType itemType)
	{
		switch (itemType)
		{
			case ItemType.None:
				Debug.LogError("Cannot create an item of type None.");
				return null;

			case ItemType.Hands:
				return null;

			case ItemType.Pistol:
				return Instantiate(pistol);

			case ItemType.Rock:
				return Instantiate(rock);

			case ItemType.Paper:
				return Instantiate(paper);

			case ItemType.Scissors:
				return Instantiate(scissors);

			case ItemType.Shield:
				return Instantiate(shield);

			case ItemType.Box:
				return Instantiate(box);

			default:
				throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
		}
	}
}