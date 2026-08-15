using UnityEngine;

public enum ItemType
{
	None,
	Hands,
	Pistol,
	Rock,
	Paper,
	Scissors,
	Shield,
	Box
}

public interface IItem
{
	ItemType ItemType { get; }

	void PrimaryAction();
	void SecondaryAction();
}

public abstract class Item : MonoBehaviour, IItem
{
	public abstract ItemType ItemType { get; }

	public abstract void PrimaryAction();
	public abstract void SecondaryAction();
}