using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Vector3 startGamePosition;
    float laneOffset;
    float pointStart;
    float pointFinish;
    float lastVectorX;
    float velocity;
    public float jumpPower = 15;
    public float jumpForce;
    public float jumpGravity = -40f;
    public float realGravity = -9.81f;
    public float forceValue;
    public float laneChangeSpeed = 15;
    public float distanceRay = 0.76f;
    public bool tryJump = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public LayerMask layerMask;
    Coroutine movingCoroutine;
    Rigidbody rb;

    void Start()
    {
        laneOffset = MapGenerator.instance.laneOffset;
        Physics.gravity = new Vector3(0, -20, 0);
        startGamePosition = transform.position;
        rb = GetComponent<Rigidbody>();
        SwipeManager.instance.MoveEvent += MovePlayer;
    }

    public void StartLevel()
    {
        RoadGenerator.instance.StartLevel();
    }

    public void ResetGame()
    {
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointFinish = 0;
        transform.position = startGamePosition;
        RoadGenerator.instance.ResetLevel();
    }

    void MovePlayer(bool[] swipes)
    {
        if (swipes[(int) SwipeManager.Direction.Left] && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Right] && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        } /*
        if (swipes[(int)SwipeManager.Direction.Up] && tryJump == false) // && rb.velocity.y == 0
        {
            tryJump = true;
        }*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            tryJump = true;
        }
    }

    private void FixedUpdate()
    {
        //velocity += Physics.gravity.y * 5 * Time.deltaTime;

        if (tryJump)
        {
            /*
            velocity = Mathf.Sqrt(30 * -2 * (Physics.gravity.y * 5));
            transform.Translate(new Vector3(0, velocity, 0) *  Time.deltaTime);
            tryJump = false;
            */

            jumpForce = Mathf.Sqrt(3 * Physics.gravity.y * -2) * rb.mass;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            tryJump = false;
        }
    }


    bool CheckingJump()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = -transform.up;
        if (Physics.Raycast(ray, distanceRay, layerMask))
        {
            isJumping = false;
        }
        else
        {
            isJumping = true;
        }
        return Physics.Raycast(ray, distanceRay, layerMask);
    }

    void MoveHorizontal(float speed)
    {
        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;

        if (isMoving)
        {
            StopCoroutine(movingCoroutine);
            isMoving = false;
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }

    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;
        while (Mathf.Abs(pointStart - transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            lastVectorX = vectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 2.5)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        }
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
        if (other.gameObject.tag == "Lose")
        {
            ResetGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (collision.gameObject.tag == "Not Lose")
        {
            MoveHorizontal(-lastVectorX);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RampPlane")
        {
            if (rb.velocity.x == 0 && isJumping == false)
            { // -10 это скорость падения с рампы
                rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
            }
        }
    }

    private void Move(Vector3 direction) 
    {
        rb.AddRelativeForce(direction * forceValue);
    }
} 
