using System;
using UnityEngine;

public abstract class PlayerRespawnable : MonoBehaviour
{
    public Action OnPlayerRespawnCondition { get; set; }
}