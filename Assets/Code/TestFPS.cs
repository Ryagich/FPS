using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFPS : MonoBehaviour
{
    private void Update()
    {
        Application.targetFrameRate = 60;
    }
}
