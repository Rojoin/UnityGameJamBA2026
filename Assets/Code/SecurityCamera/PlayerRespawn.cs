using System;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SecurityCamera camera;

    private void Awake()
    {
        camera.OnPlayerDetected += RespawnPlayer;
    }

    private void RespawnPlayer()
    {
        playerTransform.position = respawnPosition.position;
        playerTransform.rotation = respawnPosition.rotation;
    }
}
