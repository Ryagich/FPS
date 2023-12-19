﻿using UnityEngine;
using System.Collections;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	public class ExplosionScript : MonoBehaviour {
    
    	[Header("Customizable Options")]
    	public float despawnTime = 10.0f;
    	public float lightDuration = 0.02f;
    	[Header("Light")]
    	public Light lightFlash;
    
    	[Header("Audio")]
    	public AudioClip[] explosionSounds;
    
    	private void Start () 
	    {
    		StartCoroutine (DestroyTimer ());
    		StartCoroutine (LightFlash ());
    
		    AudioManager.Instance.PlaySound(
			    explosionSounds[Random.Range(0, explosionSounds.Length)],AudioSourceType.Player,transform);
    	}
    
    	private IEnumerator LightFlash () 
	    {
    		lightFlash.GetComponent<Light>().enabled = true;
    		yield return new WaitForSeconds (lightDuration);
    		lightFlash.GetComponent<Light>().enabled = false;
    	}
    
    	private IEnumerator DestroyTimer () 
	    {
    		yield return new WaitForSeconds (despawnTime);
    		Destroy (gameObject);
    	}
    }
}