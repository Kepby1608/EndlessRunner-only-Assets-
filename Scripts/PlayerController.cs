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
    public float jumpPower = 15;
    public float jumpGravity = -40f;
    public float realGravity = -9.81f;
    public float forceValue;
    public float laneChangeSpeed = 15;
    public float distanceRay = 0.76f;
    public bool letsJump = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public LayerMask layerMask;
    Coroutine movingCoroutine;
    Rigidbody rb;
    private Vector3 direction;
    //Ray ray = new Ray();

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
        }
        if (swipes[(int)SwipeManager.Direction.Up] && isJumping == false && rb.velocity.y == 0)
        {
            isJumping = true;
        }
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

    private void Update()
    {
        //direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (isJumping)
        {
            letsJump = true;
        }
    }

    private void FixedUpdate()
    {

        if (Physics.CheckSphere(rb.transform.position, 0.1f, layerMask))
        {

            if (letsJump)
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            //else
               // rb.velocity = new Vector3(direction.x, 0, direction.z);
        }

        //else
           // rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);
    }

    void Jump()
    {
        isJumping = true;
        //rb.velocity = Vector3.ClampMagnitude(rb.AddForce(0, 500, 0), );
        //rb.AddForce(0, 500, 0);
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        //Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());
    }

    IEnumerator StopJumpCoroutine()
    {
        /*
        Debug.Log("Start Check");
        yield return new WaitUntil(CheckingJump);
        Debug.Log("Stop Check");
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);
        Debug.Log("!!!!!!!!!!!!Jump = " + isJumping);
        */
        do
        {
            yield return new WaitForSeconds(0.01f);
        } while (rb.velocity.y != 0);
        isJumping = false;
        //Physics.gravity = new Vector3(0, realGravity, 0);
    }

    /*
    bool CheckingJump()
    {
        Debug.Log("Jump = " + isJumping);
        return Physics.Raycast(ray, distanceRay, layerMask);
    }

        /*Ray ray = new Ray(transform.position, -Vector3.up);
        do
        {
            //RaycastHit hit;
            //if (Physics.Raycast(ray, distanceRay, layerMask))
            //{

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            Debug.Log("Jump = " + isJumping);
            yield return new WaitForSeconds(0.01f);
            //}
        } while (!Physics.Raycast(ray, distanceRay, layerMask));

        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0); 
    
     
    private void FixedUpdate()
    {
        ray.origin = transform.position;
        ray.direction = -transform.up;
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        Debug.Log(Physics.gravity);
        /*
        Ray ray = new Ray(transform.position, -Vector3.up);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

        //RaycastHit hit;
        if (Physics.Raycast(ray, distanceRay, layerMask))
        {
            isJumping = false;
            Debug.Log("Jump = " + isJumping);
        }
        */

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
