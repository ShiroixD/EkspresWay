﻿using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsStunned { get; set; }
    public GameObject CurrentHitObstacle { get; set; }

    [SerializeField]
    private GameObject _model;

    [SerializeField]
    private SwipeDetector _swipeDetector;

    [SerializeField]
    private GameManager _gameManager;

    private Vector3 _startingPos;
    private Animator _animator;

    void Start()
    {
        _startingPos = transform.position;
        IsStunned = false;
        _animator = _model.GetComponent<Animator>();
    }

    void Update()
    {
    }

    public void TiltToSide(SwipeData data)
    {
        Debug.Log("Swipe in Direction: " + data.Direction);

        if (data.Direction == SwipeDirection.Left)
        {
            transform.position = new Vector3(_startingPos.x - 1f, _startingPos.y, _startingPos.z);
        }
        else if (data.Direction == SwipeDirection.Right)
        {
            transform.position = new Vector3(_startingPos.x + 1f, _startingPos.y, _startingPos.z);
        }
    }

    public void ReturnToNormal()
    {
        Debug.Log("Returning to normal");
        transform.position = new Vector3(_startingPos.x, _startingPos.y, _startingPos.z);
    }

    public void PlayAnimation(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Teeth":
                {
                    _gameManager.PointsCounter += 1;
                    collision.gameObject.GetComponent<Animation>().Play("zip"); 
                    break;
                }
            case "Tooth":
                {
                    collision.gameObject.GetComponentInChildren<Animation>().Play();
                    break;
                }
            case "Thread":
                {
                    AudioSource audioSource = GetComponent<AudioSource>();
                    _gameManager.MusicManager.PlaySourceWithClip(audioSource, "hitObstacle");
                    IsStunned = true;
                    CurrentHitObstacle = collision.gameObject;
                    _gameManager.PlayerWasStunned();
                    break;
                }
            case "MaterialBreak":
                {
                    AudioSource audioSource = GetComponent<AudioSource>();
                    _gameManager.MusicManager.PlaySourceWithClip(audioSource, "hitObstacle");
                    Destroy(collision.gameObject);
                    _gameManager.GameOver();
                    break;
                }
            case "TimeBonus":
                {
                    AudioSource audioSource = GetComponent<AudioSource>();
                    _gameManager.MusicManager.PlaySourceWithClip(audioSource, "timeBonus");
                    _gameManager.RemainingTime += 5.0f;
                    Destroy(collision.gameObject);
                    break;
                }
        }
    }
}
