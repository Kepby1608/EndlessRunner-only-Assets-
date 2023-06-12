using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    //округление до нуля
    //float epsilon = 0.0001f; //погрешность

    //Vector3 startGamePosition;
    int score = 0;
    int coinTemp = 0;
    int maxCoinTemp = 0;
    int tempScore;
    //float laneOffset;
    public bool flagDie = false;
    public bool unTouchible = false;
    //public float jumpPower = 15;
    //public float jumpForce;
    //public float jumpGravity = -40f;
    //public float realGravity = -9.81f;
    //public float forceValue;
    //public float laneChangeSpeed = 15;
    //public float distanceRay = 0.76f;
    //public bool tryJump = false;
    //public bool isMoving = false;
    //public bool isMovingVertical = false;
    //public bool isJumping = false;
    public LayerMask layerMask;

    public TextMeshProUGUI scoreTextInGame;
    public TextMeshProUGUI scoreTextInMenu;
    public TextMeshProUGUI maxScore;
    public TextMeshProUGUI moneyText;
    public Canvas losePanel;
    public Canvas preLosePanel;
    public Button buttonShowAd;
    //public UnityEngine.UI.Button pauseButton;

    //public AudioClip jumpClip;
    public AudioClip musicLose;
    public AudioSource musicSource;

    Rigidbody rb;


    private void Awake()
    {
        scoreTextInMenu.text = "Max Score: " + PlayerPrefs.GetInt("score").ToString();
    }

    void Start()
    {
        Physics.gravity = new Vector3(0, 0, 0); 
        rb = GetComponent<Rigidbody>(); 
        musicSource = GetComponent<AudioSource>();
        ResetGame();
        moneyText.text = "Money: " + PlayerPrefs.GetInt("money");
        unTouchible = false;
    }

    public void StartLevel()
    {
        musicSource.Play();
        RoadGenerator.Instance.StartLevel();
        RewardedAds.Instance.LoadAd();
        flagDie = false;
        unTouchible = false;
    }

    //public void StartCoroutinGamee()
    //{
    //    StartGame();
    //}

    //private IEnumerator StartGame()
    //{
    //    yield return new WaitForSeconds(1f);
    //    StartLevel();
    //}

    public void ResetGame()
    {
        CoinCalculate();
        moneyText.text = "Money: " + PlayerPrefs.GetInt("money");
        scoreTextInMenu.text = "Max Score: " + PlayerPrefs.GetInt("score");
        tempScore = 0;
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0.5f, -2);
        RoadGenerator.Instance.ResetLevel();
    }

    public void CoinCalculate()
    {
        coinTemp = CoinsCount.Instance.countCoin;
        maxCoinTemp = PlayerPrefs.GetInt("money");
        maxCoinTemp += coinTemp;
        PlayerPrefs.SetInt("money", maxCoinTemp);
        CoinsCount.Instance.countCoin = 0;
    }

    public void ScoreCalculate()
    {
        if (score > PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", score);
        }
        maxScore.text = "Score: " + score + " \nMax score:  " + PlayerPrefs.GetInt("score").ToString();
        score = 0;
    }

    public void ResetScore()
    {

        PlayerPrefs.DeleteKey("score");
    }

    private void FixedUpdate()
    {
        if (unTouchible) {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -8.15f, 8.15f),
                Mathf.Clamp(transform.position.y, 0.5f, 6.5f),
                transform.position.z);
        }

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
                scoreTextInGame.text = "Score: " + score.ToString();
            }
        }

    }

    // соприкосновения и реакции на это
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lose")
        {
            Trig();
            if (!flagDie)
            {
                preLosePanel.gameObject.SetActive(true);
            }
            else
            {
                musicSource.Stop();
                losePanel.gameObject.SetActive(true);
                ScoreCalculate();
            }
        }
    }

    private void Trig()
    {
        AudioSource.PlayClipAtPoint(musicLose, transform.position);
        Pause.Instance.PauseGame();
    }

    public void Die()
    {
        CameraTracking.Instance.Die();
        ResetGame();
        buttonShowAd.interactable = true;
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