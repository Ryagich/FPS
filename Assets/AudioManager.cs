using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Serialization;
using AudioSettings = InfimaGames.LowPolyShooterPack.AudioSettings;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _playerEffectsPref;
    [SerializeField] private AudioSource _itemEffectsPref;
    [SerializeField] private AudioSource _stepEffectsPref;
    [FormerlySerializedAs("_EnemyShootPref")] [SerializeField] private AudioSource _ShootPref;
    [SerializeField] private List<AudioSource> _sounds;

    private bool isPause;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClip clip, AudioSourceType type, Transform place = null,
            float delay = 0)
        //, AudioSettings settings = default)
    {
        if (!place)
            place = Character.Instance.transform;
        var source = Instantiate(GetSource(type),
            place.position, place.rotation);
        source.transform.SetParent(transform);
        //source.volume = settings.Volume;
        //source.spatialBlend = settings.SpatialBlend;
        source.clip = clip;
        _sounds.Add(source);
        //source.ignoreListenerPause = false;
        StartCoroutine(PlayingSound(source, clip, delay));
    }

    private AudioSource GetSource(AudioSourceType type)
    {
        switch (type)
        {
            case AudioSourceType.Item:
                return _itemEffectsPref;
            case AudioSourceType.Steps:
                return _stepEffectsPref;
            case AudioSourceType.Weapon:
                return _ShootPref;
            default:
                return _playerEffectsPref;
        }
    }

    private IEnumerator PlayingSound(AudioSource source, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(clip);
        while (true)
        {
            if (!source.isPlaying && !isPause)
            {
                RemoveSource(source);
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void RemoveSource(AudioSource source)
    {
        Debug.Log("Remove");

        _sounds.Remove(source);
        Destroy(source.gameObject);
    }

    public void Pause()
    {
        AudioListener.pause = true;
        isPause = true;
    }

    public void UnPause()
    {
        AudioListener.pause = false;
        isPause = false;
    }
}

public enum AudioSourceType
{
    Player = 0,
    Item = 1,
    Steps,
    Weapon
}