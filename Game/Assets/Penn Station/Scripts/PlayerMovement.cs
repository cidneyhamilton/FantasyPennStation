using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
		private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {

        PlayerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        PlayerRotation = new Vector3(0, Input. GetAxisRaw("Horizontal"), 0);

        MovePlayer();
        MoveCamera();

    }
    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        if (Controller.isGrounded)
        {
            Velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = JumpForce;
            }
        }
        else
        {
            Velocity.y -= Gravity * -2f * Time.deltaTime;
        }

				Controller.Move(MoveVector * Speed * Time.deltaTime);

        Controller.Move(Velocity * Time.deltaTime);

    }
		
    private void MoveCamera()
    {
        transform.Rotate(PlayerRotation * Sensitivity * Time.deltaTime);
    }
}
