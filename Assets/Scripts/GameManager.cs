using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public double ObstacleTimeGenerator;
    public Obstacle[] ObstaclesObjects;
    private int _pointsCounter;
    private float _scrollSpeed;
    public float StartingScrollSpeed = 5f;
    public GameObject Player;


    public float getScrollSpeed()
    {
        return _scrollSpeed;
    }

    public void setScrollSpeed(float value)
    {
        _scrollSpeed = value;
    }

    void Start()
    {
        _pointsCounter = 0;
        _scrollSpeed = StartingScrollSpeed;
    }

    void Update()
    {
    }


    private void FixedUpdate()
    {
        if(!Player.GetComponent<Player>().obstacleLockFlag)
            _scrollSpeed += 0.001f;
    }
    public int GetCurrentPoints()
    {
        return _pointsCounter;
    }

    public void IncreasePoints(int points)
    {
        _pointsCounter += points;
    }
}
