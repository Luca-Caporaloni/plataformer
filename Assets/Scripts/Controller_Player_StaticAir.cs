using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Controller_Player_StaticAir : Controller_Player
{
    private bool isStatic = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isStatic = !isStatic;
            if (isStatic)
            {
                rb.velocity = Vector3.zero; // Detiene el movimiento actual del personaje
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; // Congela la posición en el eje Y
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; // Restablece las restricciones de movimiento
            }
        }

        // Movimiento horizontal solo si no está estático en el aire
        if (!isStatic)
        {
            Movement(); //Pasé el void a public para poder usarlo en este script
        }
    }
}
