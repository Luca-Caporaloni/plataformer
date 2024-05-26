using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Controller_Player_StaticAir : Controller_Player
{

    private bool isStatic = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleFreeze();
        }

        if (!isStatic)
        {
            Movement();
        }
    }

    void ToggleFreeze()
    {
        isStatic = !isStatic;

        if (isStatic)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

}
