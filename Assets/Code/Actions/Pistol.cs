using System.Collections;
using UnityEngine;

public sealed class Pistol : Item
{
	public override ItemType ItemType => ItemType.Pistol;

	[SerializeField] private int clipSize = 6;
	[SerializeField] private float shootTime = 0.2f;
	[SerializeField] private float reloadTime = 1f;
	[SerializeField] private float range = 100f;
	[SerializeField] private float damage = 10f;
	public Transform shootPosition;
	[SerializeField] private GameObject pistolVisual;

	private int currentAmmo;
	private float nextShootTime;
	private bool isReloading;

	private void Awake()
	{
		currentAmmo = clipSize;
		pistolVisual.SetActive(false);
	}

	public override void Activate()
	{
		pistolVisual.SetActive(true);
	}

	public override void Deactivate()
	{
		pistolVisual.SetActive(false);
	}

	public override void PrimaryAction()
	{
		if (isReloading)
		{
			return;
		}

		if (currentAmmo <= 0)
		{
			StartReload();
			return;
		}

		if (Time.time < nextShootTime)
		{
			return;
		}

		nextShootTime = Time.time + shootTime;
		currentAmmo--;

		Shoot();
	}

	public override void SecondaryAction()
	{
		StartReload();
	}

	private void Shoot()
	{
		Debug.Log("Shoot pistol");

		Ray ray = new Ray(shootPosition.position, shootPosition.forward);
		LayerMask layer = LayerMask.GetMask("Player");
		
		if (!Physics.Raycast(ray, out RaycastHit hit, range, ~layer))
		{
			return;
		}

		IDamageable damageable = hit.collider.GetComponent<IDamageable>();

		if (damageable != null)
		{
			damageable.TakeDamage(damage);
			Debug.Log("Hit: " + hit.collider.name);
		}
	}

	private void StartReload()
	{
		if (isReloading || currentAmmo == clipSize)
		{
			return;
		}

		StartCoroutine(Reload());
	}

	private IEnumerator Reload()
	{
		isReloading = true;

		Debug.Log("Reloading");

		yield return new WaitForSeconds(reloadTime);

		currentAmmo = clipSize;
		isReloading = false;

		Debug.Log("Reload complete");
	}

	public override void PrimaryActionReleased() { }

	public override void SecondaryActionReleased() { }
    
    public override void Release()
    {
    }
}