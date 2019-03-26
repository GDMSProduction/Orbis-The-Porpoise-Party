using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by: Logan Acree
// Last Edited by: Logan Acree
public class pMovement_Script : MonoBehaviour
{
    [Header("Movement Modifiers")]
    [SerializeField, Tooltip("Multiplier for how fast the character moves normally.")] float walkSpeed;
    [SerializeField, Tooltip("Multiplier for how fast the character moves when sprinting.")] float sprintSpeed;
    [SerializeField, Tooltip("Multiplier for the force applied when the character jumps.")] float jumpSpeed = 3;
    [SerializeField, Tooltip("Amount of gravity being applied to character.")] float gravity;
    private Vector3 moveDirection; // The current direction the player is moving.
    private float verticalVel; // The current vertical velocity of the player.
    private CharacterController controller; // The CharacterController Component attached to the player

    // Start is called before the first frame update
    void Start()
    {
        // Initalize moveDirection, and set controller.
        moveDirection = Vector3.zero;
        controller = gameObject.GetComponent<CharacterController>();
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
            verticalVel = -gravity * Time.deltaTime;
            // Set vertical velocity to the jump speed/force.
            if (Input.GetButtonDown("AButton"))
            {                
                verticalVel = jumpSpeed;
            }
        }
        // Jumping: Player in Air
        else
        {
            // Decrease vertical velocity to return the player to the ground naturally.
            verticalVel -= gravity * Time.deltaTime;
        }
        // Set move direction
        moveDirection.y = verticalVel;
        // Move Player
        controller.Move(moveDirection * Time.deltaTime);
    }
}
