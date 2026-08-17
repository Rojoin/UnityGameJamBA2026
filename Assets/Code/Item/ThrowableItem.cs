using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableItem : MonoBehaviour
{
	[SerializeField] public ItemType itemType;
	private Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void Take()
	{
		rb.useGravity = false;
		rb.isKinematic = true;
	}

	public void ThrowItem(Vector3 throwForce)
	{
		rb.useGravity = true;
		rb.isKinematic =false;
		
		rb.AddForce(throwForce, ForceMode.Impulse);
	}
}