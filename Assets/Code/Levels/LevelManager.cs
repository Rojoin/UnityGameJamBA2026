using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> _levels = new();
    private Dictionary<int, Level> _idLevels = new();

    private int _currentLevel = 0;
    private int _startLevelID = 0;

    public Action OnEnd;

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

}
