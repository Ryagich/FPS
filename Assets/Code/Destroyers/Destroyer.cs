using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
    
    public void DestroyObject(GameObject go)
    {
        Destroy(go);
    }
    public void DestroyComponent(MonoBehaviour mono)
    {
        Destroy(mono);
    }
}
