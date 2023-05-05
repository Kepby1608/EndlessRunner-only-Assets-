using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour
{
    public static SwipeManager instance;

    public enum Direction { Left, Right, Up, Down };

    bool[] swipe = new bool[4];

    Vector2 startTouch;
    bool touchMoved;
    Vector2 swipeDelta;

    const float SWIPE_THRESHOLD = 50;

    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;

    public delegate void ClickDelegate(Vector2 pos);
    public ClickDelegate ClickEvent;

    Vector2 TouchPosition()
    {
        return (Vector2) Input.mousePosition;
    }

    bool TouchBegan()
    {
        return Input.GetMouseButtonDown(0);
    }

    bool TouchEnded()
    {
        return Input.GetMouseButtonUp(0);
    }

    bool GetTouch()
    {
        return Input.GetMouseButton(0);
    }

    private void Awake()
    {
        instance = this; 
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (TouchBegan())
        {
            startTouch = TouchPosition();
            touchMoved = true;
        } 
        else if (TouchEnded() && touchMoved == true)
        {
            SendSwipe();
            touchMoved = false;
        }

        // calc distance
        swipeDelta = Vector2.zero;
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - startTouch;
        }

        // check swipe
        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                // left/right
                swipe[(int) Direction.Left] = swipeDelta.x < 0;
                swipe[(int) Direction.Right] = swipeDelta.x > 0;
            } 
            else
            {
                // up/down
                swipe[(int)Direction.Down] = swipeDelta.y < 0;
                swipe[(int)Direction.Up] = swipeDelta.y > 0;
            }
            SendSwipe();
        }
    }

    void SendSwipe()
    {
        if (swipe[0] || swipe[1] || swipe[2] || swipe[3])
        {
            Debug.Log(swipe[0] + "|" + swipe[1] + "|" + swipe[2] + "|" + swipe[3]);
            MoveEvent?.Invoke(swipe);
        }
        else
        {
            Debug.Log("Click");
            ClickEvent?.Invoke(TouchPosition());
        }
        Reset();
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        touchMoved = false;
        for (int i = 0; i < swipe.Length; i++)
        {
            swipe[i] = false;
        }
    }
}

/**
 private Touch touch;
    public float speedModifier;
    Rigidbody rb;

    void Start()
    {
        speedModifier = 0.001f;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                    transform.position.x + (touch.deltaPosition.x * speedModifier),
                    transform.position.y, 
                    transform.position.z + (touch.deltaPosition.y * speedModifier));
            }

            /*Touch touch1 = Input.GetTouch(0);
            if (touch1.phase == TouchPhase.Stationary)
            {
                float distance = touch1.deltaPosition.magnitude; //touch.deltaPosition возвращает Vector2. magnitude позволяет узнать его длинну
                float speed = distance / Time.deltaTime;
                Debug.Log("Скорость движения пальца :" + speed);
            }*
        }
        /*
        foreach (Touch touch in Input.touches)
        {
            Vector3 newPosition = transform.position;
            newPosition.x += Mathf.Clamp(touch.deltaPosition.y * speedModifier, 0, 15) * Time.deltaTime;
            transform.position = newPosition;   
        }*

    }
 */