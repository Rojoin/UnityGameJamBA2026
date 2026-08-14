using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{ 
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour
	{
		[SerializeField] private InputAction movementInputAction;
		[SerializeField] private InputAction cameraMovementInputAction;
		[SerializeField] private InputAction clickInputAction;
		[SerializeField] private InputAction secondClickInputAction;
		[SerializeField] private float movementSpeed = 10f;
		private Vector2 movementInput = new Vector2(0,0);
		private CharacterController controller;


		private void Awake()
		{
			controller = GetComponent<CharacterController>();
		}

		private void Update()
		{
			movementInput = Vector2.zero;
			if (movementInputAction.ReadValue<Vector2>() != Vector2.zero)
			{
				movementInput = movementInputAction.ReadValue<Vector2>();
			}

			if (movementInput != Vector2.zero)
			{
				controller.Move( new Vector3(movementInput.x, 0, movementInput.y) * (movementSpeed * Time.deltaTime) );
			}
		}
	}
}