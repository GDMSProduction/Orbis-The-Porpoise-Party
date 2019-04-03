using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pLobbyMovement_Script : BaseMovement_Script
{

    private float verticalVelocity; // The current vertical velocity of the player.

    // Called when this component is enabled
    void OnEnable()
    {
        // Initialize Variables
        walkSpeed = 3.0f;
        sprintSpeed = 6.0f;
        jumpSpeed = 3.0f;
        gravity = 14.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from controller for movement
        moveDirection.x = Input.GetAxis("Left_Horizontal");
        moveDirection.z = Input.GetAxis("Left_Vertical");
        // If moving, then make player face direction of movement
        if (moveDirection.x != 0 && moveDirection.z != 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
        }

        // Sprinting
        if (Input.GetButton("BButton"))
        {
            moveDirection *= sprintSpeed;
        }
        // Walking
        else
        {
            moveDirection *= walkSpeed;
        }

        // Jumping: Player on Ground
        if (controller.isGrounded)
        {
            // Set the velocity to counter gravity, keeping player on the ground.
            verticalVelocity = -gravity * Time.deltaTime;
            // Set vertical velocity to the jump speed/force.
            if (Input.GetButtonDown("AButton"))
            {
                verticalVelocity = jumpSpeed;
            }
        }
        // Jumping: Player in Air
        else
        {
            // Decrease vertical velocity to return the player to the ground naturally.
            verticalVelocity -= gravity * Time.deltaTime;
        }
        // Set move direction
        moveDirection.y = verticalVelocity;
        // Move Player
        controller.Move(moveDirection * Time.deltaTime);
    }
}
