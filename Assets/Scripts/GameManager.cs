using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float ScrollSpeed { get; set; }
    public int PointsCounter { get => _pointsCounter; set => _pointsCounter += value; }

    [SerializeField] private float _startSpeed = 5f;
    [SerializeField] private float _speedLimit = 10f;
    [SerializeField] private Player _player;
    private int _pointsCounter;

    void Start()
    {
        _pointsCounter = 0;
        ScrollSpeed = _startSpeed;
    }

    void Update()
    {
    }


    private void FixedUpdate()
    {
        if(!_player.IsStunned && _startSpeed < _speedLimit)
            ScrollSpeed += 0.001f;
    }

    public void RestartSpeed()
    {
        ScrollSpeed = _startSpeed;
    }
}
