using Code.Player;
using System.Collections.Generic;
using UnityEngine;

public class RequireAnyItemCondition : LevelCondition
{
    [SerializeField] private List<ItemType> itemTypes;
    [SerializeField] private Player player;

    public override bool CanPassLevel()
    {
        if (player.Inventory.Items.Count == 1)
            return false;

        foreach (ItemType type in itemTypes)
        {
            foreach (IItem item in player.Inventory.Items)
            {
                if (item.ItemType == type)
                    return true;
            }
        }

        return false;
    }
}
