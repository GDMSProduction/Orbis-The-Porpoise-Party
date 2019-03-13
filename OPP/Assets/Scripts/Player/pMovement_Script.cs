using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pMovement_Script : MonoBehaviour
{
    [SerializeField] Vector3 moveDirection;
    [SerializeField] Vector3 lookDirection;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;

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
        moveDirection.y = transform.position.y;
        moveDirection.z = Input.GetAxis("Left_Vertical");

        transform.LookAt(moveDirection);
        // Jumping
        if (Input.GetButton("AButton"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.up * walkSpeed;
        }
        // Sprinting
        if (Input.GetButton("BButton"))
        {
            moveDirection *= sprintSpeed;
        }
        else
        {
            moveDirection *= walkSpeed;
        }
        controller.SimpleMove(moveDirection);
    }
}
