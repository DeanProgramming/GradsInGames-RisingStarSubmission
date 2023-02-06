using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas HUD;
    [SerializeField] private Canvas deathScreenCanvas;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject jewel;
    [SerializeField] private GameObject key;
    [SerializeField] private TextMeshProUGUI textUi;
    [SerializeField] private TextMeshProUGUI multiplierUI;
    [SerializeField] private TextMeshProUGUI scoreScreenUi;
    [SerializeField] private TextMeshProUGUI highscoreScreenUi;
    [SerializeField] private TextMeshProUGUI timeToGetKeyTime;
    [SerializeField] private AudioClip clip;

    private float requiredTime = 3;
    private float tempTimer;

    private float tempScoreTime;
    private float difficultyIncrease = 1; 

    private float timeToSpawnKey = 0;
    private float timeBeforeRoomIsDestroyed = 10;
    private bool spawnAKey;

    private bool offsetEnabled = false;

    void Update()
    {
        tempTimer += Time.deltaTime;

        if(difficultyIncrease < .5f) // This is 5.05 minutes til it hits the max
        {
            difficultyIncrease = .5f;
        }

        if(tempTimer >= requiredTime * difficultyIncrease)
        {
            switch(Random.Range(0, 6))
            {
                case 0:
                    {
                        TopLineAttackPattern.TopLineAttack(clip);
                        requiredTime = 2.5f;
                        break;
                    }

                case 1:
                    {
                        VerticalLineAttackPattern.VerticalLineAttack(clip);
                        requiredTime = 2.5f;
                        break;
                    }

                case 2:
                    {
                        RandomTeleportTrapAttackPattern.RandomTeleportTrap(3 + ScoreManager.GetMultiplier() , difficultyIncrease, clip);
                        requiredTime = 1f;
                        break;
                    }

                case 3:
                    {
                        DiagonalAttackPattern.StartDiagonalAttack(clip);
                        requiredTime = 2.5f;
                        break;
                    }

                case 4:
                    {
                        SpawnConsumable(heart, 2); //Hearts
                        requiredTime = 1;
                        break;
                    }

                case 5:
                    {
                        SpawnConsumable(jewel, 3); //jewel
                        SpawnConsumable(jewel, 3); //jewel 
                        requiredTime = 1;
                        break;
                    } 
            } 

            tempTimer = 0;
        }

        timeToSpawnKey += Time.deltaTime;
        if(timeToSpawnKey >= 10)
        {
            StartCountdown();
            if(spawnAKey == false)
            {
                SpawnConsumable(key, 2);
                spawnAKey = true;
            }
        }

        // Every 3 seconds 
        // Score = Score + ( Amount of players * 250) * multiplier 
        // Reduce difficulty increase this will make the attack occur more often

        tempScoreTime += Time.deltaTime;
        if (tempScoreTime >= 3)
        {
            difficultyIncrease -= 0.005f;
            ScoreManager.IncreaseScore(GameObject.FindGameObjectsWithTag("Players").Length * 250);
            tempScoreTime = 0;
        }

        ScoreManager.UpdateScoreBoard(textUi);
    }

    private void StartCountdown()
    {
        timeBeforeRoomIsDestroyed -= Time.deltaTime;
        timeToGetKeyTime.text = "Get the key ! - " + Mathf.RoundToInt(timeBeforeRoomIsDestroyed).ToString();
        if (timeBeforeRoomIsDestroyed <= 0)
        {
            GameObject[] player = GameObject.FindGameObjectsWithTag("Players");
            foreach (GameObject item in player)
            {
                item.gameObject.GetComponent<Character>().StopAllCoroutines();
                item.gameObject.tag = "Untagged";
            }
            ChangeInPlayers();
        }
    }
    public void ResetKey()
    {
        timeToSpawnKey = 0;
        timeBeforeRoomIsDestroyed = 10;
        spawnAKey = false;
        timeToGetKeyTime.text = "";
    }

    //If the player grabs the key call this
    public void NextFloor(GameObject player)
    {
        ResetKey();

        timeBeforeRoomIsDestroyed = 10 +  ScoreManager.GetMultiplier();

        ScoreManager.IncreaseMultipler(1, multiplierUI);       

        // Get position 
        Vector3 pos = player.transform.position;
        Material skin = player.GetComponentInChildren<SkinnedMeshRenderer>().material;

        //Delete everyone and map
        GameObject.Find("GameManagerObject").GetComponent<GameManager>().enabled = false;
        FindObjectOfType<Environment>().CleanUpWorld(); 
        FindObjectOfType<Environment>().SetMapSize(FindObjectOfType<Environment>().GetMapSize().x + 1, FindObjectOfType<Environment>().GetMapSize().y + 1);
        FindObjectOfType<Environment>().GenerateWorld();


        if (Physics.Raycast(new Vector3(pos.x, pos.y + 5, pos.z), -transform.up, out RaycastHit rayHit, Mathf.Infinity))
        {
            FindObjectOfType<Game>().SpawnNewCharacter(rayHit.collider.gameObject, skin);
        }

        FindObjectOfType<Game>().SelectCharacter(FindObjectOfType<Character>());
        GameObject.Find("GameManagerObject").GetComponent<GameManager>().enabled = true; 
    } 

    public void SetupWall()
    {
        offsetEnabled = !offsetEnabled;
        GameObject closestWall = GameObject.Find("ClosestWall");
        GameObject furthestWall = GameObject.Find("FurtherWall");
        GameObject leftWall = GameObject.Find("LeftWall");
        GameObject rightWall = GameObject.Find("RightWall");
        int mapSize = FindObjectOfType<Environment>().GetMapSize().x;


        if (offsetEnabled == true)
        {
            closestWall.transform.position = new Vector3((GameObject.Find("Tile(0,0)").transform.position.x + GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x) / 2 + 2.55f, closestWall.transform.position.y, GameObject.Find("Tile(0,0)").transform.position.z - .5f);
            furthestWall.transform.position = new Vector3((GameObject.Find("Tile(0,0)").transform.position.x + GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x) / 2 + 2.55f, furthestWall.transform.position.y, GameObject.Find("Tile(0," + (mapSize - 1) + ")").transform.position.z + 5.5f);
            leftWall.transform.position = new Vector3(GameObject.Find("Tile(0,0)").transform.position.x - .5f, leftWall.transform.position.y, (GameObject.Find("Tile(0,0)").transform.position.x + GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x) / 2 + 2.55f);
            rightWall.transform.position = new Vector3(GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x + 5.5f, rightWall.transform.position.y, (GameObject.Find("Tile(0,0)").transform.position.x + GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x) / 2 + 2.55f);
        }
        else
        {
            closestWall.transform.position = new Vector3((GameObject.Find("Tile(0,0)").transform.position.x + GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x) / 2, closestWall.transform.position.y, GameObject.Find("Tile(0,0)").transform.position.z - 5.5f);
            furthestWall.transform.position = new Vector3((GameObject.Find("Tile(0,0)").transform.position.x + GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x) / 2, furthestWall.transform.position.y, GameObject.Find("Tile(0," + (mapSize - 1) + ")").transform.position.z + 5.5f);
            leftWall.transform.position = new Vector3(GameObject.Find("Tile(0,0)").transform.position.x - 5.5f, leftWall.transform.position.y, (GameObject.Find("Tile(0,0)").transform.position.x + GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x) / 2 );
            rightWall.transform.position = new Vector3(GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x + 5.5f, rightWall.transform.position.y, (GameObject.Find("Tile(0,0)").transform.position.x + GameObject.Find("Tile(" + (mapSize - 1) + ",0)").transform.position.x) / 2);
        }

        closestWall.transform.localScale = new Vector3(closestWall.transform.localScale.x, closestWall.transform.localScale.y, (mapSize * 5) + 2);
        furthestWall.transform.localScale = new Vector3(furthestWall.transform.localScale.x, furthestWall.transform.localScale.y, (mapSize * 5) + 2);
        leftWall.transform.localScale = new Vector3(leftWall.transform.localScale.x, leftWall.transform.localScale.y, mapSize * 5);
        rightWall.transform.localScale = new Vector3(rightWall.transform.localScale.x, rightWall.transform.localScale.y, mapSize * 5);
    }

    public void ResetScore()
    {
        ScoreManager.ResetScoreManager(multiplierUI);
        ScoreManager.UpdateScoreBoard(textUi);
        tempScoreTime = 0;
        difficultyIncrease = 1;
        timeToSpawnKey = 0;
        timeBeforeRoomIsDestroyed = 10;
        spawnAKey = false;
        timeToGetKeyTime.text = "";

        offsetEnabled = false;
        SetupWall();
        offsetEnabled = false;
    }

    private void SpawnConsumable(GameObject customObject, float yOffset)
    {
        GameObject[] trapDoors = GameObject.FindGameObjectsWithTag("TrapDoorTag");
        Vector3 randomTile = trapDoors[Random.Range(0, trapDoors.Length)].transform.position;
        Vector3 positionTile = new Vector3(randomTile.x, GameObject.Find("Environment").GetComponent<Environment>().GetTileHeightY() + yOffset, randomTile.z);
        GameObject newObject = Instantiate(customObject, positionTile, Quaternion.identity);
        newObject.transform.SetParent(transform);
    }

    // Check if there is 0 players left if so then display gameover screen
    public void ChangeInPlayers()
    { 
        if(GameObject.FindGameObjectsWithTag("Players").Length == 0)
        {
            scoreScreenUi.text = ScoreManager.GetScore().ToString();
            GameObject.Find("Game").GetComponent<Game>().EndGame();
            GameObject.Find("Game").GetComponent<Game>().DisableCanvas(HUD);
            GameObject.Find("Game").GetComponent<Game>().EnableCanvas(deathScreenCanvas);

            if (FileSaving.GetScoreFromFile() == -1)
            {
                highscoreScreenUi.text = "File not found";
                Debug.LogError("File doesnt exist");
            }
            else
            {
                if (FileSaving.GetScoreFromFile() < ScoreManager.GetScore())
                {
                    FileSaving.SetScoreToFile(ScoreManager.GetScore());
                    highscoreScreenUi.text = FileSaving.GetScoreFromFile().ToString();
                }
                else
                {
                    highscoreScreenUi.text = FileSaving.GetScoreFromFile().ToString();
                }
            }

            FindObjectOfType<MainMenuScore>().UpdateScore();
        }
    }   
}
