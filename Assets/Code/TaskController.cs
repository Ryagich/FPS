using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    [SerializeField, Min(.0f)] private float _speed;
    private Transform TaskShowPlace;
    private Transform TaskHidePLace;
    private Transform Task;
    private TMP_Text text;

    private Transform curr;

    public void Init(GameObject go)
    {
        var holder = go.GetComponent<UIHolder>();
        Task = holder.Task;
        TaskShowPlace = holder.TaskShowPlace;
        TaskHidePLace = holder.TaskHidePLace;
        HideTask();
    }

    public void ShowTask(string t)
    {
        text.text = t;
        curr = TaskShowPlace;
    }
    
    public void HideTask()
    {
        curr = TaskHidePLace;
    }

    private void FixedUpdate()
    {
        if (!Task)
            return;
        Task.position = Vector3.MoveTowards(Task.position,
            curr.position, _speed * Time.fixedDeltaTime);
    }
}