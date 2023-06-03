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

public class MovePlayer : Singleton<RoadGenerator>
{
    // base
    private Rigidbody rb;

    // sensor
    private Vector3 touchPosition;
    private Vector3 direction;
    public float moveSpeed = 0.1f;

    // accelerator
    public Quaternion calibrationQuaternion;
    public bool isSensor = true;
    public float sensetive = 25f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CalibrateAccelerometer();
    }

    void FixedUpdate()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && isSensor)
        {
            touchPosition = Input.GetTouch(0).deltaPosition;
            direction = new Vector3(touchPosition.x, touchPosition.y, 0);
            rb.velocity = direction * moveSpeed;
        }
        else if (!isSensor)
        {
            // каждый кадр проверяем положение телефона в пространстве
            Vector3 acceleration = Input.acceleration;
            // получаем координаты текущего положения телефона относительно стартового положения
            //Vector3 acceleration = FixAcceleration(accelerationRaw);
            rb.velocity = new Vector3(acceleration.x, -acceleration.z, 0) * sensetive;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void CalibrateAccelerometer()
    {
        // получаем данные с датчика акселерометра и записываем в accelerationSnapshot
        Vector3 accelerationSnapshot = Input.acceleration;

        // поворачиваем из положения лицом вверх в положение, полученное от акселерометра
        // функция возвращает координаты положения телефона в пространстве типа кватернион (текущее положение телефона)
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), accelerationSnapshot);

        // инвертируем значение осей
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
        Debug.Log("Calibrated");
    }

    public Vector3 FixAcceleration(Vector3 acceleration)
    {
        // умножаем стартовое положение телефона на текущее
        // получаем текущее положение с учетом калибровки
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }
}



