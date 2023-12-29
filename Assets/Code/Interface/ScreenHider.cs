using System.Collections;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

public class ScreenHider : MonoBehaviour
{
    [SerializeField] private UnityEvent _onHide;
    [SerializeField, Min(.0f)] private float _speed;

    private Character character;
    private Image image;

    public void Init(GameObject go)
    {
        character = go.GetComponent<Character>();
    }

    public void Hide()
    {
        if (!image)
            image = UIHolder.Instance.Hide;
        image.gameObject.SetActive(true);
        StartCoroutine(Hiding());
        character.CanPause = false;
    }

    private IEnumerator Hiding()
    {
        while (image.color.a < 1)
        {
            image.color.WithA(Mathf.MoveTowards(image.color.a, 1, _speed));
            yield return new WaitForFixedUpdate();
        }

        _onHide?.Invoke();
        YandexGame.savesData.SceneIndex = 0;
        YandexGame.SaveProgress();
    }
}