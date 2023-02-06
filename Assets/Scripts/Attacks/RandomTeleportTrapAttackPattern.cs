using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTeleportTrapAttackPattern
{
    public static void RandomTeleportTrap(int amountOfTraps, float difficultyIncrease, AudioClip clip)
    {
        GameObject[] trapDoors = GameObject.FindGameObjectsWithTag("TrapDoorTag");

        // Find all gameobjects with tag trap doors then randomly pick 6 to open
        // If the door doesnt have trapdoor script then add it 
        for (int i = 0; i < amountOfTraps; i++)
        {
            int randomNumber = Random.Range(0, trapDoors.Length);
            trapDoors[randomNumber].TryGetComponent(out TeleportTrap trap);

            if (trap == null)
            {
                trapDoors[randomNumber].AddComponent<TeleportTrap>();
                trapDoors[randomNumber].GetComponent<TeleportTrap>().timeRequired = 1.5f * difficultyIncrease;
                trapDoors[randomNumber].gameObject.GetComponent<TeleportTrap>().clip = clip;
            }
        }
    }
}
