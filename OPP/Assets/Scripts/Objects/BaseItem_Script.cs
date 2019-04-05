using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem_Script : MonoBehaviour
{
    [Tooltip("The current player holding the item.")] public GameObject player = null;
    [Tooltip("Target position for object when thrown.")] public Vector3 target;
    [Tooltip("The damage afflicted to other entities by this object.")] public float damage;
    [HideInInspector] public bool isHeld;
    [HideInInspector] public bool isThrown;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // If item is held, move it with the player
            if (isHeld)
            {
                transform.position = player.GetComponent<pBehavior_Script>().bone.transform.position;
            }
            // If the item has been thrown.
            if (isThrown)
            {
                MoveToTarget();
            }
        }
    }

    /// <summary>
    /// Pick up item and set variables
    /// </summary>
    public void PickUpItem(GameObject _player)
    { 
        // Set Player variables
        player = _player;
        isHeld = true;
        isThrown = false;
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    /// <summary> 
    /// Drop Item and reset variables
    /// </summary>
    public void DropItem()
    {
        target = Vector3.zero;
        isHeld = false;
        player = null;
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    /// <summary>
    /// Throw Item and update variables
    /// </summary>
    /// <param name="_target">The target position for the object</param>
    public void ThrowItem(Vector3 _target)
    {
        target = _target;
        isHeld = false;
        isThrown = true;
    }

    /// <summary>
    /// LERP the object from its current position to the target position at a constant speed.
    /// </summary>
    private void MoveToTarget()
    {
        // LERP item to target position
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 10.0f);
        // Round current position
        Vector3 current = new Vector3();
        current.x = Mathf.Round(transform.position.x);
        current.y = Mathf.Round(transform.position.y);
        current.z = Mathf.Round(transform.position.z);
        // Round target position
        Vector3 test = new Vector3();
        test.x = Mathf.Round(target.x);
        test.y = Mathf.Round(target.y);
        test.z = Mathf.Round(target.z);
        // Check if current position equals the target position
        if (current == test)
        {
            // Return item back to original color, and reset variables
            player = null;
            isThrown = false;
            target = Vector3.zero;
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}
