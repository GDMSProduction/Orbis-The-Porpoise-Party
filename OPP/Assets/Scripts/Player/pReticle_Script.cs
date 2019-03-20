using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pReticle_Script : MonoBehaviour
{
    public float aimScale;
    float aimAxisH, aimAxisV = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        aimAxisH = Input.GetAxis("Right_Horizontal");
        aimAxisV = Input.GetAxis("Right_Vertical");
        Debug.Log("X: " + aimAxisH + " Y: " + aimAxisV);
        if (aimAxisH > 0 || aimAxisV > 0)
        {
            if (Mathf.Abs(aimAxisH) + Mathf.Abs(aimAxisV) > 1)
            {
                gameObject.transform.localPosition = new Vector3(aimAxisH * aimScale, -1.065f, - 1 * aimAxisV * aimScale);
            }
        }
    }
}