using UnityEngine;
using YG;

public class DeathmatchTypeSetter : MonoBehaviour
{
    // -1 - не дезматч; 0 - до 100 киллов; 1 - 5 мин; 2 - 10 мин;
    
    public void Set100Kills()
    {
        YandexGame.savesData.DeatmatchType = 0;
        YandexGame.SaveProgress();
    }
    
    public void Set5Min()
    {
        YandexGame.savesData.DeatmatchType = 1;
        YandexGame.SaveProgress();
    }
    
    public void Set10Min()
    {
        YandexGame.savesData.DeatmatchType = 2;
        YandexGame.SaveProgress();
    }
}
