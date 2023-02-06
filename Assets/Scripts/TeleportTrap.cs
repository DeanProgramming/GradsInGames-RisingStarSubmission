using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrap : MonoBehaviour
{
    public AudioClip clip;
    public float timeRequired;
    private float timer = 0;
    private bool swapMaterial;
    private bool opened;
    private List<GameObject> trappedPlayer = new List<GameObject>();
    private GameObject teleportPad;

    private void Start()
    {
        InvokeRepeating("FlashingFloor", 0, .2f);
        teleportPad = gameObject.transform.Find("TeleportObjects").gameObject;
    }

    private void FlashingFloor()
    {
        if (swapMaterial)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.509f, 0.509f, 0.509f, 1);
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        swapMaterial = !swapMaterial;
    }

    void Update()
    {
        TimerForTrapDoor();

        if (trappedPlayer.Count != 0)
        {
            foreach (GameObject player in trappedPlayer)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(teleportPad.transform.position.x, player.transform.position.y, teleportPad.transform.position.z), 2.5f);
            }        
        }
    }

    private void TimerForTrapDoor()
    {
        timer += Time.deltaTime;
        if (timer >= timeRequired && !opened) //Closed trap door and timer is 2 seconds 
        {
            CancelInvoke();
            ActivateTeleportTrap();
        }
        else if (timer >= timeRequired && opened)
        {
            CloseTeleportTrap();
        }
    }

    private void ActivateTeleportTrap()
    {
        //Animate
        //Spawn the box collider to kill  
        if (teleportPad != null)
        {
            teleportPad.SetActive(true);
        }

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        GetComponentInParent<EnvironmentTile>().IsAccessible = false;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshRenderer>().material.color = Color.black;

        opened = true;
        timer = 0;
    }

    public void CloseTeleportTrap()
    {
        if (teleportPad != null)
        {
            teleportPad.SetActive(false);
        }

        Destroy(GetComponent<Rigidbody>());
        GetComponentInParent<EnvironmentTile>().IsAccessible = true;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().material.color = new Color(0.509f, 0.509f, 0.509f, 1); 

        if (trappedPlayer.Count != 0)
        {
            foreach (GameObject player in trappedPlayer)
            {
                GameObject.Find("GameManagerObject").GetComponent<GameManager>().ChangeInPlayers();
                Destroy(player);
            }
        }

        Destroy(this);
    } 

    //When the box collider is enabled if a player is standing on top of the trap door the player will fall through and die
    //This will need to call a function that will check if everyone dead
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Players")
        {
            trappedPlayer.Add(collision.gameObject);
            collision.gameObject.GetComponent<Character>().StopAllCoroutines();
            collision.gameObject.tag = "Untagged";
            GameObject.Find("Game").GetComponent<Game>().UnselectCharacter(collision.gameObject);
            AudioSource soundSoure = GameObject.Find("SFXsounds").GetComponent<AudioSource>();
            soundSoure.PlayOneShot(clip, soundSoure.volume);
        }
    }
}
