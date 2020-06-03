using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public float ScrollSpeed { get; set; }
    public long PointsCounter { get => _pointsCounter; set => _pointsCounter = value; }
    public int AntiStunTapCounter { get; set; } = -1;
    public float RemainingTime { get => _currentTime; set => _currentTime = value; }
    public GameState GameState { get => _gameState; set => _gameState = value; }

    [SerializeField] private UiManager _uiManager;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _obstacles;
    [SerializeField] private GameObject _obstaclesSpawnPoint;
    [SerializeField] private GameObject[] _obstaclesObjectArray;
    [SerializeField] private SwipeDetector _swipeDetector;
    [SerializeField] [Range(0.0f, 1.0f)] private float _obstaclesPercentage;
    [SerializeField] private float _startSpeed = 5f;
    [SerializeField] private float _speedLimit = 10f;
    [SerializeField] private float _timeLimitMin = 1;
    [SerializeField] private int _obsatcleGap = 5;
    [SerializeField] private Player _player;
    private float _currentTime;
    private int _combo = 0;

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
        if (_gameState == GameState.IN_PROGRESS && _currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            _uiManager.SetRemainingTime(_currentTime);
            if (_currentTime <= 0)
            {
                TimeOut();
            }
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
        _uiManager.HideAntiStunButton();
        ScrollSpeed = _startSpeed;
        _combo = 0;
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

    public void TimeOut()
    {
        this.ScrollSpeed = 0.0f;
        AntiStunTapCounter = -1;
        _gameState = GameState.COMPLETED;
        _obstacles.SetActive(false);
        _uiManager.ShowGameOverUi();
        _uiManager.HideInGameUi();
        _player.transform.parent.gameObject.SetActive(false);
        _combo = 0;
    }

    public void GameOver()
    {
        this.ScrollSpeed = 0.0f;
        AntiStunTapCounter = -1;
        _gameState = GameState.GAME_OVER;
        _obstacles.SetActive(false);
        _uiManager.ShowGameOverUi();
        _uiManager.HideInGameUi();
        _player.transform.parent.gameObject.SetActive(false);
        _combo = 0;
    }

    public void RetryGame()
    {
        this.ScrollSpeed = _startSpeed;
        this._pointsCounter = 0;
        _combo = 0;
        _gameState = GameState.IN_PROGRESS;
        _obstacles.SetActive(true);
        _uiManager.ShowInGameUi();
        _uiManager.HideGameOverUi();
        _currentTime = _timeLimitMin * 60.0f;
        _uiManager.SetRemainingTime(_currentTime);
        _player.transform.parent.gameObject.SetActive(true);
        StartCoroutine(GenerateMap());

    }


    public IEnumerator GenerateMap()
    {
        UnityEngine.Random random = new UnityEngine.Random();
        Vector3 spawnPosition = _obstaclesSpawnPoint.transform.position;
        float breakPercent = _obstaclesPercentage + 3 * ((1 - _obstaclesPercentage) / 4) ;
        int obstacleInt = 0;

        while (_gameState == GameState.IN_PROGRESS && !_player.IsStunned)
        {

        float percent = UnityEngine.Random.Range(0.0f, 1.0f);
        if (percent < _obstaclesPercentage  || obstacleInt < _obsatcleGap)
        {
            GameObject teeth = GameObject.Instantiate(_obstaclesObjectArray[0]);
            teeth.transform.position = spawnPosition;
            teeth.transform.SetParent(_obstacles.transform);
            _combo++;
            if (_combo >= 20)
            {
                float side = UnityEngine.Random.Range(0.0f, 1.0f);
                GameObject timeBonus = GameObject.Instantiate(_obstaclesObjectArray[4]);
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
                _combo = 0;
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
            GameObject tooth = GameObject.Instantiate(_obstaclesObjectArray[1]);
            GameObject materialBreak = GameObject.Instantiate(_obstaclesObjectArray[3]);


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

    public void DecreaseAntiStunTapCounter()
    {
        AntiStunTapCounter--;
    }

    public void PlayerWasStunned()
    {
        ScrollSpeed = 0;
        AntiStunTapCounter = 10;
        _uiManager.ShowAntiStunButton();
    }
}


public enum GameState
{
    MENU = 0,
    IN_PROGRESS = 1,
    COMPLETED = 2,
    GAME_OVER = 3
}