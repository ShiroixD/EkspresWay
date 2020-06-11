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
    public Player Player { get => _player; set => _player = value; }
    public MusicManager MusicManager { get => _musicManager; set => _musicManager = value; }
    public int Combo { get => _combo; set => _combo = value; }

    [SerializeField]
    private UiManager _uiManager;

    [SerializeField]
    private MusicManager _musicManager;

    [SerializeField]
    private MapGenerator _mapGenerator;

    [SerializeField]
    private GameObject _playerObject;

    [SerializeField]
    private GameObject _obstacles;

    [SerializeField]
    private SwipeDetector _swipeDetector;

    [SerializeField]
    private float _startSpeed = 5f;

    [SerializeField]
    private float _speedLimit = 10f;

    [SerializeField]
    private float _timeLimitMin = 1;

    [SerializeField]
    private float _speedDelta = 0.001f;

    [SerializeField]
    private Player _player;

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
            if (AntiStunTapCounter == 0)
            {
                AntiStunTapCounter = -1;
                AudioSource audioSource = _player.GetComponent<AudioSource>();
                _musicManager.PlaySourceWithClip(audioSource, "stunRelease");
                _player.IsStunned = false;
                if (_player.CurrentHitObstacle != null)
                {
                    _player.CurrentHitObstacle.GetComponent<Obstacle>().Disappear();
                    _player.CurrentHitObstacle = null;
                }
                RestartSpeed();
                _mapGenerator.StartGenerating();
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
                ScrollSpeed += _speedDelta;
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
        _mapGenerator.StartGenerating();
    }

    public void TimeOut()
    {
        ScrollSpeed = 0.0f;
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
        _musicManager.PlayGameOverMusic();
        ScrollSpeed = 0.0f;
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
        _musicManager.PlayRandomBackgroundMusic();
        ScrollSpeed = _startSpeed;
        _pointsCounter = 0;
        _combo = 0;
        _gameState = GameState.IN_PROGRESS;
        _obstacles.SetActive(true);
        _uiManager.ShowInGameUi();
        _uiManager.HideGameOverUi();
        _currentTime = _timeLimitMin * 60.0f;
        _uiManager.SetRemainingTime(_currentTime);
        _player.transform.parent.gameObject.SetActive(true);
        _mapGenerator.StartGenerating();
    }


    public void DecreaseAntiStunTapCounter()
    {
         AntiStunTapCounter--;
        _player.PlayAnimation("Shake");
        AudioSource playerAudioSource = _player.GetComponent<AudioSource>();
        _musicManager.PlaySourceWithClip(playerAudioSource, "decreaseStun");
    }

    public void PlayerWasStunned()
    {
        GameObject.Find("Main Camera").GetComponent<Animation>().Play("Camera");
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