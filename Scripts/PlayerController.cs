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
    //округление до нуля
    //float epsilon = 0.0001f; //погрешность

    //Vector3 startGamePosition;
    int score = 0;
    int tempScore;
    //float laneOffset;
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

    Rigidbody rb;

    void Start()
    {
        //laneOffset = MapGenerator.Instance.laneOffset; 
        Physics.gravity = new Vector3(0, 0, 0); 
        //startGamePosition = transform.position; 
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
        transform.position = new Vector3(0, 0.5f, -2);
        RoadGenerator.Instance.ResetLevel();
    }

    public void ResetScore ()
    {

        PlayerPrefs.DeleteKey("score");
    }

    private void FixedUpdate()
    {
        //if (MovePlayer.instance.enabled)
        //{
        //    tempScore++;
        //    if (score < (int)(tempScore * 0.02f))
        //        RoadGenerator.Instance.speed += score * 0.1f;
        //    score = (int)(tempScore * 0.02f);
        //    scoreText.text = "Score: " + score.ToString();
        //}

        if (MovePlayer.instance.enabled)
        {
            tempScore++;

            // Рассчитываем текущий score и speed
            float currentScore = tempScore * 0.1f;
            int roundedScore = Mathf.RoundToInt(currentScore);
            float speed = 10.1f + (roundedScore - 1) * 0.1f;

            // Проверяем, изменился ли score и speed
            if (roundedScore != score)
            {
                score = roundedScore;
                RoadGenerator.Instance.speed = speed;
                scoreText.text = "Score: " + score.ToString();
            }
        }

    }

    // соприкосновения и реакции на это
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

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Ramp")
    //    {
    //        rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    /*
    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    //    }
    //    */
    //    if (collision.gameObject.tag == "Not Lose")
    //    {
    //       // MoveHorizontal(-lastVectorX);
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    /*
    //    if (collision.gameObject.tag == "RampPlane")
    //    {
    //        if (rb.velocity.x == 0 && isJumping == false)
    //        { // -10 это скорость падения с рампы
    //            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
    //        }
    //    }
    //    */
    //}
} 
