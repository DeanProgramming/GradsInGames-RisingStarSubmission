using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleAnimation : MonoBehaviour
{
    private float valueChange;
    private bool direction;
    [SerializeField] private Material[] possibleSkin;

    private void Start()
    {
        Material mat = possibleSkin[Random.Range(0, possibleSkin.Length)];
        //Loop through each body part and change the material
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer item in renderers)
        {
            item.material = mat;
        }
    }

    void Update()
    {
        transform.Rotate(transform.up, 1);
        Vector3 pos = transform.position;

        if (direction)
        {
            pos.y += 0.005f;
        }
        else
        {
            pos.y -= 0.005f;
        }

        valueChange += 0.005f;

        if (valueChange >= .5f)
        {
            direction = !direction;
            valueChange = 0;
        }
        transform.position = pos;
    }
}
