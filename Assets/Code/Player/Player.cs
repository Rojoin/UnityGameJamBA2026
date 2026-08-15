using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code.Player
{
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour
	{
		private Inventory inventory;

		[SerializeField] private Hands hands;

		//TODO: SACAR ESTO PORQUE LA PISTOLA SE RECOGE
		//[SerializeField] private Pistol pistol;

		[SerializeField] private InputActionReference movementInputAction;
		[SerializeField] private InputActionReference cameraMovementInputAction;
		[SerializeField] private InputActionReference clickInputAction;
		[SerializeField] private InputActionReference secondClickInputAction;
		[SerializeField] private InputActionReference inventoryScrollInputAction;

		[SerializeField] private float movementSpeed = 10f;
		[SerializeField] private float mouseSensibility = 10f;
		private float minAngleX = -90f;
		private float maxAngleX = 90f;
		private float xRotation = 0f;
		private Vector2 movementInput = new Vector2(0, 0);
		private CharacterController controller;
		private Camera camera;

		private void Awake()
		{
			controller = GetComponent<CharacterController>();
			movementInputAction.action.Enable();
			cameraMovementInputAction.action.Enable();
			clickInputAction.action.Enable();
			secondClickInputAction.action.Enable();
			inventoryScrollInputAction.action.Enable();

			inventory = new Inventory(hands);
			//TODO: SACAR ESTO PORQUE LA PISTOLA SE RECOGE
			//inventory.Add(pistol);

			clickInputAction.action.performed += OnPrimaryClick;
			secondClickInputAction.action.performed += OnSecondaryClick;
			inventoryScrollInputAction.action.performed += OnInventoryScroll;
			camera = Camera.main;
		}

		private void OnSecondaryClick(InputAction.CallbackContext obj)
		{
			inventory.GetSelectedItem().SecondaryAction();
		}

		private void OnPrimaryClick(InputAction.CallbackContext obj)
		{
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
				Vector3 movementDir = new Vector3(movementInput.x, 0, movementInput.y).normalized * (mouseSensibility * Time.deltaTime);
				Vector3 dir = transform.right * movementDir.x + camera.transform.forward * movementDir.z;
				dir.y = 0;
				controller.Move(dir);
			}

			Vector2 inputCamera = cameraMovementInputAction.action.ReadValue<Vector2>();
			if (inputCamera != Vector2.zero)
			{
				Vector2 inputDir = inputCamera;
				inputDir *= mouseSensibility * Time.deltaTime;
				xRotation -= inputCamera.y;
				xRotation = Mathf.Clamp(xRotation, minAngleX, maxAngleX);
				transform.Rotate(Vector3.up * inputDir.x);
				camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
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
	}
}