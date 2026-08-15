using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class Rock : Item
{
	[SerializeField] private Transform hand;
	[SerializeField] private Transform throwPosition;
	[SerializeField] private float throwForce = 20f;

	private Rigidbody rigidbody;
	private bool isThrown;

	public override ItemType ItemType => ItemType.Rock;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}
	
	public override void Activate()
	{

	}
	public override void Deactivate()
	{

	}

	public override void PrimaryAction()
	{
		if (isThrown)
		{
			return;
		}

		isThrown = true;

		transform.SetParent(null);
		transform.position = throwPosition.position;

		rigidbody.isKinematic = false;
		rigidbody.linearVelocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		rigidbody.AddForce(
			throwPosition.forward * throwForce,
			ForceMode.Impulse
		);
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