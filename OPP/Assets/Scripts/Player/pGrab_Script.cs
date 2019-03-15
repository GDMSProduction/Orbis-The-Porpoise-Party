using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pGrab_Script : MonoBehaviour
{

    public GameObject item = null;
    private bool hasItem = false;
    private float dropTimer;
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
            // Move the item with the player
            if (item != null)
            {
                item.transform.position = transform.position;
            }
            // Hold Button down to drop item
            if (Input.GetButton("XButton"))
            {
                dropTimer += Time.deltaTime;
                if (dropTimer >= timeToDrop)
                {
                    item.GetComponent<MeshRenderer>().material.color = Color.white;
                    item = null;
                    hasItem = false;
                    dropTimer = 0.0f;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hasItem == true)
            return;
        if (other.gameObject.tag == "PickUp")
        {
            if (Input.GetButtonDown("XButton"))
            {
                Debug.Log("Pressed XBUTTON");
                // Convert into world space
                item = other.gameObject;
                item.GetComponent<MeshRenderer>().material.color = Color.red;
                hasItem = true;
            }   
        }
    }
}
