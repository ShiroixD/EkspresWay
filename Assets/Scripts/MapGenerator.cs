using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public float SpawnTimeDelay { get => _spawnTimeDelay; set => _spawnTimeDelay = value; }
    public float ObstaclesPercentage { get => _obstaclesPercentage; set => _obstaclesPercentage = value; }
    public int ObstacleGap { get => _obstacleGap; set => _obstacleGap = value; }
    public int ComboTimeBonusLimit { get => _comboTimeBonusLimit; set => _comboTimeBonusLimit = value; }

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _obstaclesSpawnPoint;

    [SerializeField]
    private GameObject[] _obstaclesObjectArray;

    [SerializeField]
    private GameObject _obstacles;

    [SerializeField]
    private float _spawnTimeDelay = 0.3f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _obstaclesPercentage;

    [SerializeField]
    private int _obstacleGap = 5;

    [SerializeField]
    private int _comboTimeBonusLimit = 2;

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
            case "leftTooth":
                {
                    objectToSpawn = _obstaclesObjectArray[1];
                    break;
                }
            case "rightTooth":
                {
                    objectToSpawn = _obstaclesObjectArray[2];
                    break;
                }
            case "thread_left":
                {
                    objectToSpawn = _obstaclesObjectArray[3];
                    break;
                }
            case "thread_right":
                {
                    objectToSpawn = _obstaclesObjectArray[6];
                    break;
                }
            case "leftMaterialBreak":
                {
                    objectToSpawn = _obstaclesObjectArray[4];
                    break;
                }
            case "rightMaterialBreak":
                {
                    objectToSpawn = _obstaclesObjectArray[7];
                    break;
                }
            case "timeBonus":
                {
                    objectToSpawn = _obstaclesObjectArray[5];
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
                if (_gameManager.Combo >= _comboTimeBonusLimit)
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
                GameObject tooth;
                if (side < 0.5f)
                {
                    GameObject thread = SpawnGameObject("thread_right");
                    tooth = SpawnGameObject("leftTooth");
                    tooth.transform.position = new Vector3(-0.1f, spawnPosition.y, spawnPosition.z);
                    tooth.transform.SetParent(_obstacles.transform);

                    thread.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                    thread.transform.SetParent(_obstacles.transform);
                }
                else
                {
                    GameObject thread = SpawnGameObject("thread_left");
                    tooth = SpawnGameObject("rightTooth");
                    tooth.transform.position = new Vector3(0.1f, spawnPosition.y, spawnPosition.z);
                    tooth.transform.SetParent(_obstacles.transform);

                    thread.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                    thread.transform.SetParent(_obstacles.transform);
                }
                obstacleInt = 0;
            }
            else
            {
                float side = Random.Range(0.0f, 1.0f);
                GameObject tooth;
                GameObject materialBreak;

                if (side < 0.5f)
                {
                    materialBreak = SpawnGameObject("rightMaterialBreak");
                    tooth = SpawnGameObject("leftTooth");
                    tooth.transform.position = new Vector3(-0.1f, spawnPosition.y, spawnPosition.z);
                    tooth.transform.SetParent(_obstacles.transform);

                    materialBreak.transform.position = new Vector3(0.1f, spawnPosition.y, spawnPosition.z);
                    materialBreak.transform.SetParent(_obstacles.transform);
                }
                else
                {
                    materialBreak = SpawnGameObject("leftMaterialBreak");
                    tooth = SpawnGameObject("rightTooth");
                    tooth.transform.position = new Vector3(0.1f, spawnPosition.y, spawnPosition.z);
                    tooth.transform.SetParent(_obstacles.transform);

                    materialBreak.transform.position = new Vector3(-0.1f, spawnPosition.y, spawnPosition.z);
                    materialBreak.transform.SetParent(_obstacles.transform);
                }
                obstacleInt = 0;
            }
            yield return new WaitForSecondsRealtime(_spawnTimeDelay);
        }
        yield return null;
    }
}
