using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Player _player;
    [SerializeField] private float _delayToReadSwipe;
    [SerializeField] private float _delayToReturnCenterPosition = 0.1f;
    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;
    private bool _fingerPushed = false;
    private SwipeDirection _currentSwipeDirection = SwipeDirection.None;

    void Start()
    {
        Input.multiTouchEnabled = false;
    }

    void Update()
    {
        #if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!_player.IsStunned)
            {
                if (touch.phase == TouchPhase.Began && !_fingerPushed)
                {
                    _fingerUpPosition = touch.position;
                    _fingerDownPosition = touch.position;
                    _fingerPushed = true;
                    StartCoroutine(SwipeTimer());
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    Debug.Log("Should return to normal");
                    StartCoroutine(ReturnToNormalState(_delayToReturnCenterPosition));
                }
            }
            else
            {
                if (_gameManager.AntiStunTapCounter == 0)
                {
                    _player.IsStunned = false;
                    _gameManager.AntiStunTapCounter = -1;
                    if (_player.CurrentHitObstacle != null)
                    {
                        _player.CurrentHitObstacle.GetComponent<Obstacle>().Disappear();
                        _player.CurrentHitObstacle = null;
                    }
                    _gameManager.RestartSpeed();
                    StartCoroutine(_gameManager.GenerateMap());
                }
            }
        }
#elif UNITY_EDITOR
        if (!_player.IsStunned)
        {
            if (Input.GetMouseButtonDown(0) && !_fingerPushed)
            {
                _fingerUpPosition = Input.mousePosition;
                _fingerDownPosition = Input.mousePosition;
                _fingerPushed = true;
                StartCoroutine(SwipeTimer());
            }

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Should return to normal");
                StartCoroutine(ReturnToNormalState(_delayToReturnCenterPosition));
            }
        }
        else
        {
            if (_gameManager.AntiStunTapCounter >= 10)
            {
                _player.IsStunned = false;
                _gameManager.AntiStunTapCounter = -1;
                if (_player.CurrentHitObstacle != null)
                {
                    _player.CurrentHitObstacle.GetComponent<Obstacle>().Disappear();
                    _player.CurrentHitObstacle = null;
                }
                _gameManager.RestartSpeed();
            }
        }
#endif
    }

    private IEnumerator SwipeTimer()
    {
        Debug.Log("Started swipe timer");
        float currentFrameTime = 0;

        #if UNITY_ANDROID || UNITY_IOS
        while (_fingerPushed && Input.touchCount > 0)
        #elif UNITY_EDITOR
        while (_fingerPushed && Input.GetMouseButtonDown(0))
        #endif
        {
            if (currentFrameTime >= _delayToReadSwipe)
            {
                Debug.Log("Should detect swipe");

                    #if UNITY_ANDROID || UNITY_IOS
                    Touch touch = Input.touches[Input.touchCount - 1];
                    _fingerDownPosition = touch.position;
                    #elif UNITY_EDITOR
                    _fingerDownPosition = Input.mousePosition;
                    #endif

                DetectSwipe();
                break;
            }
            else
            {
                currentFrameTime += Time.deltaTime;
            }

            yield return null;
        }

        Debug.Log("Finished swipe timer");
        yield return null;
    }

    private IEnumerator ReturnToNormalState(float seconds)
    {
        if (_currentSwipeDirection == SwipeDirection.Left)
        {
            _player.PlayAnimation("EndLeft");
        }
        else if (_currentSwipeDirection == SwipeDirection.Right)
        {
            _player.PlayAnimation("EndRight");
        }

        _currentSwipeDirection = SwipeDirection.None;
        yield return new WaitForSeconds(seconds);
        _player.ReturnToNormal();
        _fingerPushed = false;
    }

    private void DetectSwipe()
    {
        SwipeDirection direction = _fingerDownPosition.x - _fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        if (direction == SwipeDirection.Left)
        {
            _player.PlayAnimation("StartLeft");
            _currentSwipeDirection = SwipeDirection.Left;
        }
        else if (direction == SwipeDirection.Right)
        {
            _player.PlayAnimation("StartRight");
            _currentSwipeDirection = SwipeDirection.Right;
        }
        else
        {
            _currentSwipeDirection = SwipeDirection.None;
        }
        Debug.Log("Swipe dirrection: " + direction);
        SendSwipe(direction);
        _fingerUpPosition = _fingerDownPosition;
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = _fingerDownPosition,
            EndPosition = _fingerUpPosition
        };
        Debug.Log("Should swipe to: " + swipeData.Direction);
        _player.TiltToSide(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    None,
    Left,
    Right,
}