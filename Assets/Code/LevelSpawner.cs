using Unity.AI.Navigation;
using UnityEngine;
using YG;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private Levels _levels;

    public void CreateLevel()
    {
        var stage = YandexGame.savesData.Stage;

        switch (stage)
        {
            case 0:
                Instantiate(_levels.Stage_0);
                break;
            case 1:
                Instantiate(_levels.Stage_1[Random.Range(0, _levels.Stage_1.Count - 1)]);
                break;
            case 2:
                Instantiate(_levels.Stage_2[Random.Range(0, _levels.Stage_2.Count - 1)]);
                break;
            case 3:
                Instantiate(_levels.Stage_3[Random.Range(0, _levels.Stage_3.Count - 1)]);
                break;
            case 4:
                Instantiate(_levels.Stage_4[Random.Range(0, _levels.Stage_4.Count - 1)]);
                break;
        }
        GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>().Spawn();
    }
}