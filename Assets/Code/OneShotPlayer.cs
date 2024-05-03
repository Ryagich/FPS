using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioSource _source;
    
    public void Play()
    {
        _source.PlayOneShot(_clip);
    }
    
    public void Play2(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
