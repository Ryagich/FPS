using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
