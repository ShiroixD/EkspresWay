using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public float ScrollSpeed { get; set; }
    public long PointsCounter { get => _pointsCounter; set => _pointsCounter = value; }
    public float RemainingTime { get; set; }
    public GameState GameState { get => _gameState; set => _gameState = value; }

    [SerializeField] private UiManager _uiManager;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _obstacles;
    [SerializeField] private GameObject _obstaclesSpawnPoint;
    [SerializeField] private GameObject[] _obstaclesObjectArray;
    [SerializeField] [Range(0.0f, 1.0f)] private float _obstaclesPercentage;
    [SerializeField] private float _startSpeed = 5f;
    [SerializeField] private float _speedLimit = 10f;
    [SerializeField] private float _timeLimitMin = 1;
    [SerializeField] private Player _player;
    private float _currentTime;

    private GameState _gameState;
    private long _pointsCounter;

    void Start()
    {
        _pointsCounter = 0;
        ScrollSpeed = _startSpeed;
        _gameState = GameState.MENU;
        _uiManager.ShowStartUi();
        _currentTime = _timeLimitMin * 60.0f;
        _uiManager.SetRemainingTime(_currentTime);
    }

    void Update()
    {
        if (_gameState == GameState.IN_PROGRESS &&_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            _uiManager.SetRemainingTime(_currentTime);
        }
        else
            _uiManager.SetRemainingTime(0);
    }


    private void FixedUpdate()
    {
        if (_player != null)
        {
            if (!_player.IsStunned && _startSpeed < _speedLimit)
                ScrollSpeed += 0.001f;
        }
    }

    public void RestartSpeed()
    {
        ScrollSpeed = _startSpeed;
    }

    public void StartGame()
    {
        _gameState = GameState.IN_PROGRESS;
        _uiManager.HideStartUi();
        _uiManager.ShowInGameUi();
        _playerObject.SetActive(true);
            _obstacles.SetActive(true);
        StartCoroutine(GenerateMap());
    }

    public void GameOver()
    {
        this.ScrollSpeed = 0.0f;
        _gameState = GameState.GAME_OVER;
        _uiManager.ShowGameOverUi();
        _uiManager.HideInGameUi();
    }

    public void RetryGame()
    {
        this.ScrollSpeed = _startSpeed;
        this._pointsCounter = 0;
        _gameState = GameState.IN_PROGRESS;
        _uiManager.ShowInGameUi();
        _uiManager.HideGameOverUi();
        _currentTime = _timeLimitMin * 60.0f;
        _uiManager.SetRemainingTime(_currentTime);
        StartCoroutine(GenerateMap());

    }


    public IEnumerator GenerateMap()
    {
        UnityEngine.Random random = new UnityEngine.Random();
        Vector3 spawnPosition = _obstaclesSpawnPoint.transform.position;
        float breakPercent = _obstaclesPercentage + (1 - _obstaclesPercentage) / 2;

        while (_gameState == GameState.IN_PROGRESS && !_player.IsStunned)
        {

                float percent = UnityEngine.Random.Range(0.0f, 1.0f);
                if (percent < _obstaclesPercentage)
                {
                    GameObject newObscatles = GameObject.Instantiate(_obstaclesObjectArray[0]);
                    newObscatles.transform.position = spawnPosition;
                    newObscatles.transform.SetParent(_obstacles.transform);
                }
                else if (percent >= _obstaclesPercentage && percent < breakPercent)
                {
                    float side = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (side < 0.5f)
                    {
                        GameObject leftNewObscatles = GameObject.Instantiate(_obstaclesObjectArray[1]);
                        leftNewObscatles.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                        leftNewObscatles.transform.SetParent(_obstacles.transform);

                        GameObject RightNewObscatles = GameObject.Instantiate(_obstaclesObjectArray[2]);
                        RightNewObscatles.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                        RightNewObscatles.transform.SetParent(_obstacles.transform);
                    }
                    else
                    {
                        GameObject leftNewObscatles = GameObject.Instantiate(_obstaclesObjectArray[1]);
                        leftNewObscatles.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                        leftNewObscatles.transform.SetParent(_obstacles.transform);

                        GameObject RightNewObscatles = GameObject.Instantiate(_obstaclesObjectArray[2]);
                        RightNewObscatles.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                        RightNewObscatles.transform.SetParent(_obstacles.transform);
                    }
                }
                else
                {
                    float side = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (side < 0.5f)
                    {
                        GameObject leftNewObscatles = GameObject.Instantiate(_obstaclesObjectArray[1]);
                        leftNewObscatles.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                        leftNewObscatles.transform.SetParent(_obstacles.transform);

                        GameObject RightNewObscatles = GameObject.Instantiate(_obstaclesObjectArray[3]);
                        RightNewObscatles.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                        RightNewObscatles.transform.SetParent(_obstacles.transform);
                    }
                    else
                    {
                        GameObject leftNewObscatles = GameObject.Instantiate(_obstaclesObjectArray[1]);
                        leftNewObscatles.transform.position = new Vector3(1.2f, spawnPosition.y, spawnPosition.z);
                        leftNewObscatles.transform.SetParent(_obstacles.transform);

                        GameObject RightNewObscatles = GameObject.Instantiate(_obstaclesObjectArray[3]);
                        RightNewObscatles.transform.position = new Vector3(-1.2f, spawnPosition.y, spawnPosition.z);
                        RightNewObscatles.transform.SetParent(_obstacles.transform);
                    }
                }
                yield return new WaitForSecondsRealtime(0.3f);
        }
        yield return null;
    }
}


public enum GameState
{
    MENU = 0,
    IN_PROGRESS = 1,
    COMPLETED = 2,
    GAME_OVER = 3
}