using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foo : Singleton<Foo>
{
    public int value = 10;

    protected void Awake()
    {
        value = 20;
    }
}
