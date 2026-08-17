using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour
	{
		private Inventory inventory;

		[SerializeField] private Hands hands;

		//TODO: SACAR ESTO PORQUE LA PISTOLA SE RECOGE
		//[SerializeField] private Rock rock;

		[SerializeField] private InputActionReference movementInputAction;
		[SerializeField] private InputActionReference cameraMovementInputAction;
		[SerializeField] private InputActionReference clickInputAction;
		[SerializeField] private InputActionReference secondClickInputAction;
		[SerializeField] private InputActionReference inventoryScrollInputAction;

		[SerializeField] private float movementSpeed = 10f;
		[SerializeField] private float mouseSensibilityX = 50f;
		[SerializeField] private float mouseSensibilityY = 0.2f;
		private float minAngleX = -90f;
		private float maxAngleX = 90f;
		private float xRotation = 0f;
		private Vector2 movementInput = new Vector2(0, 0);
		private CharacterController controller;
		private Camera camera;

		[SerializeField] private Animator animator;
		[SerializeField] private Transform handPosition;
		[SerializeField] private Transform ShootPosition;

		public Inventory Inventory => inventory;

		private void Awake()
		{
			controller = GetComponent<CharacterController>();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			movementInputAction.action.Enable();
			cameraMovementInputAction.action.Enable();
			clickInputAction.action.Enable();
			secondClickInputAction.action.Enable();
			inventoryScrollInputAction.action.Enable();

			hands.SetPlayer(this);
			inventory = new Inventory(hands);
			//TODO: SACAR ESTO PORQUE LA PISTOLA SE RECOGE
			//inventory.Add(rock);

			clickInputAction.action.performed += OnPrimaryClick;
			clickInputAction.action.canceled += OnPrimaryClickReleased;
			secondClickInputAction.action.performed += OnSecondaryClick;
			secondClickInputAction.action.canceled += OnSecondaryClickReleased;
			inventoryScrollInputAction.action.performed += OnInventoryScroll;
			camera = Camera.main;
		}


		private void OnSecondaryClickReleased(InputAction.CallbackContext context)
		{
			inventory.GetSelectedItem().SecondaryActionReleased();
		}

		private void OnPrimaryClickReleased(InputAction.CallbackContext context)
		{
			inventory.GetSelectedItem().PrimaryActionReleased();
		}

		private void OnSecondaryClick(InputAction.CallbackContext obj)
		{
			inventory.GetSelectedItem().SecondaryAction();
		}

		private void OnPrimaryClick(InputAction.CallbackContext obj)
		{
			if (inventory.GetSelectedItem().ItemType == ItemType.Pistol)
			{
				animator.SetTrigger("Shoot");
			}

			inventory.GetSelectedItem().PrimaryAction();
		}

		private void OnInventoryScroll(InputAction.CallbackContext context)
		{
			Vector2 scroll = context.ReadValue<Vector2>();

			if (scroll.y > 0f)
			{
				inventory.SelectPrevious();
			}
			else if (scroll.y < 0f)
			{
				inventory.SelectNext();
			}

			animator.SetTrigger("ChangeItem");
		}

		private void OnInventorySlot(InputAction.CallbackContext context)
		{
			int slot = (int)context.ReadValue<float>();

			inventory.Select(slot);
		}

		private void Update()
		{
			movementInput = Vector2.zero;
			if (movementInputAction.action.ReadValue<Vector2>() != Vector2.zero)
			{
				movementInput = movementInputAction.action.ReadValue<Vector2>();
			}

			if (movementInput != Vector2.zero)
			{
				Vector3 movementDir = new Vector3(movementInput.x, 0, movementInput.y).normalized * (movementSpeed * Time.deltaTime);
				Vector3 dir = transform.right * movementDir.x + camera.transform.forward * movementDir.z;
				dir.y = 0;
				controller.Move(dir);
			}

			Vector2 inputCamera = cameraMovementInputAction.action.ReadValue<Vector2>();
			if (inputCamera != Vector2.zero)
			{
				Vector2 inputDir = inputCamera;
				inputDir *= Time.deltaTime;
				xRotation -= inputCamera.y * mouseSensibilityY;
				xRotation = Mathf.Clamp(xRotation, minAngleX, maxAngleX);
				transform.Rotate(Vector3.up * (inputDir.x * mouseSensibilityX));
				camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
			}
		}

		public void AddItemToInventory(Item item)
		{
			item.SetPlayer(this);
			inventory.Add(item);
			item.transform.SetParent(handPosition);
			item.transform.localPosition = Vector3.zero;
			item.transform.localRotation = Quaternion.identity;
			item.transform.localScale = Vector3.one;

			if (item is Pistol pistol)
			{
				pistol.shootPosition = ShootPosition;
			}
		}

		private void OnDestroy()
		{
			movementInputAction.action.Disable();
			cameraMovementInputAction.action.Disable();
			clickInputAction.action.Disable();
			secondClickInputAction.action.Disable();

			clickInputAction.action.performed -= OnPrimaryClick;
			secondClickInputAction.action.performed -= OnSecondaryClick;
			inventoryScrollInputAction.action.performed -= OnInventoryScroll;
		}

		public IItem GetSelectedItem()
		{
			return inventory.GetSelectedItem();
		}
	}
}