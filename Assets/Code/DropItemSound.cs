using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DropItemSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play()
    {
        source.PlayOneShot(_clip);
    }
}