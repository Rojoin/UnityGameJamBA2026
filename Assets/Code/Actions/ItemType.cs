using Code.Player;
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

	void Activate();
	void Deactivate();
	
	void PrimaryAction();
	void SecondaryAction();
}

public abstract class Item : MonoBehaviour, IItem
{
	public abstract ItemType ItemType { get; }

	public abstract void Activate();
	public abstract void Deactivate();
	
	public abstract void PrimaryAction();
	public abstract void SecondaryAction();
	protected Player player;

	public void SetPlayer(Player player)
	{
		this.player = player;
	}
}
