using UnityEngine;

public class BombNeutralizedCondition : LevelCondition
{
    [SerializeField] private Bomb bomb;
    private bool passedLevel = false;

    public override bool CanPassLevel()
    {
        if (bomb.IsDefused)
            passedLevel = true;

        return bomb.IsDefused;
    }

    public override bool PassedLevel()
    {
        return passedLevel;
    }
}