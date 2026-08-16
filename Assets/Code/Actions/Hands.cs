public class Hands : Item
{
	public override ItemType ItemType => ItemType.Hands;
	public override void Activate()
	{

	}
	public override void Deactivate()
	{

	}

	public override void PrimaryAction()
	{
		// Pick up object
	}

    public override void PrimaryActionReleased()
    {

    }

    public override void SecondaryAction()
	{
		// Drop or throw held object
	}

    public override void SecondaryActionReleased()
    {

    }
}