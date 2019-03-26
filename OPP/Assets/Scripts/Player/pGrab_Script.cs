using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by: Logan Acree
// Last Edited by: Logan Acree
public class pGrab_Script : MonoBehaviour
{
    private bool hasItem = false;   // used to check if the player has an item or not.
    private bool hasThrown = false; // used to check if the player has thrown an item or not.
    private float dropTimer;        // counter for how long the button to drop an object has been pressed.
    private Vector3 target;         // Target location for the Item when thrown. Set to the reticle's position at the time the button to throw the object was pressed.
    [Header("Grabbing and Throwing")]
    [Tooltip("The current Item GameObject being held by the player.")] public GameObject pItem = null;
    [Tooltip("The GameObject being used as the player's reticle.")] public GameObject pReticle;
    [SerializeField, Tooltip("Amount of time a button needs to be held down to drop an object.")] float timeToDrop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                if (pItem != null)
                {
                    pItem.transform.position = transform.position;
                }
                // Hold Button down to drop item
                if (Input.GetButton("XButton"))
                {
                    dropTimer += Time.deltaTime;
                    // Drop Item, reset variables, and set reticle to inactive
                    if (dropTimer >= timeToDrop)
                    {
                        pItem.GetComponent<MeshRenderer>().material.color = Color.white;
                        pItem = null;
                        hasItem = false;
                        dropTimer = 0.0f;
                        pReticle.SetActive(false);
                    }
                }
                // Throw the item
                if (Input.GetButton("YButton"))
                {
                    // Throw item towards reticle
                    hasThrown = true;
                    target = pReticle.transform.position;
                    pReticle.transform.localPosition = new Vector3(0.2f, -1.065f, 0.3f);
                    pReticle.SetActive(false);
                }
            }
            // If the player has thrown the item
            else
            {
                // LERP item to target position
                pItem.transform.position = Vector3.Lerp(pItem.transform.position, target, Time.deltaTime * 10.0f);
                // Round current position
                Vector3 current = new Vector3();
                current.x = Mathf.Round(pItem.transform.position.x);
                current.y = Mathf.Round(pItem.transform.position.y);
                current.z = Mathf.Round(pItem.transform.position.z);
                // Round target position
                Vector3 test = new Vector3();
                test.x = Mathf.Round(target.x);
                test.y = Mathf.Round(target.y);
                test.z = Mathf.Round(target.z);
                // Check if current position equals the target position
                if (current == test)
                {
                    // Return item back to original color, and reset variables
                    pItem.GetComponent<MeshRenderer>().material.color = Color.white;
                    pItem = null;
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
                pReticle.SetActive(true);
                // Convert into world space
                pItem = other.gameObject;
                pItem.GetComponent<MeshRenderer>().material.color = Color.red;
                hasItem = true;
            }   
        }
    }
}
