using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelScript : CollectablesBaseClass
{
    [SerializeField] private AudioClip clip;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Players")
        {
            PlaySound(clip);
            ScoreManager.IncreaseScore(1000);
            Destroy(gameObject);
        }
    }
}
