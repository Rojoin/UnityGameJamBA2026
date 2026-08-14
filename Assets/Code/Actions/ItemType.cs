using System;
using System.Collections.Generic;
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

public interface IInventorySlot
{
    IItem Item { get; }


    void AssignItem(IItem item);
    void PrimaryAction();
    void SecondaryAction();
    void OnEquip();
}


public interface IItem
{
    ItemType ItemType { get; }

    void Grab();
    void Throw();

}

public class Item : MonoBehaviour, IItem
{
    protected ItemType type;

    public ItemType ItemType => type;


    public Item(ItemType type)
    {
        this.type = type;
    }


    public virtual void Grab()
    {

    }

    public virtual void Throw()
    {

    }
}

public class Rock : Item
{
    public Rock() : base(ItemType.Rock)
    {
    }
}