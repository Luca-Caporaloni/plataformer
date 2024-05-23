using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private bool movingToB = true;

    void Update()
    {
        if (movingToB)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
            if (transform.position == pointB.position)
            {
                movingToB = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
            if (transform.position == pointA.position)
            {
                movingToB = true;
            }
        }
    }
}