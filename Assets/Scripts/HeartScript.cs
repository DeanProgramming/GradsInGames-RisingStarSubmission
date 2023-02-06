using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : CollectablesBaseClass
{
    [SerializeField] private Material[] possibleSkin;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private AudioClip clip;
    
    // If a player runs into the heart it will turn off the box collider and call spawn new character from the script Game
    // and then delete the heart
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Players")
        {            
            if (Physics.Raycast(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z), -transform.up, out RaycastHit rayHit, Mathf.Infinity, ~ignoreLayer))
            {
                PlaySound(clip);
                GetComponent<BoxCollider>().enabled = false;
                GameObject.Find("Game").GetComponent<Game>().SpawnNewCharacter(rayHit.collider.gameObject, possibleSkin[Random.Range(0, possibleSkin.Length)]);
                Destroy(gameObject);
            }
        }
    }
}
