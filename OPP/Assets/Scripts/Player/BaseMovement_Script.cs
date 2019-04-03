using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement_Script : MonoBehaviour
{
    [Header("Movement Modifiers")]
    [Tooltip("The CharacterController component attached to this character.")] public CharacterController controller;
    [Tooltip("Multiplier for how fast the character moves normally.")] public float walkSpeed;
    [Tooltip("Multiplier for how fast the character moves when sprinting.")] public float sprintSpeed;
    [Tooltip("Multiplier for the force applied when the character jumps.")] public float jumpSpeed;
    [Tooltip("Amount of gravity being applied to character.")] public float gravity;
    [Tooltip("The current direction the character is moving.")] public Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        moveDirection = Vector3.zero;
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
