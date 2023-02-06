using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesBaseClass : MonoBehaviour
{
    private float timer = 0; 
    void Update()
    {
        transform.Rotate(transform.up, 2);
        timer += Time.deltaTime;
        if (timer >= 4)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip soundEffect)
    {       
        AudioSource soundSoure = GameObject.Find("SFXsounds").GetComponent<AudioSource>();
        soundSoure.PlayOneShot(soundEffect, soundSoure.volume); 
    }
}
