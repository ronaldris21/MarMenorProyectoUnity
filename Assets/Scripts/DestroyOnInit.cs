using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyOnInit : MonoBehaviour
{
    public int seconds = 0;
    void Start()
    {
        Destroythis();
    }

    private async void Destroythis()
    {
        Thread.Sleep(seconds * 1000);
        Destroy(gameObject);

        throw new NotImplementedException();
    }
}
