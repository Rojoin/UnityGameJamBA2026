using Code.Player;
using System;
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
    public Action<IItem> OnItemReleased { get; set; }

    void Activate();
    void Deactivate();
    void PrimaryAction();
    void PrimaryActionReleased();
    void SecondaryAction();
    void SecondaryActionReleased();
}

public abstract class Item : MonoBehaviour, IItem
{
    public abstract ItemType ItemType { get; }

    public Action<IItem> OnItemReleased { get; set; }

    public abstract void Activate();
    public abstract void Deactivate();

    public abstract void PrimaryAction();
    public abstract void PrimaryActionReleased();
    public abstract void SecondaryAction();
    public abstract void SecondaryActionReleased();
    public abstract void Release();

    protected Player player;

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
}
