using System;
using Code.Player;
using UnityEngine;

public class Hands : Item
{
	public override ItemType ItemType => ItemType.Hands;
	public ThrowableItem currentlyHoldingObject;
	public Transform itemHolder;
	[SerializeField] private float radius;
	[SerializeField] private float throwForce;
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
			if (Physics.Raycast(transform.position, transform.forward * radius, out RaycastHit hit ))
			{
				Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
				if (hit.collider.TryGetComponent(out ThrowableItem item))
				{
					if (item.itemType == ItemType.None)
					{
						currentlyHoldingObject = item;
						currentlyHoldingObject.Take();
						currentlyHoldingObject.transform.SetParent(itemHolder);
						currentlyHoldingObject.transform.localPosition = Vector3.zero;
					}
					else
					{
						player.AddItemToInventory(ItemFactory.Instance.CreateItem(item.itemType));
						Destroy(hit.collider.gameObject);
					}
				}	
			}
		}
		else
		{
			currentlyHoldingObject.transform.SetParent(null);
			currentlyHoldingObject.ThrowItem(Vector3.up+ transform.forward *throwForce);
			currentlyHoldingObject = null;
		}
		
	}

    public override void PrimaryActionReleased()
    {

    }

    public override void SecondaryAction()
	{
	}

    public override void SecondaryActionReleased()
    {

    }

    private void OnDrawGizmos()
    {
	    Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position + transform.forward,radius);
    }
}