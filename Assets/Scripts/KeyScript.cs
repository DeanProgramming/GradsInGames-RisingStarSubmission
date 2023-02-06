using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    void Update()
    {
        transform.Rotate(transform.up, 2);
    }

    // If a player runs into the heart it will turn off the box collider and call spawn new character from the script Game
    // and then delete the heart
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Players")
        {
            PlaySound(clip);

            if(ScoreManager.GetMultiplier() == 10)
            {
                //11 is max so reward the player
                ScoreManager.IncreaseScore(10000);
                FindObjectOfType<GameManager>().ResetKey();
            }
            else
            {
                FindObjectOfType<GameManager>().NextFloor(collision.gameObject);
                FindObjectOfType<GameManager>().SetupWall();
            }

            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip soundEffect)
    {
        AudioSource soundSoure = GameObject.Find("SFXsounds").GetComponent<AudioSource>();
        soundSoure.PlayOneShot(soundEffect, soundSoure.volume);
    }
}
