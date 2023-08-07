using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement for the player character
/// </summary>
public class PlayerMovement : MonoBehaviour
{
		/// True if the player can move
		public bool MovementEnabled = true;

		// True if the player can jump
		public bool JumpEnabled = false;

		// The player's velocity
		private Vector3 Velocity;

		// Raw player movement input
    private Vector3 PlayerMovementInput;

		// Raw player rotation input
    private Vector3 PlayerRotation;

    [Header("Components Needed")]
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform Player;

    [Space]
	 		
    [Header("Movement")]
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Sensitivity = 0.01f;
    [SerializeField] private float Gravity = -9.81f;

		const float JUMP_FORCE = 2f;
		
    void Start()
    {
				// Disable the cursor by default
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {

        PlayerMovementInput = new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
        PlayerRotation = new Vector3(0, Input. GetAxisRaw("Horizontal"), 0);

        MovePlayer();
        MoveCamera();
 
    }

		/// <summary>
		/// Handles moving the player
		/// </summary>
    private void MovePlayer()
    {
				if (!MovementEnabled) {
						return;
				}
				
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

				if (Controller.isGrounded)
				{
						Velocity.y = -1f;

						// Starts a jump
						if (Input.GetKeyDown(KeyCode.Space) && JumpEnabled)
						{
								Velocity.y = JumpForce;
						}
				}
				else
				{
						// Continues a jump
						Velocity.y += Gravity * JUMP_FORCE * Time.deltaTime;
				}

				Controller.Move(MoveVector * Speed * Time.deltaTime);
        Controller.Move(Velocity * Time.deltaTime);

    }
		
    private void MoveCamera()
    {
        transform.Rotate(PlayerRotation * Sensitivity * Time.deltaTime);
    }
}
