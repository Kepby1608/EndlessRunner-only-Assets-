/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorController : Singleton<AcceleratorController>
{

    private Rigidbody rb;

    // accelerator
    public bool isSensor = true;
    private bool isSensorAvailable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        isSensorAvailable = SystemInfo.supportsAccelerometer;
        if (SystemInfo.supportsAccelerometer)
        {
            Input.gyro.enabled = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isSensor)
        {
            Vector3 acceleration = Input.acceleration;
            rb.velocity = new Vector3(acceleration.x * 6, acceleration.y * 6, 0);
        }
    }
}
*/
