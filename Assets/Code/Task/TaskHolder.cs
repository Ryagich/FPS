using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskHolder : MonoBehaviour
{
    [TextAreaAttribute, SerializeField] private string _text;

    public void Write()
    {
        TaskController.Instance.ShowTask(_text);
    }

    public void Hide()
    {
        TaskController.Instance.HideTask();
    }
}