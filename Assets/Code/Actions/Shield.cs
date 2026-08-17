
using System;
using UnityEngine;

public class Shield : Item
{
    [SerializeField] private float parryDuration = 0.1f;
    [SerializeField] private GameObject shieldGO;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform coverPosition;

    private bool isBlocking = false;
    private float parryStartTime;

    public override ItemType ItemType => ItemType.Shield;

    public override void Activate()
    {
        shieldGO.SetActive(true);
        SetShieldParent(hand);
    }

   
    public override void Deactivate()
    {
        shieldGO.SetActive(false);
        isBlocking = false;
    }

    public override void PrimaryAction()
    {
        //cover/parry
        parryStartTime = Time.time;
        isBlocking = true;
        SetShieldParent(coverPosition);
        Debug.Log("Blocking");
    }

    public override void PrimaryActionReleased()
    {
        isBlocking = false;
        SetShieldParent(hand);
        Debug.Log("Blocking released");
    }

    public override void Release()
    {
        isBlocking = false;
    }

    public override void SecondaryAction()
    {

    }

    public override void SecondaryActionReleased()
    {

    }

    private bool IsParrying()
    {
        return isBlocking && Time.time - parryStartTime <= parryDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        IBlockable blockable = other.GetComponentInParent<IBlockable>();
        IParryable parryable = other.GetComponentInParent<IParryable>();
        
        if (parryable != null && IsParrying())
        {
            parryable.ReactToParry();
            Debug.Log("Parry");
            return;
        }

        if (blockable != null && isBlocking)
            blockable.ReactToBlock();
    }

    private void SetShieldParent(Transform parent)
    {
        shieldGO.transform.SetParent(parent);
        shieldGO.transform.localPosition = Vector3.zero;
        shieldGO.transform.localRotation = Quaternion.identity;
    }
}


public interface IParryable
{
    void ReactToParry();
}

public interface IBlockable
{
    void ReactToBlock();
}