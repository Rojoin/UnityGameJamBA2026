using UnityEngine;

public sealed class Scissors : Item
{
	[SerializeField] private GameObject scissorsVisual;
	[SerializeField] private SphereCollider attackCollider;
	[SerializeField] private float attackDuration = 0.1f;
	[SerializeField] private float attackCooldown = 0.5f;
	[SerializeField] private float damage = 1f;

	private float nextAttackTime;
	private bool isAttacking;

	public override ItemType ItemType => ItemType.Scissors;

	private void Awake()
	{
		attackCollider.enabled = false;
		scissorsVisual.SetActive(false);
	}

	public override void Activate()
	{
		scissorsVisual.SetActive(true);
	}

	public override void Deactivate()
	{
		scissorsVisual.SetActive(false);
		attackCollider.enabled = false;
		isAttacking = false;
	}

	public override void PrimaryAction()
	{
		if (isAttacking || Time.time < nextAttackTime)
		{
			return;
		}

		isAttacking = true;
		nextAttackTime = Time.time + attackCooldown;

		attackCollider.enabled = true;

		Invoke(nameof(EndAttack), attackDuration);
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

	private void OnTriggerEnter(Collider other)
	{
		if (!isAttacking)
		{
			return;
		}

		IDamageable damageable = other.GetComponent<IDamageable>();

		if (damageable == null)
		{
			return;
		}

		damageable.TakeDamage(damage);
	}

	private void EndAttack()
	{
		attackCollider.enabled = false;
		isAttacking = false;
	}
    
    public override void Release()
    {
        
    }
}