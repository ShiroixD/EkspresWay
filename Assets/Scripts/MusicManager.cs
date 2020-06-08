using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClips;

    void Start()
    {;
        int number = Random.Range(0, musicClips.Length);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClips[number];
        audioSource.Play();
    }
}
