//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Manages the spawning and playing of sounds.
    /// </summary>
    public class AudioManagerService : MonoBehaviour, IAudioManagerService
    {
        private readonly struct OneShotCoroutine
        {
            public AudioClip Clip { get; }
            public AudioSettings Settings { get; }
            public float Delay { get; }
            
            public OneShotCoroutine(AudioClip clip, AudioSettings settings, float delay)
            {
                Clip = clip;
                Settings = settings;
                Delay = delay;
            }
        }

        private bool IsPlayingSource(AudioSource source)
        {
            if (source == null)
                return false;

            return source.isPlaying;
        }

        private IEnumerator DestroySourceWhenFinished(AudioSource source)
        {
            yield return new WaitWhile(() => IsPlayingSource(source));
            if(source != null)
                DestroyImmediate(source.gameObject);
        }

        private IEnumerator PlayOneShotAfterDelay(OneShotCoroutine value)
        {
            yield return new WaitForSeconds(value.Delay);
            PlayOneShot_Internal(value.Clip, value.Settings);
        }
        
        private void PlayOneShot_Internal(AudioClip clip, AudioSettings settings)
        {
            if (clip == null)
                return;
            var newSourceObject = new GameObject($"Audio Source -> {clip.name}");
            var newAudioSource = newSourceObject.AddComponent<AudioSource>();
            newAudioSource.outputAudioMixerGroup = Character.Instance.EffectsGroup;
            newAudioSource.ignoreListenerPause = false;
            newAudioSource.volume = settings.Volume;
            newAudioSource.spatialBlend = settings.SpatialBlend;
            
            newAudioSource.PlayOneShot(clip);
            
           // if(settings.AutomaticCleanup)
           //     StartCoroutine(nameof(DestroySourceWhenFinished), newAudioSource);
        }

        #region Audio Manager Service Interface

        public void PlayOneShot(AudioClip clip, AudioSettings settings = default)
        {
            //Play.
            PlayOneShot_Internal(clip, settings);
        }

        public void PlayOneShotDelayed(AudioClip clip, AudioSettings settings = default, float delay = 1.0f)
        {
            //Play.
            StartCoroutine(nameof(PlayOneShotAfterDelay), new OneShotCoroutine(clip, settings, delay));
        }

        #endregion
    }
}