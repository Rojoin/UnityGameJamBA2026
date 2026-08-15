public class Hands : Item
{
	public override ItemType ItemType => ItemType.Hands;

	public override void PrimaryAction()
	{
		// Pick up object
	}

	public override void SecondaryAction()
	{
		// Drop or throw held object
	}
}