using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class Rock : Item
{
	[SerializeField] private Transform hand;
	[SerializeField] private float throwForce = 20f;
	[SerializeField] private float returnSpeed = 25f;
	[SerializeField] private float catchDistance = 0.5f;

	private Rigidbody rigidbody;
	private bool isThrown;
	private bool isReturning;

	public override ItemType ItemType => ItemType.Rock;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.isKinematic = true;
	}

	private void Update()
	{
		if (!isReturning)
		{
			return;
		}

		Vector3 direction = hand.position - transform.position;

		rigidbody.linearVelocity = direction.normalized * returnSpeed;

		if (direction.sqrMagnitude <= catchDistance * catchDistance)
		{
			Catch();
		}
	}

	public override void Activate()
	{
		if (isThrown || isReturning)
		{
			return;
		}
		
		gameObject.SetActive(true);
		
		rigidbody.isKinematic = true;

		transform.SetParent(hand);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	public override void Deactivate()
	{
		if (isThrown || isReturning)
		{
			return;
		}

		gameObject.SetActive(false);
		rigidbody.isKinematic = true;
	}

	public override void PrimaryAction()
	{
		if (isThrown || isReturning)
		{
			return;
		}

		isThrown = true;

		transform.SetParent(null);

		rigidbody.isKinematic = false;
		rigidbody.linearVelocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		rigidbody.AddForce(
			hand.forward * throwForce,
			ForceMode.Impulse
		);
	}

	public override void SecondaryAction()
	{
		if (!isThrown || isReturning)
		{
			return;
		}

		isReturning = true;

		rigidbody.isKinematic = false;
		rigidbody.linearVelocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

	private void Catch()
	{
		isThrown = false;
		isReturning = false;

		rigidbody.linearVelocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.isKinematic = true;

		transform.SetParent(hand);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

    public override void PrimaryActionReleased()
    {

    }

    public override void SecondaryActionReleased()
    {

    }
}