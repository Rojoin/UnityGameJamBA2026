using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private List<PlayerRespawnable> playerRespawnables;

    private void Awake()
    {
        foreach (PlayerRespawnable respawnable in playerRespawnables)
            respawnable.OnPlayerRespawnCondition += RespawnPlayer;
    }

    private void RespawnPlayer()
    {
        playerTransform.position = respawnPosition.position;
        playerTransform.rotation = respawnPosition.rotation;
    }
}
