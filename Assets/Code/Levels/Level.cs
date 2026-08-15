using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Level
{
    [SerializeField] private int _levelId;
    [Header("GOs to activate/deactivate on level enter")]
    [SerializeField] private List<GameObject> _goToDeactivate;
    [SerializeField] private List<GameObject> _goToActivate;

    public int Id => _levelId;

    public void OnEnter()
    {
        foreach (GameObject gameObject in _goToDeactivate)
        {
            if (gameObject == null)
                Debug.LogError("Garbage gameobject found in objects to deactivate.");
            gameObject?.SetActive(false);
        }

        foreach (GameObject gameObject in _goToActivate)
        {
            if (gameObject == null)
                Debug.LogError("Garbage gameobject found in objects to activate.");
            gameObject?.SetActive(true);
        }
    }
}