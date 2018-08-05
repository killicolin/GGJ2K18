using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSound : MonoBehaviour
{

    public List<AudioClip> Sounds;

    private AudioSource audioSource;
    private int Count
    {
        get
        {
            return Sounds.Count;
        }
    }

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playDeathSound()
    {
        if (Sounds.Count > 0)
        {
            int soundNumber;
            soundNumber = Mathf.RoundToInt(Random.value * Count);
            audioSource.clip = Sounds[soundNumber];
            audioSource.Play();
        }
    }

    public void Update()
    {
        

    }
}
