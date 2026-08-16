using System;
using UnityEngine;

public sealed class BombCable : MonoBehaviour, IDamageable
{
	[SerializeField] private float health = 1f;

	public event Action<BombCable> Cut;

	public float Health => health;

	public void TakeDamage(float damage)
	{
		if (health <= 0f)
		{
			return;
		}

		health -= damage;

		Debug.Log("Cable damaged");

		if (health <= 0f)
		{
			health = 0f;
			Cut?.Invoke(this);

			Debug.Log("Cable cut");
		}
	}
}