using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bomb : MonoBehaviour
{
	[SerializeField] private float countdown = 10f;
	[SerializeField] private List<BombCable> cables = new List<BombCable>();

	private bool isActive;
	private bool isDefused;

	private void Awake()
	{
		for (int i = 0; i < cables.Count; i++)
		{
			cables[i].Cut += OnCableCut;
		}
		
		StartBomb();
	}

	private void OnDestroy()
	{
		for (int i = 0; i < cables.Count; i++)
		{
			cables[i].Cut -= OnCableCut;
		}
	}

	public void StartBomb()
	{
		if (isActive || isDefused)
		{
			return;
		}

		StartCoroutine(Countdown());
	}

	private IEnumerator Countdown()
	{
		isActive = true;

		Debug.Log("Bomb activated");

		yield return new WaitForSeconds(countdown);

		if (!isDefused)
		{
			Explode();
		}
	}

	private void OnCableCut(BombCable cable)
	{
		if (!isActive || isDefused)
		{
			return;
		}

		Debug.Log("Bomb cable cut");

		for (int i = 0; i < cables.Count; i++)
		{
			if (cables[i].Health > 0f)
			{
				return;
			}
		}

		Defuse();
	}

	private void Defuse()
	{
		isDefused = true;
		isActive = false;

		Debug.Log("Bomb defused");
	}

	private void Explode()
	{
		isActive = false;

		Debug.Log("Bomb exploded");
	}
}