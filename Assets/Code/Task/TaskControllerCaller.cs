using UnityEngine;

public class TaskControllerCaller : MonoBehaviour
{
    [SerializeField] [TextArea] private string _task = "";
    [SerializeField] [TextArea] private string _task2 = "";

    public void Show2()
    {
        TaskController.Instance.ShowTask(_task2);
    }

    public void Show()
    {
        TaskController.Instance.ShowTask(_task);
    }
    
    public void ShowTask(string t)
    {
        TaskController.Instance.ShowTask(t);
    }
    
    public void HideTask()
    {
        TaskController.Instance.HideTask();
    }
}
