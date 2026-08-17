using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private List<LevelCondition> _levelConditions;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        if (_levelManager == null)
            Debug.LogError("No level manager provided");

        _boxCollider = GetComponent<BoxCollider>();
      
        _boxCollider.isTrigger = true; 
    }

    private void OnTriggerStay(Collider other)
    {
        if (!CanPassLevel())
            return;

        _levelManager.NextLevel();
        gameObject.SetActive(false);
    }

    private bool CanPassLevel()
    {
        foreach (LevelCondition level in _levelConditions)
        {
            if (level.PassedLevel())
                return false;

            if (!level.CanPassLevel())
                return false;
        }

        return true;
    }
}
