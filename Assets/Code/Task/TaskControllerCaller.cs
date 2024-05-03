using UnityEngine;

public class TaskControllerCaller : MonoBehaviour
{
    public void ShowTask(string t)
    {
        TaskController.Instance.ShowTask(t);
    }
    
    public void HideTask()
    {
        TaskController.Instance.HideTask();
    }
}
