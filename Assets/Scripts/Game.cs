using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private Character Character;

    [SerializeField] private GameObject backToMainMenu;
    [SerializeField] private GameObject backToGame;

    private RaycastHit[] mRaycastHits;
    private Character mCharacter;
    private Environment mMap;

    private readonly int NumberOfRaycastHits = 1;  

    void Start()
    {
        mRaycastHits = new RaycastHit[NumberOfRaycastHits];
        mMap = GetComponentInChildren<Environment>();
        mCharacter = Instantiate(Character, transform);
        ReturnHat(mCharacter.gameObject).GetComponent<MeshRenderer>().material.color = Color.red;
        GameObject.Find("GameManagerObject").GetComponent<GameManager>().enabled = false;
    }

    private void Update()
    {
        // When the player left clicks on a player it will select that character and then the player can then 
        // right click to tell the character to move
        if (Input.GetMouseButtonDown(0))
        {             
            if(Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHit))
            { 
                if(rayHit.collider.tag == "Players")
                {
                    if (mCharacter != null)
                    {
                        ReturnHat(mCharacter.gameObject).GetComponent<MeshRenderer>().material.color = Color.black; //Set old character hat to black
                    }

                    Debug.Log(rayHit.collider.gameObject.name);
                    mCharacter = rayHit.collider.gameObject.GetComponent<Character>();
                    ReturnHat(mCharacter.gameObject).GetComponent<MeshRenderer>().material.color = Color.red;
                } 
            }
        }

        
        // Check to see if the player has clicked a tile and if they have, try to find a path to that 
        // tile. If we find a path then the character will move along it to the clicked tile. 
        // Changing the code from Input.GetMouseButtonDown(0) to Input.GetMouseButtonDown(1) so the player
        // can right click to move the character 
        if (Input.GetMouseButtonDown(1) && mCharacter != null)
        {
            Ray screenClick = MainCamera.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);
            if( hits > 0)
            {
                EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();
                if (tile != null)
                {
                    List<EnvironmentTile> route = mMap.Solve(mCharacter.CurrentPosition, tile);
                    mCharacter.GoTo(route);
                }
            }
        }
    }


    private void MoveCharacterPosition(Vector3 position, Quaternion rotation, bool gameManager)
    {
        mCharacter.transform.position = position;
        mCharacter.transform.rotation = rotation;
        GameObject.Find("GameManagerObject").GetComponent<GameManager>().enabled = gameManager;
    }

    /*
     *  Had to change the canvas system as I want more screens for example (death screen, option screen)
     *  I also dont want to have a character on the main menu screen 
     */

    public void EndGame()
    { 
        GameObject.Find("GameManagerObject").GetComponent<GameManager>().enabled = false;
        mMap.CleanUpWorld();
        mCharacter = Instantiate(Character, transform);
        ReturnHat(mCharacter.gameObject).GetComponent<MeshRenderer>().material.color = Color.red; 
    }    

    public void EnableCanvas(Canvas desiredCanvas)
    {
        desiredCanvas.gameObject.SetActive(true);
    }
    public void DisableCanvas(Canvas active)
    {
        active.gameObject.SetActive(false);
    }

     
    public void Generate()
    {
        FindObjectOfType<Environment>().SetMapSize(10, 10);
        mMap.GenerateWorld();
        GameObject.Find("GameManagerObject").GetComponent<GameManager>().enabled = true;
        GameObject.Find("GameManagerObject").GetComponent<GameManager>().ResetScore();
        mCharacter.CurrentPosition = mMap.Start;
        MoveCharacterPosition(mMap.Start.Position, Quaternion.identity, true);
    }

    public void Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

    public void SpawnNewCharacter(GameObject hitObject, Material mat)
    {        
        Vector3 position = new Vector3(hitObject.transform.position.x + (mMap.GetTileSizeX() / 2), 4.5f, hitObject.transform.position.z + (mMap.GetTileSizeX() / 2));
        Character spawnCharacter = Instantiate(Character, position, Quaternion.identity);
        spawnCharacter.gameObject.transform.SetParent(transform);
        spawnCharacter.CurrentPosition = hitObject.GetComponent<EnvironmentTile>();
        ReturnHat(spawnCharacter.gameObject).GetComponent<MeshRenderer>().material.color = Color.black;
         
        //Loop through each body part and change the material
        SkinnedMeshRenderer[] renderers = spawnCharacter.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer item in renderers)
        {
            item.material = mat;
        }
    }

    // Make sure that the trapped player is the current player if so deselect the player
    // else dont unselect the player
    public void UnselectCharacter(GameObject trappedPlayer)
    {
        trappedPlayer.GetComponent<Animator>().SetBool("Running", false);
        trappedPlayer.GetComponent<Animator>().SetBool("Dead", true);
        if (trappedPlayer.GetComponent<Character>() == mCharacter)
        {
            ReturnHat(mCharacter.gameObject).GetComponent<MeshRenderer>().material.color = Color.black; //Set old character hat to black
            mCharacter = null;
        }
    }

    public void SelectCharacter(Character player)
    {
        mCharacter = player;
        ReturnHat(mCharacter.gameObject).GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            backToGame.SetActive(true);
            backToMainMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            backToGame.SetActive(false);
            backToMainMenu.SetActive(true);
        }
    }

    private GameObject ReturnHat(GameObject firstParent)
    {
        return firstParent.transform.Find("Rig1").transform.Find("Spine1").transform.Find("Spine2").transform.Find("Neck1").transform.Find("Hat").gameObject;
    }
}
