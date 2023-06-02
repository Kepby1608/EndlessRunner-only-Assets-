using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Vector3 touchPosition;
    private Rigidbody rb;
    private Vector3 direction;
    public float moveSpeed = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
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
}


