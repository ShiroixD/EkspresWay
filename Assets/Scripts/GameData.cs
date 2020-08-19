using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int stage;
    public float speedLimit;
    public float timeLimit;
    public float spawnTimeDelay;
    public float obstaclePercentage;
    public int obstacleGap;
    public int comboTimeBonusLimit;
    public int activeBackground;
    public List<float> pointRecords;

    public GameData(int stage, float speedLimit, float timeLimit, float spawnTimeDelay, float obstaclePercentage, int obstacleGap, int comboTimeBonusLimit, List<float> pointsRecords, int activeBackground)
    {
        this.stage = stage;
        this.speedLimit = speedLimit;
        this.timeLimit = timeLimit;
        this.spawnTimeDelay = spawnTimeDelay;
        this.obstaclePercentage = obstaclePercentage;
        this.obstacleGap = obstacleGap;
        this.comboTimeBonusLimit = comboTimeBonusLimit;
        this.activeBackground = activeBackground;
        this.pointRecords = pointsRecords;
    }
}
