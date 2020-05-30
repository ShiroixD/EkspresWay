using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float ScrollSpeed { get; set; }
    public int PointsCounter { get => _pointsCounter; set => _pointsCounter += value; }
    public float RemainingTime { get; set; }

    [SerializeField] private UiManager _uiManager;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _obstacles;
    [SerializeField] private float _startSpeed = 5f;
    [SerializeField] private float _speedLimit = 10f;
    [SerializeField] private float _timeLimitMin = 1;
    private Player _player;
    private float _currentTime;

    private GameState _gameState;
    private int _pointsCounter;

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
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _obstacles.SetActive(true);
    }

    public void GameOver()
    {

    }
}

public enum GameState
{
    MENU = 0,
    IN_PROGRESS = 1,
    COMPLETED = 2,
    GAME_OVER = 3
}