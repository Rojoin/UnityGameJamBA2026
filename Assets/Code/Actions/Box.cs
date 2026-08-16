
using UnityEngine;
using UnityEngine.UIElements;

public class Box : Item
{
    [SerializeField] private Transform hand;
	[SerializeField] private Transform hidePosition;
	[SerializeField] private GameObject boxGO;
	public override ItemType ItemType => ItemType.Box;
    public bool isDisguised { get; private set; }

    public override void Activate()
	{
		boxGO.SetActive(true);
        SetBoxParent(hand);
    }
	
	public override void Deactivate()
	{
		boxGO.SetActive(false);
        
    }

	public override void PrimaryAction()
    {
        SetBoxParent(hidePosition);
        isDisguised = true;
    }

    public override void PrimaryActionReleased()
    {
        SetBoxParent(hand);
        isDisguised = false;
    }

    public override void Release()
    {

    }

    public override void SecondaryAction()
	{

	}

    public override void SecondaryActionReleased()
    {

    }

    private void SetBoxParent(Transform parent)
    {
        boxGO.transform.SetParent(parent);
        boxGO.transform.localPosition = Vector3.zero;
        boxGO.transform.localRotation = Quaternion.identity;
    }

}