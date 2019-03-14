using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pMovement_Script : MonoBehaviour
{
    [SerializeField, Tooltip("Multiplier for how fast the character moves normally.")] float walkSpeed;
    [SerializeField, Tooltip("Multiplier for how fast the character moves when sprinting.")] float sprintSpeed;
    [SerializeField, Tooltip("Multiplier for the force applied when the character jumps.")] float jumpSpeed = 3;
    [SerializeField, Tooltip("Amount of gravity being applied to character.")] float gravity;
    private float verticalVel;
    Vector3 moveDirection;
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector3.zero;
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement and Looking
        moveDirection.x = Input.GetAxis("Left_Horizontal");
        moveDirection.z = Input.GetAxis("Left_Vertical");
        transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));

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
        
        // Jumping
        if (controller.isGrounded)
        {
            verticalVel = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("AButton"))
            {
                verticalVel = jumpSpeed;
            }
        }
        else
        {
            verticalVel -= gravity * Time.deltaTime;
        }
        moveDirection.y = verticalVel;
        // Move Player
        controller.Move(moveDirection * Time.deltaTime);


    }
}
