using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalAttackPattern : MonoBehaviour
{
    public static void StartDiagonalAttack(AudioClip clip)
    {
        int i = Random.Range(0, FindObjectOfType<Environment>().GetMapSize().x);
        int x = i;

        if (i < FindObjectOfType<Environment>().GetMapSize().x / 2) //Right attack
        {
            for (int e = 0; e < FindObjectOfType<Environment>().GetMapSize().x; e++)
            {
                DiagonalAttack(x, e, clip);
                x++;
            }
        }
        else // Left attack
        {
            for (int e = 0; e < FindObjectOfType<Environment>().GetMapSize().x; e++)
            {
                DiagonalAttack(x, e, clip);
                x--;
            }
        }
    }

    private static void DiagonalAttack(int x, int y, AudioClip clip)
    {
        EnvironmentTile[][] mapTiles = FindObjectOfType<Environment>().GetMap();

        if (!(x >= FindObjectOfType<Environment>().GetMapSize().x || y >= FindObjectOfType<Environment>().GetMapSize().y || x < 0 || y < 0))
        {
            Transform tile = mapTiles[x][y].transform.Find("TrapDoorObject");

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
