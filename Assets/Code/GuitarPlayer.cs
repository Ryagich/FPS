using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GuitarPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _clips = new();
    [SerializeField] private AudioSource _source;

    private AudioClip current;

    private void Update()
    {
        if (!_source.isPlaying)
        {
            _source.PlayOneShot(GetNewClip());
        }
    }
    
    private AudioClip GetNewClip()
    {
        var clips = new List<AudioClip>(_clips);
        if (current)
        {
            clips.Remove(current);
        }

        var newClip = clips[Random.Range(0, clips.Count - 1)];
        current = newClip;
        return current;
    }
}
