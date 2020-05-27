using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public double ObstacleTimeGenerator;
    public Obstacle[] ObstaclesObjects;
    private int _pointsCounter;

    void Start()
    {
        _pointsCounter = 0;
    }

    void Update()
    {
        
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
