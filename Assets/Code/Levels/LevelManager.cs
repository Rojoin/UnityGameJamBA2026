using System;
using System.Collections.Generic;
using Code;
using Code.Player;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private List<Level> _levels = new();
	private Dictionary<int, Level> _idLevels = new();
	private int _currentLevel = 0;
	private int _startLevelID = 0;
	public Action OnEnd;
	public List<LevelOptions> levelOptions;
	public CanvasGroup loseCanvas;
	public CanvasGroup winCanvas;
	public Enemy enemy;
	public Player player;

	private void Awake()
	{
		if (_levels == null)
		{
			Debug.LogError("No levels scriptable objects provided");
			return;
		}

		foreach (Level level in _levels)
			_idLevels.Add(level.Id, level);

		_currentLevel = _startLevelID;

		if (_idLevels.ContainsKey(_currentLevel))
			_idLevels[_startLevelID].OnEnter();
		//else
		//    Debug.LogError($"There's no first level of ID {_startLevelID}");

		for (int index = 0; index < levelOptions.Count; index++)
		{
			LevelOptions levelOption = levelOptions[index];
			int index1 = index;
			levelOption.first.OnItemTake += () => DeactivateItemsInLevel(index1);
		}

		enemy.OnDeath += YouWin;
		player.OnDeath += YouLose;
		loseCanvas.alpha = 0;
		loseCanvas.interactable = false;
		loseCanvas.blocksRaycasts = false;
		winCanvas.alpha = 0;
		winCanvas.interactable = false;
		winCanvas.blocksRaycasts = false;
	}

	private void YouLose()
	{
		loseCanvas.alpha = 1;
	}
	private void YouWin()
	{
		winCanvas.alpha = 1;
	}

	public void NextLevel()
	{
		int previousLevel = _currentLevel;
		_currentLevel++;
		if (_idLevels.ContainsKey(_currentLevel))
		{
			_idLevels[_currentLevel].OnEnter();
			Debug.Log($"Next level! Previous: {previousLevel}. Current level: {_currentLevel}");
		}
		else
		{
			OnEnd?.Invoke();
			Debug.Log($"No next level found of ID {_currentLevel}. Ending!");
		}
	}

	private void DeactivateItemsInLevel(int levelID)
	{
		levelOptions[levelID].Deactivate();
	}

	private void OnDestroy()
	{
		enemy.OnDeath -= YouWin;
		player.OnDeath -= YouLose;
	}
}

[Serializable]
public class LevelOptions
{
	public ThrowableItem first;
	public ThrowableItem second;

	public void Deactivate()
	{
		if (first != null)
		{
			first.gameObject.SetActive(false);
		}

		if (second != null)
		{
			second.gameObject.SetActive(false);
		}
	}
}