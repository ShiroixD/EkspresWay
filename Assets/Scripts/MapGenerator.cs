using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _obstaclesSpawnPoint;

    [SerializeField]
    private GameObject[] _obstaclesObjectArray;

    [SerializeField]
    private GameObject _obstacles;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _obstaclesPercentage;

    [SerializeField]
    private int _obstacleGap = 5;

    private Player _player;
    
    void Start()
    {
        _player = _gameManager.Player;
    }

    void Update()
    {
        
    }

    public void StartGenerating()
    {
        StartCoroutine(GenerateMap());
    }

    private GameObject SpawnGameObject(string name)
    {
        GameObject objectToSpawn = null;
        switch(name)
        {
            case "teeth":
                {
                    objectToSpawn = _obstaclesObjectArray[0];
                    break;
                }
            case "tooth":
                {
                    objectToSpawn = _obstaclesObjectArray[1];
                    break;
                }
            case "thread":
                {
                    objectToSpawn = _obstaclesObjectArray[2];
                    break;
                }
            case "materialBreak":
                {
                    objectToSpawn = _obstaclesObjectArray[3];
                    break;
                }
            case "timeBonus":
                {
                    objectToSpawn = _obstaclesObjectArray[4];
                    break;
                }
        }
        if (objectToSpawn != null)
            return Instantiate(objectToSpawn);
        else return null;
    }

    private IEnumerator GenerateMap()
    {
        Vector3 spawnPosition = _obstaclesSpawnPoint.transform.position;
        float breakPercent = _obstaclesPercentage + 3 * ((1 - _obstaclesPercentage) / 4);
        int obstacleInt = 0;

        while (_gameManager.GameState == GameState.IN_PROGRESS && !_player.IsStunned)
        {
            float percent = Random.Range(0.0f, 1.0f);
            if (percent < _obstaclesPercentage || obstacleInt < _obstacleGap)
            {
                GameObject teeth = SpawnGameObject("teeth");
                teeth.transform.position = spawnPosition;
                teeth.transform.SetParent(_obstacles.transform);
                _gameManager.Combo++;
                if (_gameManager.Combo >= 20)
                {
                    float side = Random.Range(0.0f, 1.0f);
                    GameObject timeBonus = SpawnGameObject("timeBonus");
                    if (side < 0.5f)
                    {
                        timeBonus.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z); ;
                        timeBonus.transform.SetParent(_obstacles.transform);
                    }
                    else
                    {
                        timeBonus.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z); ;
                        timeBonus.transform.SetParent(_obstacles.transform);
                    }
                    _gameManager.Combo = 0;
                }
                obstacleInt++;
            }
            else if (percent >= _obstaclesPercentage && percent < breakPercent)
            {
                float side = Random.Range(0.0f, 1.0f);
                GameObject tooth = SpawnGameObject("tooth");
                GameObject thread = SpawnGameObject("thread");
                if (side < 0.5f)
                {
                    tooth.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                    tooth.transform.SetParent(_obstacles.transform);

                    thread.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                    thread.transform.SetParent(_obstacles.transform);
                }
                else
                {
                    tooth.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                    tooth.transform.SetParent(_obstacles.transform);

                    thread.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                    thread.transform.SetParent(_obstacles.transform);
                }
                obstacleInt = 0;
            }
            else
            {
                float side = Random.Range(0.0f, 1.0f);
                GameObject tooth = SpawnGameObject("tooth");
                GameObject materialBreak = SpawnGameObject("materialBreak");

                if (side < 0.5f)
                {
                    tooth.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                    tooth.transform.SetParent(_obstacles.transform);

                    materialBreak.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                    materialBreak.transform.SetParent(_obstacles.transform);
                }
                else
                {
                    tooth.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                    tooth.transform.SetParent(_obstacles.transform);

                    materialBreak.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                    materialBreak.transform.SetParent(_obstacles.transform);
                }
                obstacleInt = 0;
            }
            yield return new WaitForSecondsRealtime(0.3f);
        }
        yield return null;
    }
}
