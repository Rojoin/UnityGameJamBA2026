using System;
using UnityEngine;

namespace Code
{
	public class Enemy : MonoBehaviour, IDamageable
	{
		[Header("Target")] [SerializeField] private Transform player;
		[Header("Movement")] [SerializeField] private float followRadius = 10f;
		[SerializeField] private float attackDistance = 5f;
		[SerializeField] private float moveSpeed = 3f;
		[SerializeField] private float rotationSpeed = 8f;
		[Header("Floating")] [SerializeField] private float floatHeight = 0.5f;
		[SerializeField] private float floatSpeed = 2f;
		[Header("Attack")] [SerializeField] private float attackCooldown = 2f;
		[SerializeField] private float attackDamage = 10f;
		[SerializeField] private float attackRayDistance = 20f;
		[SerializeField] private LayerMask attackMask = ~0;
		[Header("Stun")] [SerializeField] private float stunDuration = 2f;
		[Header("Animation")] [SerializeField] private Animator animator;
		private float nextAttackTime;
		private float stunTimer;
		private float baseY;
		private static readonly int Attack = Animator.StringToHash("Attack");
		private static readonly int Stunned = Animator.StringToHash("IsDamaged");
		private static readonly int IsDead = Animator.StringToHash("IsDead");
		public float Health => currentHealth;
		private float MaxHealth = 100f;
		private float currentHealth;

		public Action OnDeath;

		private void Start()
		{
			baseY = transform.position.y;
			currentHealth = MaxHealth;
			if (player == null)
			{
				GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

				if (playerObject != null)
					player = playerObject.transform;
			}
		}

		private void Update()
		{
			if (player == null)
				return;

			if (stunTimer > 0f)
			{
				UpdateStun();
				return;
			}

			UpdateFloat();
			UpdateBehaviour();
		}

		private void UpdateBehaviour()
		{
			Vector3 direction = player.position - transform.position;

			float distance = direction.magnitude;

			if (distance > followRadius)
			{
				SetIdleAnimation();
				return;
			}

			RotateTowards(player.position);

			if (distance > attackDistance)
			{
				MoveTowardsPlayer();
				SetIdleAnimation();
			}
			else
			{
				SetIdleAnimation();

				if (Time.time >= nextAttackTime)
					AttackPlayer();
			}
		}

		private void MoveTowardsPlayer()
		{
			Vector3 direction = player.position - transform.position;
			direction.y = 0f;

			if (direction.sqrMagnitude <= 0.001f)
				return;

			direction.Normalize();

			transform.position += direction * moveSpeed * Time.deltaTime;
		}

		private void RotateTowards(Vector3 target)
		{
			Vector3 direction = target - transform.position;
			direction.y = 0f;

			if (direction.sqrMagnitude <= 0.001f)
				return;

			Quaternion targetRotation = Quaternion.LookRotation(direction);

			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				targetRotation,
				rotationSpeed * Time.deltaTime
			);
		}

		private void UpdateFloat()
		{
			Vector3 position = transform.position;

			position.y = baseY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

			transform.position = position;
		}

		private void AttackPlayer()
		{
			nextAttackTime = Time.time + attackCooldown;

			if (animator != null)
				animator.SetTrigger(Attack);

			Vector3 origin = transform.position;
			Vector3 direction = player.position - origin;

			if (Physics.Raycast(
				    origin,
				    direction.normalized,
				    out RaycastHit hit,
				    attackRayDistance,
				    attackMask))
			{
				IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();

				if (damageable != null)
					damageable.TakeDamage(attackDamage);
			}

			Debug.DrawRay(
				origin,
				direction.normalized * attackRayDistance,
				Color.red,
				1f
			);
		}

		public void Stun()
		{
			stunTimer = stunDuration;

			if (animator != null)
				animator.SetBool(Stunned, true);
		}

		public void Stun(float duration)
		{
			stunTimer = duration;

			if (animator != null)
				animator.SetBool(Stunned, true);
		}

		private void UpdateStun()
		{
			stunTimer -= Time.deltaTime;

			if (stunTimer <= 0f)
			{
				stunTimer = 0f;

				if (animator != null)
					animator.SetBool(Stunned, false);
			}
		}

		private void SetIdleAnimation()
		{
			if (animator != null)
				animator.SetBool(Stunned, false);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, followRadius);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackDistance);

			if (player != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(
					transform.position,
					player.position
				);
			}
		}

		public void TakeDamage(float damage)
		{
			currentHealth -= damage;
			if (Health <= 0)
			{
				animator.SetBool(IsDead, true);
				
				Invoke(nameof(Death),1f);
			}
		}

		private void Death()
		{
			OnDeath.Invoke();
			Destroy(this.gameObject);
		}
	}
}