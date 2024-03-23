using UnityEngine;

public class Talent : MonoBehaviour
{
    public void Interactable(GameObject character, GameObject _)
    {
        TalentsController.Instance.ShowTalents(character,_);
    }
}
