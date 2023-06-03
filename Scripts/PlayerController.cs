using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //округление до нул€
    float epsilon = 0.0001f; //погрешность

    Vector3 startGamePosition;
    int score = 0;
    int tempScore;
    float laneOffset;
    float pointStart;
    float pointStartV;
    float pointFinish;
    float pointFinishV;
    float lastVectorX;
    float lastVectorY;
    public float jumpPower = 15;
    public float jumpForce;
    public float jumpGravity = -40f;
    public float realGravity = -9.81f;
    public float forceValue;
    public float laneChangeSpeed = 15;
    public float distanceRay = 0.76f;
    public bool tryJump = false;
    public bool isMoving = false;
    public bool isMovingVertical = false;
    public bool isJumping = false;
    public LayerMask layerMask;

    public Text scoreText;
    public Text maxScore;
    public Canvas losePanel;
    public UnityEngine.UI.Button pauseButton;

    public AudioClip jumpClip;
    public AudioClip musicLose;
    private AudioSource musicSource;

    Coroutine movingCoroutine;
    Coroutine movingCoroutineVertical;
    Rigidbody rb;

    void Start()
    {
        laneOffset = MapGenerator.Instance.laneOffset; 
        Physics.gravity = new Vector3(0, 0, 0); 
        startGamePosition = transform.position; 
        rb = GetComponent<Rigidbody>(); 
        //SwipeManager.instance.MoveEvent += MovePlayer; 
        musicSource = GetComponent<AudioSource>();
        ResetGame();
    }

    public void StartLevel()
    {
        RoadGenerator.Instance.StartLevel();
    }

    public void ResetGame()
    {
        tempScore = 0;
        CoinsCount.Instance.countCoin = 0;
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointStartV = 0;
        pointFinish = 0;
        pointFinishV = 0;
        transform.position = new Vector3(0, 0, -2);
        RoadGenerator.Instance.ResetLevel();
    }

    public void ResetScore ()
    {

        PlayerPrefs.DeleteKey("score");
    }

    private void FixedUpdate()
    {
        if (MovePlayer.Instance.enabled)
        {
            tempScore++;
            score = (int)(tempScore * 0.02f);
            scoreText.text = "Score: " + score.ToString();
        }

    }

    /**
    void MovePlayer(bool[] swipes)
    {
        //горизонтальное движение
        if (swipes[(int) SwipeManager.Direction.Left] && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Right] && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
            Debug.Log(pointFinishV);
        }

        //вертикальное движение
        if (swipes[(int) SwipeManager.Direction.Up] && pointFinishV > -laneOffset)
        {
            MoveVertical(laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Down] && pointFinishV < laneOffset)
        {
            MoveVertical(-laneChangeSpeed);
        } 

        //прыжок (отменен)
        /*if (swipes[(int)SwipeManager.Direction.Up] && isJumping == false) // && rb.velocity.y == 0
        {
            Jump();
        }
    /
    }

    void MoveVertical(float speed)
    {
        pointStartV = pointFinishV;
        pointFinishV += Mathf.Sign(speed) * laneOffset; //расчет на случай, если плеер был между двух направлений и игрок изменил решение

        if (isMovingVertical) //отмена движени€ и применение нового движени€
        {
            StopCoroutine(movingCoroutineVertical);
            isMovingVertical = false;
        }
        movingCoroutineVertical = StartCoroutine(MoveVerticalCoroutine(speed));
    }

    IEnumerator MoveVerticalCoroutine(float vectorY)
    {
        isMovingVertical = true;
        while (Mathf.Abs(pointStartV - transform.position.y) < laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(rb.velocity.x, vectorY, 0);
            lastVectorY = vectorY;
            float y = Mathf.Clamp(transform.position.y, Mathf.Min(pointStartV, pointFinishV), Mathf.Max(pointStartV, pointFinishV));
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, pointFinishV, transform.position.z);
        isMovingVertical = false;
    }

    /*
    void Jump()
    {
        isJumping = true;
        AudioSource.PlayClipAtPoint(jumpClip, transform.position);
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine()); 
    }

    IEnumerator StopJumpCoroutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);
        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0); //когда он перестал падать (скорость ” = 0), его гравитаци€ измен€етс€
    }
    /

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
        /*
        if (transform.position.y > 2.5)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        }
        /
        isMoving = false;
    }
    */


    // соприкосновени€ и реакции на это
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
        if (other.gameObject.tag == "Lose")
        {
            losePanel.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(musicLose, transform.position);
            if (losePanel.isActiveAndEnabled)
            {
                musicSource.Stop();
                if (score > PlayerPrefs.GetInt("score"))
                {
                    PlayerPrefs.SetInt("score", score);
                }
                maxScore.text = "Score: " + score + " \nMac score:  " + PlayerPrefs.GetInt("score").ToString();
                score = 0;
            }
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
        /*
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        */
        if (collision.gameObject.tag == "Not Lose")
        {
           // MoveHorizontal(-lastVectorX);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        /*
        if (collision.gameObject.tag == "RampPlane")
        {
            if (rb.velocity.x == 0 && isJumping == false)
            { // -10 это скорость падени€ с рампы
                rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
            }
        }
        */
    }
} 
