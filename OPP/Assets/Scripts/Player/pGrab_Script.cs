using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pGrab_Script : MonoBehaviour
{

    public GameObject reticle;
    public GameObject item = null;
    public Vector3 target;
    private bool hasItem = false;
    private bool hasThrown = false;
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
                    hasThrown = true;
                    target = reticle.transform.position;
                    reticle.SetActive(false);
                }
            }
            // If the player has thrown the item
            else
            {
                item.transform.position = Vector3.Lerp(item.transform.position, target, Time.deltaTime * 10.0f);
                Vector3 current = new Vector3();
                current.x = Mathf.Round(item.transform.position.x);
                current.y = Mathf.Round(item.transform.position.y);
                current.z = Mathf.Round(item.transform.position.z);
                Vector3 test = new Vector3();
                test.x = Mathf.Round(target.x);
                test.y = Mathf.Round(target.y);
                test.z = Mathf.Round(target.z);
                if (current == test)
                {
                    item.GetComponent<MeshRenderer>().material.color = Color.white;
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
        if (hasItem == true)
            return;
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
