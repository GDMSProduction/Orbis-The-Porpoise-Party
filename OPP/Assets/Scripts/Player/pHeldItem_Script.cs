using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by: Logan Acree
// Last Edited by: Logan Acree
public class pHeldItem_Script : MonoBehaviour
{
    private bool hasItem = false;   // used to check if the player has an item or not.
    private bool hasThrown = false; // used to check if the player has thrown an item or not.
    private float dropTimer;        // counter for how long the button to drop an object has been pressed.
    private Vector3 target;         // Target location for the Item when thrown. Set to the reticle's position at the time the button to throw the object was pressed.
    [Header("Grabbing and Throwing")]
    [Tooltip("The current Item GameObject being held by the player.")] public GameObject item = null;
    [Tooltip("The GameObject being used as the player's reticle.")] public GameObject reticle;
    [SerializeField, Tooltip("Amount of time a button needs to be held down to drop an object.")] float timeToDrop;

    // Update is called once per frame
    void Update()
    {
        // If player has an item
        if (hasItem)
        {
            // If the player hasn't thrown the item
            if (!hasThrown)
            {
                // Move the item with the player
                if (item != null)
                {
                    item.transform.position = transform.position;
                }
                // Hold Button down to drop item
                if (Input.GetButton("XButton"))
                {
                    dropTimer += Time.deltaTime;
                    // Drop Item, reset variables, and set reticle to inactive
                    if (dropTimer >= timeToDrop)
                    {
                        item.GetComponent<MeshRenderer>().material.color = Color.white;
                        item = null;
                        hasItem = false;
                        dropTimer = 0.0f;
                        reticle.SetActive(false);
                    }
                }
                // Throw the item
                if (Input.GetButton("YButton"))
                {
                    // Throw item towards reticle
                    // Set Target
                    
                    hasThrown = true;
                    target = reticle.transform.position;
                    reticle.transform.localPosition = new Vector3(0.2f, -1.065f, 0.3f);
                    reticle.SetActive(false);
                }
            }
            // If the player has thrown the item
            else
            {
                // LERP item to target position
                item.transform.position = Vector3.Lerp(item.transform.position, target, Time.deltaTime * 10.0f);
                // Round current position
                Vector3 current = new Vector3();
                current.x = Mathf.Round(item.transform.position.x);
                current.y = Mathf.Round(item.transform.position.y);
                current.z = Mathf.Round(item.transform.position.z);
                // Round target position
                Vector3 test = new Vector3();
                test.x = Mathf.Round(target.x);
                test.y = Mathf.Round(target.y);
                test.z = Mathf.Round(target.z);
                // Check if current position equals the target position
                if (current == test)
                {
                    // Return item back to original color, and reset variables
                    item.GetComponent<MeshRenderer>().material.color = Color.white;
                    // Do Item Action
                    item = null;
                    hasItem = false;
                    hasThrown = false;
                    target = Vector3.zero;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // If an item is already being held, return;
        if (hasItem == true)
        {
            return;
        }
        // If the object can be picked up
        if (other.gameObject.tag == "PickUp")
        {
            if (Input.GetButtonDown("XButton"))
            {
                Debug.Log("Pressed XBUTTON");
                // Turn reticle on
                reticle.SetActive(true);
                // Convert into world space
                item = other.gameObject;
                item.GetComponent<MeshRenderer>().material.color = Color.red;
                hasItem = true;
            }   
        }
    }
}
