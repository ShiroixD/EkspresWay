using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public float Speed { get => _currentSpeed; set => _currentSpeed = value; }
    public long PointsCounter { get => _pointsCounter; set => _pointsCounter = value; }
    public int AntiStunTapCounter { get; set; } = -1;
    public float RemainingTime { get => _currentTime; set => _currentTime = value; }
    public GameState GameState { get => _gameState; set => _gameState = value; }
    public Player Player { get => _player; set => _player = value; }
    public MusicManager MusicManager { get => _musicManager; set => _musicManager = value; }
    public int Combo { get => _combo; set => _combo = value; }
    public List<float> PointRecords { get => _pointRecords; }

    [SerializeField]
    private Camera _camera;

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
    private float _maxSpeedLimit = 15f;

    [SerializeField]
    private float _currentSpeed = 0f;

    [SerializeField]
    private float _timeLimitMin = 1;

    [SerializeField]
    private float _speedDelta = 0.5f;

    [SerializeField]
    private float _raiseSpeedInterval = 15.0f;

    [SerializeField]
    private float _globalLimitSpeedLimit = 15;

    [SerializeField]
    private float _globalLimitSpawnTimeDelay = 0.12f;

    [SerializeField]
    private float _globalLimitObstaclePercentage = 0.1f;

    [SerializeField]
    private int globalLimitObstacleGap = 3;

    [SerializeField]
    private int _globalLimitComboTimeBonuslimit = 10;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private List<GameObject>_backgroundPacks;
    private GameObject _activeBackgrund;

    private int _currentStage;
    private float _currentTime;
    private int _combo = 0;
    private List<float> _pointRecords;

    private GameState _gameState;
    private long _pointsCounter;

    void Start()
    {
        _activeBackgrund = _backgroundPacks[0];
        _currentStage = 1;
        _pointsCounter = 0;
        _currentSpeed = 0;
        _pointRecords = new List<float>();
        _gameState = GameState.MENU;
        _uiManager.ShowStartUi(_currentStage);
        _currentTime = _timeLimitMin * 60.0f;
        _uiManager.SetRemainingTime(_currentTime);
        Debug.Log(Application.persistentDataPath);
        LoadGame();
    }

    void Update()
    {
        if (_gameState == GameState.IN_PROGRESS && _currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            _uiManager.SetRemainingTime(_currentTime);
            if (_currentTime <= 0)
            {
                CompletedStage();
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

    public void StartGame()
    {
        _activeBackgrund.SetActive(true);
        _gameState = GameState.IN_PROGRESS;
        _currentSpeed = _startSpeed;
        _uiManager.HideStartUi();
        _uiManager.ShowInGameUi();
        _playerObject.SetActive(true);
        _obstacles.SetActive(true);
        _mapGenerator.StartGenerating();
        StartCoroutine(RaiseSpeed(_raiseSpeedInterval));
    }

    public void GameOver()
    {
        _musicManager.PlayGameOverMusic();
        _currentSpeed = 0.0f;
        AntiStunTapCounter = -1;
        _gameState = GameState.GAME_OVER;
        _obstacles.SetActive(false);
        _uiManager.ShowGameOverUi(_currentStage);
        _uiManager.HideInGameUi();
        _player.transform.parent.gameObject.SetActive(false);
        _combo = 0;
        RemoveObstacles();
    }

    public void RetryGame()
    {
        _musicManager.PlayRandomBackgroundMusic();
        _currentSpeed = _startSpeed;
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
        StartCoroutine(RaiseSpeed(_raiseSpeedInterval));
    }

    public void DecreaseAntiStunTapCounter()
    {
        AudioSource playerAudioSource = _player.GetComponent<AudioSource>();
        _musicManager.PlaySourceWithClip(playerAudioSource, "decreaseStun");
        _player.PlayAnimation("Shake");
        AntiStunTapCounter--;
    }

    public void PlayerWasStunned()
    {
        _camera.GetComponent<Animation>().Play("Camera");
        _currentSpeed = 0;
        _uiManager.ShowAntiStunButton();
        AntiStunTapCounter = 10;
    }

    public void NextStage()
    {
        _activeBackgrund.SetActive(false);
        int random = UnityEngine.Random.Range(0, 4);
        _activeBackgrund = _backgroundPacks[random];
        _activeBackgrund.SetActive(true);
        _gameState = GameState.IN_PROGRESS;
        _player.IsStunned = false;
        _currentSpeed = _startSpeed;
        _obstacles.SetActive(true);
        _uiManager.HideTimeOutUi();
        _uiManager.ShowInGameUi();
        _uiManager.SetRemainingTime(_currentTime);
        _player.transform.parent.gameObject.SetActive(true);
        _mapGenerator.StartGenerating();
        StartCoroutine(RaiseSpeed(_raiseSpeedInterval));
    }

    public void ResetGame()
    {
        _currentStage = 1;
        _pointsCounter = 0;
        _currentSpeed = 0.0f;
        _speedLimit = 4f;
        _timeLimitMin = 0.5f;
        _currentTime = _timeLimitMin * 60.0f;
        _pointRecords.Clear();
        AntiStunTapCounter = -1;
        _mapGenerator.SpawnTimeDelay = 0.36f;
        _mapGenerator.ObstaclesPercentage = 0.9f;
        _mapGenerator.ObstacleGap = 10;
        _mapGenerator.ComboTimeBonusLimit = 30;
        _combo = 0;
        _uiManager.HideStartUi();
        _uiManager.ShowStartUi(_currentStage);
        _uiManager.SetRemainingTime(_currentTime);
        SaveGame();
    }

    private void SaveGame()
    {
        SaveSystem.SaveGameData
        (
            stage: _currentStage,
            speedLimit: _speedLimit,
            timeLimit: _timeLimitMin,
            spawnTimeDelay: _mapGenerator.SpawnTimeDelay,
            obstaclePercentage: _mapGenerator.ObstaclesPercentage,
            obstacleGap: _mapGenerator.ObstacleGap,
            comboTimeBonusLimit: _mapGenerator.ComboTimeBonusLimit,
            pointsRecords: _pointRecords,
            activeBackground: _backgroundPacks.IndexOf(_activeBackgrund)
        );
    }

    private void LoadGame()
    {
        GameData gameData = SaveSystem.LoadGameData();
        if (gameData != null)
        {
            _activeBackgrund.SetActive(false);
            _currentStage = gameData.stage;
            _speedLimit = gameData.speedLimit;
            _timeLimitMin = gameData.timeLimit;
            _mapGenerator.SpawnTimeDelay = gameData.spawnTimeDelay;
            _mapGenerator.ObstaclesPercentage = gameData.obstaclePercentage;
            _mapGenerator.ObstacleGap = gameData.obstacleGap;
            _mapGenerator.ComboTimeBonusLimit = gameData.comboTimeBonusLimit;
            _pointRecords = new List<float>(gameData.pointRecords);
            _currentTime = _timeLimitMin * 60.0f;
            _uiManager.SetRemainingTime(_currentTime);
            _uiManager.ShowStartUi(_currentStage);
            _activeBackgrund = _backgroundPacks[gameData.activeBackground];
            _activeBackgrund.SetActive(true);
        }
        else
        {
            SaveGame();
        }
    }

    private IEnumerator RaiseSpeed(float time)
    {
        while (_gameState == GameState.IN_PROGRESS)
        {
            yield return new WaitForSecondsRealtime(time);
            if (_gameState != GameState.IN_PROGRESS)
                break;
            while (_player.IsStunned)
                yield return null;
            if (_player != null)
            {
                if (_currentSpeed < _speedLimit)
                    _currentSpeed += _speedDelta;
            }
        }
    }

    private void RestartSpeed()
    {
        _uiManager.HideAntiStunButton();
        _currentSpeed = _startSpeed;
        _combo = 0;
    }

    private void RemoveObstacles()
    {
        foreach (Transform child in _obstacles.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CompletedStage()
    {
        _gameState = GameState.COMPLETED;
        _currentSpeed = 0.0f;
        AntiStunTapCounter = -1;
        _currentStage += 1;
        if (_speedLimit < _globalLimitSpeedLimit)
            _speedLimit += 0.5f;
        _timeLimitMin += 1f / 6f;
        _currentTime = _timeLimitMin * 60.0f;
        if (_mapGenerator.SpawnTimeDelay > _globalLimitSpawnTimeDelay)
            _mapGenerator.SpawnTimeDelay -= 0.03f;
        if (_mapGenerator.ObstaclesPercentage > _globalLimitObstaclePercentage)
            _mapGenerator.ObstaclesPercentage -= 0.1f;
        if (_mapGenerator.ObstacleGap > globalLimitObstacleGap)
            _mapGenerator.ObstacleGap -= 1;
        if (_mapGenerator.ComboTimeBonusLimit > _globalLimitComboTimeBonuslimit)
            _mapGenerator.ComboTimeBonusLimit -= 1;
        _pointRecords.Add(_pointsCounter);
        _pointsCounter = 0;
        _combo = 0;
        RemoveObstacles();
        _obstacles.SetActive(false);
        _uiManager.HideInGameUi();
        _uiManager.ShowTimeOutUi(_currentStage - 1);
        _player.transform.parent.gameObject.SetActive(false);
        SaveGame();  
    }
}

public enum GameState
{
    MENU = 0,
    IN_PROGRESS = 1,
    COMPLETED = 2,
    GAME_OVER = 3
}