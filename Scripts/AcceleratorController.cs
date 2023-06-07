using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{

    private Rigidbody rb;
    public float speed;
    Vector3 momentV;
    Vector3 dirV;
    Vector3 defV;
    bool flag;
    bool zet;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 10000;
    }

    void Update()
    {

        if (!flag)
        {
            momentV.x = Input.acceleration.x;
            momentV.y = Input.acceleration.z;
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
                    momentV.y = Input.acceleration.y;
                    zet = false;
                }
                defV = momentV - dirV;
            }
        }

        if (flag && zet)
        {
            momentV.x = Input.acceleration.x;
            momentV.y = Input.acceleration.z;
            Debug.Log("Z = " + momentV);
        }
        else if (flag && !zet)
        {
            momentV.x = Input.acceleration.x;
            momentV.y = Input.acceleration.y;
            Debug.Log("Y = " + momentV);
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
}
