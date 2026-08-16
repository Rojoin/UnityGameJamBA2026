using Code.Player;
using UnityEngine;

public class Hands : Item
{
	public override ItemType ItemType => ItemType.Hands;
	public GameObject currentlyHoldingObject;
	[SerializeField] private float radius;
	public override void Activate()
	{

	}
	public override void Deactivate()
	{

	}

	public override void PrimaryAction()
	{
		// Pick up object

		if (currentlyHoldingObject == null)
		{
			if (Physics.SphereCast(transform.position + transform.forward,radius, transform.forward, out RaycastHit hit ))
			{
				if (hit.collider.TryGetComponent(out ThrowableItem item))
				{
					if (item.itemType == ItemType.None)
					{
						currentlyHoldingObject = hit.collider.gameObject;
					}
					else
					{
						player.AddItemToInventory(ItemFactory.Instance.CreateItem(item.itemType));
						Destroy(hit.collider.gameObject);
					}
				}	
			}

			
		}
		
	}

    public override void PrimaryActionReleased()
    {

    }

    public override void Release()
    {

    }

    public override void SecondaryAction()
	{
		// Drop or throw held object
	}

    public override void SecondaryActionReleased()
    {

    }
}