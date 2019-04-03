using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState
{
    PICKUP,
    DROPPED,
    THROWN
};


public class pBehavior_Script : MonoBehaviour
{
    [Header("Held Item Variables")]
    [Tooltip("The current Item GameObject being held by the player.")] public GameObject heldItem = null;
    [Tooltip("The GameObject being used as the player's reticle.")] public GameObject reticle;
    [SerializeField, Tooltip("Amount of time a button needs to be held down to drop an object.")] float timeToDrop;
    private ItemState state; // The state of the item being held
    public bool hasItem; // used to check if the player has an item or not.
    public bool hasThrown; // used to check if the player has thrown an item or not.
    private float dropTimer; // counter for how long the button to drop an object has been pressed.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If an item is held
        if (hasItem)
        {
            // If an object has NOT been thrown
            if (!hasThrown)
            {
                // Hold button down to drop item
                if (Input.GetButton("XButton"))
                {
                    dropTimer += Time.deltaTime;
                    // Drop item and set state to DROPPED
                    if (dropTimer >= timeToDrop)
                    {
                        heldItem.GetComponent<BaseItem_Script>().DropItem();
                        SetHeldItemVariables(ItemState.DROPPED);
                    }
                }
                // Throw the item and set state to THROWN
                else if (Input.GetButton("YButton"))
                {
                    heldItem.GetComponent<BaseItem_Script>().ThrowItem(reticle.transform.position);
                    SetHeldItemVariables(ItemState.THROWN);
                }
            }
        }
    }

    // Called on continious collision
    void OnTriggerStay(Collider other)
    {
        // If item is already held return.
        if (hasItem)
        {
            return;
        }

        // If the collision object has an item script
        if (other.GetComponent<BaseItem_Script>())
        {
            // Press Button to pick up object
            if (Input.GetButtonDown("XButton"))
            {
                // Set reticle active, set heldItem, set state, call PickUpItem()
                heldItem = other.gameObject;
                SetHeldItemVariables(ItemState.PICKUP);
                heldItem.GetComponent<BaseItem_Script>().PickUpItem(gameObject);
            }
        }
    }

    /// <summary>
    /// Set the state of the held item and the variables related to it
    /// </summary>
    /// <param name="_state">The state of the held item: 1 = PICKUP, 2 = DROPPED, 3 = THROWN</param>
    private void SetHeldItemVariables(ItemState _state)
    {
        switch (_state)
        {
            // Pick up object
            case ItemState.PICKUP:
                state = _state;

                hasItem = true;
                hasThrown = false;
                dropTimer = 0.0f;
                reticle.SetActive(true);
                break;
            // Drop object
            case ItemState.DROPPED:
                state = _state;
                heldItem = null;
                hasItem = false;
                hasThrown = false;
                dropTimer = 0.0f;
                reticle.SetActive(false);
                break;
            // Throw Object
            case ItemState.THROWN:
                state = _state;
                heldItem = null;
                hasItem = false;
                hasThrown = true;
                dropTimer = 0.0f;
                reticle.transform.localPosition = new Vector3(0.2f, -1.065f, 0.3f);
                reticle.SetActive(false);
                break;
            default:
                break;
        }
    }
}
