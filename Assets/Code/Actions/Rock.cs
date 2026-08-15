using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rock : Item
{
	[SerializeField] private Transform hand;
	[SerializeField] private float throwForce = 20f;
	[SerializeField] private float returnSpeed = 15f;
	
	private Rigidbody rigidbody;
	private bool isThrown;
	
	public override ItemType ItemType => ItemType.Rock;
	
	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public override void PrimaryAction()
	{
		if (isThrown)
		{
			return;
		}

		isThrown = true;

		transform.SetParent(null);

		rigidbody.isKinematic = false;
		rigidbody.linearVelocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		Vector3 direction = hand.forward;

		rigidbody.AddForce(direction * throwForce, ForceMode.Impulse);
	}

	public override void SecondaryAction()
	{
		if (!isThrown)
		{
			return;
		}

		isThrown = false;

		rigidbody.linearVelocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.isKinematic = true;

		transform.SetParent(hand);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}
}