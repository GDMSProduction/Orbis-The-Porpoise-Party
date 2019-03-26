using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by: Logan Acree
// Last Edited by: Logan Acree
public class pReticle_Script : MonoBehaviour
{
    [SerializeField, Tooltip("Value used to scale the input values")] float aimScale;
    [SerializeField, Tooltip("Deadzone of the Right Analog Stick")] float deadZone = 0.19f;
    float aimAxisH, aimAxisV = 0; // Horizontal and Vertical values for Right Analog Stick Input.

    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    // Determines the position of the reticle based on input from Right Analog Stick.
    private void Aim()
    {
        // Get current input of Right Analog Stick
        aimAxisH = Input.GetAxis("Right_Horizontal");
        aimAxisV = Input.GetAxis("Right_Vertical");

        // Calculate deadzone for accurate results
        Vector2 stickInput = new Vector2(aimAxisH, aimAxisV);
        if (stickInput.magnitude < deadZone)
        {
            stickInput = Vector2.zero;
        }
        else
        {
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadZone) / (1 - deadZone));
        }

        // Set the new axis variables
        aimAxisH = stickInput.x;
        aimAxisV = stickInput.y;
        Debug.Log("X: " + aimAxisH + " Y: " + aimAxisV);

        // If there is input, move reticle to targeted position
        if (aimAxisH != 0 || aimAxisV != 0)
        {
            gameObject.transform.localPosition = new Vector3(aimAxisH * aimScale, -1.065f, -1 * aimAxisV * aimScale);
        }
        // Else reset it's position the origin.
        else
        {
            gameObject.transform.localEulerAngles = new Vector3(0.2f, -1.065f, 0.3f);
        }
    }
}