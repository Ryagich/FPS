using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsHolder : MonoBehaviour
{
    [TextArea, SerializeField] private string _text;
    
    public void Write()
    {
        SubsController.Instance.WriteText(_text);
    }
}
