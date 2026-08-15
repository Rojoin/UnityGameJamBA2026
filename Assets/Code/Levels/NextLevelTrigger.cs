using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        if (_levelManager == null)
            Debug.LogError("No level manager provided");

        _boxCollider = GetComponent<BoxCollider>();
      
        _boxCollider.isTrigger = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        _levelManager.NextLevel();
    }
}