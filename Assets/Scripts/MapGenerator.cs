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
    private int _obsatcleGap = 5;

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

    private IEnumerator GenerateMap()
    {
        UnityEngine.Random random = new UnityEngine.Random();
        Vector3 spawnPosition = _obstaclesSpawnPoint.transform.position;
        float breakPercent = _obstaclesPercentage + 3 * ((1 - _obstaclesPercentage) / 4);
        int obstacleInt = 0;

        while (_gameManager.GameState == GameState.IN_PROGRESS && !_player.IsStunned)
        {

            float percent = UnityEngine.Random.Range(0.0f, 1.0f);
            if (percent < _obstaclesPercentage || obstacleInt < _obsatcleGap)
            {
                GameObject teeth = Instantiate(_obstaclesObjectArray[0]);
                teeth.transform.position = spawnPosition;
                teeth.transform.SetParent(_obstacles.transform);
                _gameManager.Combo++;
                if (_gameManager.Combo >= 20)
                {
                    float side = UnityEngine.Random.Range(0.0f, 1.0f);
                    GameObject timeBonus = Instantiate(_obstaclesObjectArray[4]);
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
                float side = UnityEngine.Random.Range(0.0f, 1.0f);
                GameObject tooth = GameObject.Instantiate(_obstaclesObjectArray[1]);
                GameObject thread = GameObject.Instantiate(_obstaclesObjectArray[2]);
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
                float side = UnityEngine.Random.Range(0.0f, 1.0f);
                GameObject tooth = Instantiate(_obstaclesObjectArray[1]);
                GameObject materialBreak = Instantiate(_obstaclesObjectArray[3]);


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
