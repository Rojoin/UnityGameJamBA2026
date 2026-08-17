using UnityEngine;

public class BombNeutralizedCondition : LevelCondition
{
    [SerializeField] private Bomb bomb;

    public override bool CanPassLevel()
    {
        return bomb.IsDefused;
    }
}