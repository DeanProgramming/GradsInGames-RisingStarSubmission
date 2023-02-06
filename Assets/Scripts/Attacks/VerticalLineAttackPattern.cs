using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLineAttackPattern : MonoBehaviour
{    
    public static void VerticalLineAttack(AudioClip clip)
    {
        EnvironmentTile[][] mapTiles = FindObjectOfType<Environment>().GetMap();
        int line = Random.Range(0, FindObjectOfType<Environment>().GetMapSize().x);

        for (int i = 0; i < FindObjectOfType<Environment>().GetMapSize().y; i++)
        {
            Transform tile = mapTiles[i][line].transform.Find("TrapDoorObject");

            if (tile != null)
            {
                tile.gameObject.TryGetComponent(out TeleportTrap trap);

                if (trap == null)
                {
                    tile.gameObject.AddComponent<TeleportTrap>();
                    tile.gameObject.GetComponent<TeleportTrap>().timeRequired = 2;
                    tile.gameObject.GetComponent<TeleportTrap>().clip = clip;
                }
            }
        }
    }
}
