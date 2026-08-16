using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProjectileDetector : MonoBehaviour
{
    public Action OnProjectileDetected;

    private void Awake()
    {
        if (!TryGetComponent<SphereCollider>(out var collider))
        { 
            Debug.LogError("No collider added to projectile detector");
            return;
        }

        collider.isTrigger = true;
    }

    //It assumes it's a projectile, the player should not be able to collide
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == transform.parent || other.transform.parent == transform.parent || other.transform.parent == transform)
            return;

        OnProjectileDetected?.Invoke();
        Debug.Log("Trigger enter projectile");
    }
}
