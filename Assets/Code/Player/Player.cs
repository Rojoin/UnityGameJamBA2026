using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{ 
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour
	{
		[SerializeField] private InputActionReference movementInputAction;
		[SerializeField] private InputActionReference cameraMovementInputAction;
		[SerializeField] private InputActionReference clickInputAction;
		[SerializeField] private InputActionReference secondClickInputAction;
		[SerializeField] private float movementSpeed = 10f;
		private Vector2 movementInput = new Vector2(0,0);
		private CharacterController controller;

		private Camera camera;
		private void Awake()
		{
			controller = GetComponent<CharacterController>();
			movementInputAction.action.Enable();
			cameraMovementInputAction.action.Enable();
			clickInputAction.action.Enable();
			secondClickInputAction.action.Enable();

			clickInputAction.action.performed += OnPrimaryClick;
			secondClickInputAction.action.performed += OnSecondaryClick;
			camera = Camera.main;
		}

		private void OnSecondaryClick(InputAction.CallbackContext obj)
		{
			
		}

		private void OnPrimaryClick(InputAction.CallbackContext obj)
		{
			
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
				controller.Move( new Vector3(movementInput.x, 0, movementInput.y) * (movementSpeed * Time.deltaTime) );
			}

			if (cameraMovementInputAction.action.ReadValue<Vector2>() != Vector2.zero)
			{

				Vector2 inputDir;
				camera.transform.rotation = Quaternion.Euler(cameraMovementInputAction.action.ReadValue<Vector2>().x, 0, 0);
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
		}
	}
}