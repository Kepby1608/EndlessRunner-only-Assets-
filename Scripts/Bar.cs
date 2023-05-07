using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    void Start ()
    {
        Foo.Instance.value = 5;
    }
}
