using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _musicClips;
    [SerializeField] private AudioClip[] _sfx;
    [SerializeField] private AudioClip _gameOverClip;

    public AudioClip[] Sfx { get => _sfx; set => _sfx = value; }
    public AudioClip GameOverClip { get => _gameOverClip; set => _gameOverClip = value; }

    void Start()
    {
        RandomBackgroundMusciPlay();
    }

    public void RandomBackgroundMusciPlay()
    {
        int number = Random.Range(0, _musicClips.Length);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = _musicClips[number];
        audioSource.Play();
    }
}
