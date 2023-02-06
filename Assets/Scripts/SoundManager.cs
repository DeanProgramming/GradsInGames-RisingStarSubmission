using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeLevel;
    [SerializeField] private AudioSource audioLevel;

    private void Start()
    {
        volumeLevel.value = audioLevel.volume;
    }

    public void VolumeChange()
    {
        audioLevel.volume = volumeLevel.value;
    }
}
