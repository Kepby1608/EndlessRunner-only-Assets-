/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class MovePlayer : Singleton<MovePlayer>
{
    // base
    private Rigidbody rb;

    // sensor
    private Vector3 touchPosition;
    private Vector3 direction;
    public float moveSpeed = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && AcceleratorController.Instance.isSensor)
        {
            touchPosition = Input.GetTouch(0).deltaPosition;
            direction = new Vector3(touchPosition.x, touchPosition.y, 0);
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
} */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class MovePlayer : Singleton<RoadGenerator>
{
    // base
    private Rigidbody rb;

    // sensor
    private Vector3 touchPosition;
    private Vector3 direction;
    public float moveSpeed = 0.1f;
    public bool isSensor;

    // accelerometr
    public float speed;
    Vector3 momentV;
    Vector3 dirV;
    Vector3 defV;
    bool flag = false;
    bool zet = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        flag = false;
        zet = true;
        speed = 1000;
    }

    void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && isSensor) // sensor
        {
            touchPosition = Input.GetTouch(0).deltaPosition;
            direction = new Vector3(touchPosition.x, touchPosition.y, 0);
            rb.velocity = direction * moveSpeed;
        }
        else if (!isSensor) // accelerometr
        {
            if (!flag)
            {
                momentV.x = Input.acceleration.x;
                momentV.y = -Input.acceleration.z;
                momentV.z = 0;
                dirV = Vector3.zero;
                defV = momentV - dirV;

                if (dirV != defV)
                {
                    flag = true;

                    if (momentV.y > -0.7 && momentV.y <= 0.7)
                    {
                        zet = true;
                    }
                    else
                    {
                        momentV.y = -Input.acceleration.y;
                        zet = false;
                    }
                    defV = momentV - dirV;
                }
            }

            if (flag && zet)
            {
                momentV.x = Input.acceleration.x;
                momentV.y = -Input.acceleration.z;
            }
            else if (flag && !zet)
            {
                momentV.x = Input.acceleration.x;
                momentV.y = -Input.acceleration.y;
            }

            dirV = (momentV - defV);

            dirV.x = Mathf.Clamp(dirV.x, -0.25f, 0.25f);
            dirV.y = Mathf.Clamp(dirV.y, -0.25f, 0.25f);

            if (momentV.sqrMagnitude > 1)
                momentV.Normalize();

            dirV *= speed;

            dirV *= Time.deltaTime;
            rb.velocity = dirV;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

    }
}



