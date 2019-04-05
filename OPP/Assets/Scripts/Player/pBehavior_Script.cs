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
    [Tooltip("The GameObject of the Bone for the held item's location while being held. It is set automatically.")] public GameObject bone;
    [SerializeField, Tooltip("Amount of time a button needs to be held down to drop an object.")] float timeToDrop;
    [HideInInspector] public bool hasItem; // used to check if the player has an item or not.
    [HideInInspector] public bool hasThrown; // used to check if the player has thrown an item or not.
    private float dropTimer; // counter for how long the button to drop an object has been pressed.
    private ItemState state; // The state of the item being held

    [Header("Reticle Variables")]
    [Tooltip("The GameObject being used as the player's reticle. It is set on automatically.")] public GameObject reticle;
    [SerializeField, Tooltip("Value used to scale the input values")] float aimScale = 4.0f;
    [SerializeField, Tooltip("Deadzone of the Right Analog Stick")] float deadZone = 0.19f;
    float aimAxisH = 0; // Horizontal value for Right Analog Stick Input
    float aimAxisV = 0; // Vertical value for Right Analog Stick Input.
    bool reticleMoved = false; // Check if the object has been moved.

    // Start is called before the first frame update
    void Start()
    {
        reticle = transform.Find("Reticle").gameObject;
        bone = transform.Find("Bone").gameObject;
        timeToDrop = timeToDrop == 0 ? 2.0f : timeToDrop;
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
                // Aim the reticle
                AimReticle();
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
                // If the button is pressed and reticle is not at its origin, throw the item and set state to THROWN
                else if (Input.GetAxis("Right_Trigger") == -1 && reticleMoved == true)
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
    public void SetHeldItemVariables(ItemState _state)
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

    /// <summary>
    /// Determines the position of the reticle based on input from Right Analog Stick.
    /// </summary>
    private void AimReticle()
    {
        // Get current input of Right Analog Stick
        aimAxisH = Input.GetAxis("RS_Horizontal");
        aimAxisV = Input.GetAxis("RS_Vertical");

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

        // If there is input, move reticle to targeted position
        if (aimAxisH != 0 || aimAxisV != 0)
        {
            reticleMoved = true;
            float y = ReticleRaycast();
            reticle.transform.localPosition = new Vector3(aimAxisH * aimScale, y, -1 * aimAxisV * aimScale);
        }
        // Else reset it's position the original value. (center of player, at feet)
        else
        {
            reticleMoved = false;
            reticle.transform.localPosition = new Vector3(0.2f, -1.065f, 0.3f);
        }
    }

    /// <summary>
    /// Perfomr raycast from player to reticle to check for obstacles intersecting with reticle and return the normal of the hit as the new y value.
    /// </summary>
    /// <returns></returns>
    private float ReticleRaycast()
    {
        RaycastHit hit;
        Vector3 direction = reticle.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            Debug.DrawRay(transform.position, direction, Color.cyan);
            if (hit.transform.name != "Reticle")
            {
                Debug.Log(hit.transform.name);
                return hit.normal.y;
            }
        }
        return -1.065f;
    }
}
