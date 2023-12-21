using UnityEngine;

public class SubsControllerCaller : MonoBehaviour
{
    public void WriteText(string str)
    {
        SubsController.Instance.WriteText(str);
    }
    
    public void Hide()
    {
        SubsController.Instance.HideNow();
    }
}
