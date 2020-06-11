using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _musicClips;

    [SerializeField]
    private AudioClip[] _sounds;

    [SerializeField]
    private AudioClip _gameOverClip;

    void Start()
    {
        PlayRandomBackgroundMusic();
    }

    public void PlayRandomBackgroundMusic()
    {
        int number = Random.Range(0, _musicClips.Length);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = _musicClips[number];
        audioSource.Play();
    }

    public void PlayGameOverMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = _gameOverClip;
        audioSource.Play();
    }

    public void PlaySourceWithClip(AudioSource audioSource, string clipName)
    {
        audioSource.Stop();
        AudioClip clip = null;
        switch (clipName)
        {
            case "hitObstacle":
                {
                    clip = _sounds[0];
                    break;
                }
            case "decreaseStun":
                {
                    clip = _sounds[1];
                    break;
                }
            case "stunRelease":
                {
                    clip = _sounds[2];
                    break;
                }
            case "timeBonus":
                {
                    clip = _sounds[3];
                    break;
                }
        }
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
